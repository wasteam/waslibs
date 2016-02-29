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
            var patternFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Html/CSharp.html"));
            var patternText = await patternFile.ReadTextAsync();

            var csharpFile = await StorageFile.GetFileFromApplicationUriAsync(source);
            var csharpText = await csharpFile.ReadTextAsync();
            csharpText = csharpText.Replace("<", "&lt;").Replace(">", "&gt;");

            string html = patternText.Replace("[CODE]", csharpText);
            _webView.NavigateToString(html);
        }

        public async Task ShowXaml(Uri source)
        {
            var patternFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Html/Xaml.html"));
            var patternText = await patternFile.ReadTextAsync();

            var xamlFile = await StorageFile.GetFileFromApplicationUriAsync(source);
            var xamlText = await xamlFile.ReadTextAsync();
            xamlText = xamlText.Replace("<", "&lt;").Replace(">", "&gt;");

            string html = patternText.Replace("[CODE]", xamlText);
            _webView.NavigateToString(html);
        }
    }
}
