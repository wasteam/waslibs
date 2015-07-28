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

        public override async Task<IEnumerable<HtmlSchema>> LoadDataAsync(LocalStorageDataConfig config)
        {
            return await LoadDataAsync(config, new HtmlParser());
        }

        public override async Task<IEnumerable<HtmlSchema>> LoadDataAsync(LocalStorageDataConfig config, IParser<HtmlSchema> parser)
        {
            Assertions(config, parser);

            var uri = new Uri(string.Format("ms-appx://{0}", config.FilePath));

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return parser.Parse(await r.ReadToEndAsync());
            }
        }

        public override bool IsLocal
        {
            get
            {
                return true;
            }
        }

        private static void Assertions(LocalStorageDataConfig config, IParser<HtmlSchema> parser)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            if (config.FilePath == null)
            {
                throw new ConfigParameterNullException("FilePath");
            }
        }
    }
}
