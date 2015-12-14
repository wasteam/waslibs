using System;
using System.Threading.Tasks;

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

            await SetFontSize();
            await SetForeground();
            await SetContentAlignment(this.ContentAlignment);

            await SetHtmlDocumentMargin();

            this.FadeIn();
        }

        private void OnScriptNotify(object sender, NotifyEventArgs e)
        {
            ArrangeParts(e.Value);
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
