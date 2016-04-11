using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.LocalStorage;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.Html
{
    public class HtmlDataProvider : DataProviderBase<LocalStorageDataConfig, HtmlSchema>
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(LocalStorageDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            var uri = new Uri(string.Format("ms-appx://{0}", config.FilePath));

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return parser.Parse(await r.ReadToEndAsync());
            }
        }

        protected override IParser<HtmlSchema> GetDefaultParserInternal(LocalStorageDataConfig config)
        {
            return new HtmlParser();
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
