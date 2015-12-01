using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace AppStudio.Uwp
{
    public static class WebViewExtensions
    {
        public static async Task LoadAsync(this WebView webView, string path)
        {
            string url = String.Format("ms-appx:///{0}", path);
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(url));
            var randomStream = await file.OpenReadAsync();

            using (var reader = new StreamReader(randomStream.AsStreamForRead()))
            {
                webView.NavigateToString(await reader.ReadToEndAsync());
            }
        }

        public static async Task LoadScriptAsync(this WebView webView, string path)
        {
            var assembly = typeof(WebViewExtensions).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    await EvalScriptAsync(webView, reader.ReadToEnd());
                }
            }
        }

        public static async Task EvalScriptAsync(this WebView webView, string script, params string[] scripts)
        {
            await webView.InvokeScriptAsync("eval", new[] { script });
            foreach (var scr in scripts)
            {
                await webView.InvokeScriptAsync("eval", new[] { scr });
            }
        }

        public static async Task<string> InvokeScriptAsync(this WebView webView, string scriptName)
        {
            return await webView.InvokeScriptAsync(scriptName, null);
        }
        public static async Task<string> InvokeScriptAsync(this WebView webView, string scriptName, object value)
        {
            return await webView.InvokeScriptAsync(scriptName, new string[] { value.ToString() });
        }
    }
}
