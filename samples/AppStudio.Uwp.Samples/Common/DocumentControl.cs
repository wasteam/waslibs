using System;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace AppStudio.Uwp.Samples
{
    public class DocumentControl : ContentControl
    {
        private WebView _webView;

        public DocumentControl()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.VerticalContentAlignment = VerticalAlignment.Stretch;

            _webView = new WebView();
            this.Content = new Border
            {
                Child = _webView,
                BorderThickness = new Thickness(0, 0, 0, 1),
                BorderBrush = new SolidColorBrush(Colors.LightGray)
            };
        }

        public async Task ShowHelp(Uri source)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(source);
            _webView.NavigateToString(await file.ReadTextAsync());
        }

        public async Task ShowCSharp(Uri source)
        {
            await ShowDocument(source, "CSharp.html");
        }

        public async Task ShowXaml(Uri source)
        {
            await ShowDocument(source, "Xaml.html");
        }

        public async Task ShowJson(Uri source)
        {
            await ShowDocument(source, "Json.html");
        }

        private async Task ShowDocument(Uri source, string pattern)
        {
            var patternFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Html/{pattern}"));
            var patternText = await patternFile.ReadTextAsync();

            var jsonFile = await StorageFile.GetFileFromApplicationUriAsync(source);
            var jsonText = await jsonFile.ReadTextAsync();
            jsonText = jsonText.Replace("<", "&lt;").Replace(">", "&gt;");

            string html = patternText.Replace("[CODE]", jsonText);
            _webView.NavigateToString(html);
        }
    }
}
