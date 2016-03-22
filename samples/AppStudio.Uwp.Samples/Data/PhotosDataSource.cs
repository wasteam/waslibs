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
    class PhotosDataSource
    {
        static private IEnumerable<PhotoDataItem> _photos = null;
        static private IEnumerable<IEnumerable<PhotoDataItem>> _groupedPhotos = null;

        public IEnumerable<PhotoDataItem> GetItems()
        {
            return _photos;
        }

        public IEnumerable<IEnumerable<PhotoDataItem>> GetGroupedItems()
        {
            return _groupedPhotos;
        }

        static public async Task Load()
        {
            _photos = await GetPhotos();
            _groupedPhotos = _photos.GroupBy(x => x.Category);
        }

        private static async Task<IEnumerable<PhotoDataItem>> GetPhotos()
        {
            var uri = new Uri("ms-appx:///Assets/Photos.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return Parse(await r.ReadToEndAsync());                
            }
        }

        private static IEnumerable<PhotoDataItem> Parse(string jsonData)
        {
            return JsonConvert.DeserializeObject<IList<PhotoDataItem>>(jsonData);
        }
    }

    public class PhotoDataItem
    {
        public string Title { get; set; }
        public string Category { get; set; }        
        public string Thumbnail { get; set; }
    }
}
