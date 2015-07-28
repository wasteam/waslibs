using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.LocalStorage
{
    public class LocalStorageDataProvider<T> : DataProviderBase<LocalStorageDataConfig, T> where T : SchemaBase
    {
        public override async Task<IEnumerable<T>> LoadDataAsync(LocalStorageDataConfig config)
        {
            return await LoadDataAsync(config, new GenericParser<T>());
        }

        public override async Task<IEnumerable<T>> LoadDataAsync(LocalStorageDataConfig config, IParser<T> parser)
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

        private static void Assertions(LocalStorageDataConfig config, IParser<T> parser)
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
