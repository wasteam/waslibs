using System;
using System.Threading.Tasks;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
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
                await Launcher.LaunchUriAsync(args.Uri);
            }
        }

        private async void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _isHtmlLoaded = true;

            await _webView.LoadScriptAsync("AppStudio.Uwp.Controls.HtmlViewer.HtmlViewerScript.js");

            _currentHeaderHeight = 0;
            _currentFooterHeight = 0;

            await SetFontSize();
            await SetForeground();
            await SetContentAlignment(this.ContentAlignment);

            _documentSize = ParseRect(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));
            await SetHtmlDocumentMargin();

            _documentSize = ParseRect(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));
            await OnDocumentResize(_documentSize);

            _progress.IsActive = false;
            _progress.Visibility = Visibility.Collapsed;
            _frame.FadeIn(250);
        }

        private async void OnScriptNotify(object sender, NotifyEventArgs e)
        {
            string value = e.Value;
            if (!String.IsNullOrEmpty(value))
            {
                switch (value[0])
                {
                    case 'L':
                    case 'R':
                        _documentSize = ParseRect(value);
                        await OnDocumentResize(_documentSize);
                        break;
                    case 'S':
                        _documentSize = ParseRect(value);
                        OnDocumentScroll(_documentSize);
                        break;
                    default:
                        break;
                }
            }
        }

        private Rect ParseRect(string value)
        {
            string[] parts = value.Substring(1).Split('|');
            return new Rect(parts[0].AsDouble(), parts[1].AsDouble(), parts[2].AsDouble(), parts[3].AsDouble());
        }

        private async Task SetFontSize()
        {
            if (_isHtmlLoaded)
            {
                await _webView.InvokeScriptAsync("setFontSize", this.FontSize.ToInvariantString() + "px");
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
