using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.Uwp.Samples
{
    class DevicesDataSource
    {
        static private IEnumerable<DeviceDataItem> _devices = null;
        public IEnumerable<object> GetItems()
        {
            return _devices;
        }

        static public async void Load()
        {
            _devices = await GetDevices();
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
            return JsonConvert.DeserializeObject<IEnumerable<DeviceDataItem>>(jsonData);
        }
    }
    class DeviceDataItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public List<string> Images { get; set; }
    }
}
