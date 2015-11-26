using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

            await _webView.LoadScriptAsync("AppStudio.Uwp.Controls.HtmlViewer.HtmlViewer.js");
            await SetHtmlDocumentMargin();

            _header.Visibility = IsHeaderVisible ? Visibility.Visible : Visibility.Collapsed;
            _footer.Visibility = IsFooterVisible ? Visibility.Visible : Visibility.Collapsed;

            SetFontSize();
            SetForeground();

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
                _footer.Width = this.ActualWidth;
                _clip.Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            }
        }

        private async Task SetHtmlDocumentMargin()
        {
            if (_isHtmlLoaded)
            {
                double headerHeight = IsHeaderVisible ? _header.ActualHeight : 0.0;
                double footerHeight = IsFooterVisible ? _footer.ActualHeight : 0.0;

                string margin = $"{headerHeight}px 0px {footerHeight}px 0px";
                await _webView.InvokeScriptAsync("setHtmlDocumentMargin", margin);
            }
        }

        private void ArrangeHeaderFooter(string value)
        {
            string[] parts = value.Split('|');

            double y = GetSafeDouble(parts[1]);
            double hy = y - _header.ActualHeight;
            double fy = y + GetSafeDouble(parts[3]);

            fy = Math.Max(fy, this.ActualHeight - _footer.ActualHeight);
            _header.TranslateY(hy);
            _footer.TranslateY(fy);

            _header.Opacity = _header.ActualHeight > 0 ? (_header.ActualHeight + hy) / _header.ActualHeight : 0.0;
        }

        #region GetSafeDouble
        static private double GetSafeDouble(string str)
        {
            double d = 0.0;
            Double.TryParse(str, out d);
            return d;
        }
        #endregion
    }
}
