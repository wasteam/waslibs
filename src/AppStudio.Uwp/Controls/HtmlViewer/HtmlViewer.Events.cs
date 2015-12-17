using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.System;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private async void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null)
            {
                args.Cancel = true;
                await Launcher.LaunchUriAsync(args.Uri);
            }
        }

        private async void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _isHtmlLoaded = true;

            await _webView.LoadScriptAsync("AppStudio.Uwp.Controls.HtmlViewer.HtmlViewerScript.js");

            var size = await _webView.InvokeScriptAsync("getHtmlDocumentRect");

            System.Diagnostics.Debug.WriteLine("OnNavigationCompleted " + size);

            _currentHeaderHeight = 0;
            _currentFooterHeight = 0;

            await SetFontSize();
            await SetForeground();
            await SetContentAlignment(this.ContentAlignment);

            await SetHtmlDocumentMargin();

            _progress.Visibility = Visibility.Collapsed;
            _frame.FadeIn(250);
        }

        private async void OnScriptNotify(object sender, NotifyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnScriptNotify " + e.Value);

            string value = e.Value;
            string[] parts = value.Substring(1).Split('|');

            _docX = parts[0].AsDouble();
            _docY = parts[1].AsDouble();
            _docWidth = parts[2].AsDouble();
            _docHeight = parts[3].AsDouble();

            if (value.StartsWith("R"))
            {
                System.Diagnostics.Debug.WriteLine("DocumentResize");
                await OnDocumentResize(_docX, _docY, _docWidth, _docHeight);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Scroll");
                OnDocumentScroll(_docY);
            }
        }

        private async Task SetFontSize()
        {
            if (_isHtmlLoaded)
            {
                await _webView.InvokeScriptAsync("setFontSize", this.FontSize + "px");
            }
        }

        private async Task SetForeground()
        {
            if (_isHtmlLoaded)
            {
                var solidBrush = this.Foreground as SolidColorBrush;
                if (solidBrush != null)
                {
                    await _webView.InvokeScriptAsync("setHtmlColor", "#" + solidBrush.Color.ToString().Substring(3));
                }
            }
        }
    }
}
