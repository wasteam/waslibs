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
    class DevicesDataSource
    {
        static private IEnumerable<DeviceDataItem> _devices = null;
        static private IEnumerable<IEnumerable<DeviceDataItem>> _groupedDevices = null;

        public IEnumerable<DeviceDataItem> GetItems()
        {
            return _devices;
        }

        public IEnumerable<IEnumerable<DeviceDataItem>> GetGroupedItems()
        {
            return _groupedDevices;
        }

        static public async Task Load()
        {
            _devices = await GetDevices();
            _groupedDevices = _devices.GroupBy(x => x.Category);
        }

        private static async Task<IEnumerable<DeviceDataItem>> GetDevices()
        {
            var uri = new Uri("ms-appx:///Assets/Devices.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return Parse(await r.ReadToEndAsync());                
            }
        }

        private static IEnumerable<DeviceDataItem> Parse(string jsonData)
        {
            return JsonConvert.DeserializeObject<IList<DeviceDataItem>>(jsonData);
        }
    }
    public class DeviceDataItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }        
        public string Thumbnail { get; set; }
        public List<string> Images { get; set; }
    }
}
