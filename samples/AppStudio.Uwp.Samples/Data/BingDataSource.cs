using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Samples
{
    class BingDataSource
    {
        static private IEnumerable<BingDataItem> _items = null;

        public IEnumerable<BingDataItem> GetItems()
        {
            return _items;
        }

        public static async Task Load()
        {
            _items = (await ReadItems()).Union(await ReadItems("es-ES")).Distinct();
        }

        private static async Task<BingDataItem[]> ReadItems(string mkt = "en-US")
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"http://www.bing.com/HPImageArchive.aspx?format=xml&idx=10&n=20&mkt={mkt}";
                string xml = await httpClient.GetStringAsync(url);

                var xdoc = XDocument.Parse(xml);
                return ReadItems(xdoc).ToArray();
            }
        }

        private static IEnumerable<BingDataItem> ReadItems(XDocument xdoc)
        {
            foreach (var item in xdoc.Descendants("images").Descendants("image"))
            {
                yield return new BingDataItem
                {
                    ImageUrl = $"http://www.bing.com{item.Element("url").Value}",
                    Copyright = item.Element("copyright").Value
                };
            }
        }
    }

    public class BingDataItem
    {
        public string ImageUrl { get; set; }
        public string Copyright { get; set; }
    }
}
