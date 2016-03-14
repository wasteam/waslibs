using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Globalization;

namespace AppStudio.Uwp.Samples
{
    class FeaturedControlsDataSource
    {
        static private IEnumerable<ControlDataItem> _featuredControls = null;

        public IEnumerable<ControlDataItem> GetItems()
        {
            return _featuredControls;
        }

        static public async Task Load()
        {
            _featuredControls = await GetDevices();
        }

        private static async Task<IEnumerable<ControlDataItem>> GetDevices()
        {
            var uri = new Uri("ms-appx:///Assets/FeaturedControls.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return Parse(await r.ReadToEndAsync());                
            }
        }

        private static IEnumerable<ControlDataItem> Parse(string jsonData)
        {
            return JsonConvert.DeserializeObject<IList<ControlDataItem>>(jsonData);
        }
    }
    public class ControlDataItem
    {
        public int Index { get; set; }
        public string Title { get; set; }        
        public string Category { get; set; }        
        public string Thumbnail { get; set; }
        public string Color { get; set; }
        public string DetailPageName { get; set; }
        public Brush Background
        {
            get
            {
                if (string.IsNullOrEmpty(Color))
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
                return new SolidColorBrush(new Color()
                {
                    A = 255,
                    R = Convert.ToByte(Color.Substring(1, 2), 16),
                    G = Convert.ToByte(Color.Substring(3, 2), 16),
                    B = Convert.ToByte(Color.Substring(5, 2), 16)
                });
            }
        }        
    }
}
