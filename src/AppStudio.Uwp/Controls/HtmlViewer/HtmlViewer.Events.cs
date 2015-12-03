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

            await SetContentAlignment(this.ContentAlignment);
            await SetFontSize();
            await SetForeground();

            ArrangeHeaderFooter(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));

            this.FadeIn();
        }

        private void OnScriptNotify(object sender, NotifyEventArgs e)
        {
            ArrangeHeaderFooter(e.Value);
        }

        private async void OnHeaderFooterSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await SetHtmlDocumentMargin();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_clip != null)
            {
                _header.Width = this.ActualWidth;
                _footer.Width = this.ActualWidth;
                _clip.Rect = new Rect(0, 0, this.ActualWidth - 10, this.ActualHeight);
            }
        }

        private async Task SetHtmlDocumentMargin()
        {
            if (_isHtmlLoaded & !DesignMode.DesignModeEnabled)
            {
                double headerHeight = IsHeaderVisible ? _header.ActualHeight : 0.0;
                double footerHeight = IsFooterVisible ? _footer.ActualHeight : 0.0;

                string margin = $"{headerHeight}px 0px {footerHeight}px 0px";
                await _webView.InvokeScriptAsync("setHtmlDocumentMargin", margin);

                ArrangeHeaderFooter(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));
            }
        }

        private void ArrangeHeaderFooter(string value)
        {
            string[] parts = value.Split('|');

            double y = parts[1].AdDouble();
            double hy = y - _header.ActualHeight;
            double fy = y + parts[3].AdDouble();

            fy = Math.Max(fy, this.ActualHeight - _footer.ActualHeight);
            _header.TranslateY(hy);
            _footer.TranslateY(fy);

            _header.Opacity = _header.ActualHeight > 0 ? (_header.ActualHeight + hy) / _header.ActualHeight : 0.0;
        }
    }
}
