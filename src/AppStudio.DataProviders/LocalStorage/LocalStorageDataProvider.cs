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
        object _totalItems;


        bool _hasMoreItems = false;
        public override bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(LocalStorageDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = "1";
            var uri = new Uri(string.Format("ms-appx://{0}", config.FilePath));

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                var items = await parser.ParseAsync(await r.ReadToEndAsync());
                if (items != null && items.Any())
                {
                    _totalItems = items.ToList();
                    var total = (_totalItems as IEnumerable<TSchema>);
                    var resultToReturn = total.Take(pageSize).ToList();
                    _hasMoreItems = total.Count() > pageSize;
                    ContinuationToken = GetContinuationToken(ContinuationToken);
                    return resultToReturn;
                }
                _hasMoreItems = false;
                return new TSchema[0];
            }
        }

        protected override IParser<T> GetDefaultParserInternal(LocalStorageDataConfig config)
        {
            return new JsonParser<T>();
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(LocalStorageDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            int page = Convert.ToInt32(ContinuationToken);
            var task = Task.Run(() => { return GetMoreData<TSchema>(pageSize, page); });
            var items = await task;
            _hasMoreItems = items.Any();
            ContinuationToken = GetContinuationToken(ContinuationToken);
            return items;
        }

        protected override void ValidateConfig(LocalStorageDataConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (config.FilePath == null)
            {
                throw new ConfigParameterNullException("FilePath");
            }
        }

        private static string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }

        private IEnumerable<TSchema> GetMoreData<TSchema>(int pageSize, int page)
        {
            if (_totalItems == null)
            {
                throw new InvalidOperationException("LoadMoreDataAsync can not be called. You must call the LoadDataAsync method prior to calling this method");
            }
            var total = (_totalItems as IEnumerable<TSchema>);
            var resultToReturn = total.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            return resultToReturn;
        }
    }
}
