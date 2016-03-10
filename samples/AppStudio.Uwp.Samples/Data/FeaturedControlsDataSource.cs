using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Newtonsoft.Json;

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
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }        
        public string Thumbnail { get; set; }        
    }
}
