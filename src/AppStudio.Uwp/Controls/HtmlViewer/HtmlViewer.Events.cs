using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private async void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null)
            {
                args.Cancel = true;
                await Windows.System.Launcher.LaunchUriAsync(args.Uri);
            }
        }

        private async void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _isHtmlLoaded = true;

            await _webView.LoadScriptAsync("AppStudio.Uwp.Controls.HtmlViewer.HtmlViewerScript.js");
            await SetHtmlDocumentMargin();

            _header.Visibility = IsHeaderVisible ? Visibility.Visible : Visibility.Collapsed;
            _footer.Visibility = IsFooterVisible ? Visibility.Visible : Visibility.Collapsed;

            _asideLeft.Visibility = DetermineASideLeftVisibility();
            _asideRight.Visibility = DetermineASideRightVisibility();

            await SetFontSize();
            await SetForeground();
            await SetContentAlignment(this.ContentAlignment);

            ArrangeParts(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));

            this.FadeIn();
        }

        private Visibility DetermineASideLeftVisibility()
        {
            if (this.ActualWidth > 1200)
            {
                return IsASideLeftVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        private Visibility DetermineASideRightVisibility()
        {
            if (this.ActualWidth > 800)
            {
                return IsASideRightVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        private void OnScriptNotify(object sender, NotifyEventArgs e)
        {
            ArrangeParts(e.Value);
        }

        private async void OnComplementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await SetHtmlDocumentMargin();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_clip != null)
            {
                _header.Width = GetPartWidth();
                _footer.Width = GetPartWidth();
                _asideLeft.Width = GetPartWidth();
                _asideRight.Width = GetPartWidth();
                _clip.Rect = new Rect(0, 0, this.ActualWidth - 10, this.ActualHeight);
            }
        }

        private async Task SetHtmlDocumentMargin()
        {
            if (_isHtmlLoaded & !DesignMode.DesignModeEnabled)
            {
                double headerHeight = IsHeaderVisible ? _header.ActualHeight : 0.0;

                // TODO: What if aside is higher than WebView? Calculate Max size.
                double footerHeight = IsFooterVisible ? _footer.ActualHeight : 0.0;

                double partWidth = GetPartWidth();

                double leftWidth = DetermineASideLeftVisibility() == Visibility.Visible ? partWidth : 0.0;
                double rightWidth = DetermineASideRightVisibility() == Visibility.Visible ? partWidth : 0.0;

                string margin = $"{headerHeight}px {rightWidth}px {footerHeight}px {leftWidth}px";
                await _webView.InvokeScriptAsync("setHtmlDocumentMargin", margin);

                ArrangeParts(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));
            }
        }

        private void ArrangeParts(string value)
        {
            string[] parts = value.Split('|');

            double y = parts[1].AdDouble();
            double hy = y - _header.ActualHeight * (IsHeaderVisible ? 1 : 0);
            double ly = hy;
            double ry = hy;
            double fy = y + parts[3].AdDouble();
            fy = Math.Max(fy, this.ActualHeight - _footer.ActualHeight);

            double partWidth = GetPartWidth();
            double ml = DetermineASideLeftVisibility() == Visibility.Visible ? partWidth : 0.0;
            double mr = this.ActualWidth - partWidth;

            _header.Width = GetPartWidth();
            _footer.Width = GetPartWidth();
            _asideLeft.Width = GetPartWidth();
            _asideRight.Width = GetPartWidth();

            _header.TranslateX(ml);
            _footer.TranslateX(ml);
            _asideLeft.TranslateX(0.0);
            _asideRight.TranslateX(mr);

            _header.TranslateY(hy);
            _footer.TranslateY(fy);
            _asideLeft.TranslateY(ly);
            _asideRight.TranslateY(ry);

            _asideLeft.Visibility = DetermineASideLeftVisibility();
            _asideRight.Visibility = DetermineASideRightVisibility();

            _header.Opacity = _header.ActualHeight > 0 ? (_header.ActualHeight + hy) / _header.ActualHeight : 0.0;
            _asideLeft.Opacity = _asideLeft.ActualHeight > 0 ? (_asideLeft.ActualHeight + hy) / _asideLeft.ActualHeight : 0.0;
            _asideRight.Opacity = _asideRight.ActualHeight > 0 ? (_asideRight.ActualHeight + hy) / _asideRight.ActualHeight : 0.0;
        }

        private double GetPartWidth()
        {
            int count = 1;
            if (DetermineASideLeftVisibility() == Visibility.Visible)
            {
                count++;
            }
            if (DetermineASideRightVisibility() == Visibility.Visible)
            {
                count++;
            }
            return this.ActualWidth / count;
        }
    }
}
