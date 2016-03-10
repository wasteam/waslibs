using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.Test
{
    public static class Common
    {
        public static async Task<string> ReadAssetFile(string fileName)
        {
            var uri = new Uri(string.Format("ms-appx://{0}", fileName));

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return await r.ReadToEndAsync();
            }
        }

        public static bool CheckHasHtmlTags(string text)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            return tagRegex.IsMatch(text);
        }
    }
}
