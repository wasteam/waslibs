using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Samples
{
    class BingDataSource
    {
        static private IEnumerable<string> _imageUrls = null;

        public IEnumerable<object> GetItems()
        {
            return _imageUrls;
        }

        static public async Task Load()
        {
            _imageUrls = (await GetImageUrls()).Union(await GetImageUrls("es-ES")).Distinct();
        }

        public static async Task<string[]> GetImageUrls(string mkt = "en-US")
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"http://www.bing.com/HPImageArchive.aspx?format=xml&idx=10&n=20&mkt={mkt}";
                string xml = await httpClient.GetStringAsync(url);

                var xdoc = XDocument.Parse(xml);
                return GetImageUrls(xdoc).ToArray();
            }
        }

        private static IEnumerable<string> GetImageUrls(XDocument xdoc)
        {
            foreach (var item in xdoc.Descendants("images").Descendants("image").Descendants("url"))
            {
                yield return $"http://www.bing.com{item.Value}";
            }
        }
    }
}
