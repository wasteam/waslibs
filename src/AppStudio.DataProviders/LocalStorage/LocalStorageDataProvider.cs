using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Linq;

namespace AppStudio.DataProviders.LocalStorage
{
    public class LocalStorageDataProvider<T> : DataProviderBase<LocalStorageDataConfig, T> where T : SchemaBase
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(LocalStorageDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var uri = new Uri(string.Format("ms-appx://{0}", config.FilePath));

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                var items = parser.Parse(await r.ReadToEndAsync());
                return items.Take(maxRecords).ToList();
            }
        }

        protected override IParser<T> GetDefaultParserInternal(LocalStorageDataConfig config)
        {
            return new JsonParser<T>();
        }

        protected override Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(LocalStorageDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            throw new NotSupportedException();
        }

        protected override void ValidateConfig(LocalStorageDataConfig config)
        {
            if (config.FilePath == null)
            {
                throw new ConfigParameterNullException("FilePath");
            }
        }
    }
}
