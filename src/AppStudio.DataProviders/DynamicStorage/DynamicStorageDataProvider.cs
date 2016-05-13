using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Linq;

namespace AppStudio.DataProviders.DynamicStorage
{
    public class DynamicStorageDataProvider<T> : DataProviderBase<DynamicStorageDataConfig, T> where T : SchemaBase
    {
        private bool _hasMoreItems;
        public override bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(DynamicStorageDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = config.PageIndex.ToString();
            return await GetDataFromProvider(config, pageSize, parser);
        }

        protected override IParser<T> GetDefaultParserInternal(DynamicStorageDataConfig config)
        {
            return new JsonParser<T>();
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(DynamicStorageDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            return await GetDataFromProvider(config, pageSize, parser);
        }

        protected override void ValidateConfig(DynamicStorageDataConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }

        private static string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }

        private async Task<IEnumerable<TSchema>> GetDataFromProvider<TSchema>(DynamicStorageDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
                  
            var settings = new HttpRequestSettings
            {
                RequestedUri = GetUrl(config, pageSize),
                UserAgent = "NativeHost"
            };

            settings.Headers["WAS-APPID"] = config.AppId;
            settings.Headers["WAS-STOREID"] = config.StoreId;
            settings.Headers["WAS-DEVICETYPE"] = config.DeviceType;
            settings.Headers["WAS-ISBACKGROUND"] = config.IsBackgroundTask.ToString();

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var items = await parser.ParseAsync(result.Result);
                _hasMoreItems = items.Any();
                ContinuationToken = GetContinuationToken(ContinuationToken);
                return items;
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private Uri GetUrl(DynamicStorageDataConfig config, int pageSize)
        {
            var sortDirection = config.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var url = $"{config.Url}&pageIndex={ContinuationToken}&blockSize={pageSize}";
            if (!string.IsNullOrEmpty(config.OrderBy))
            {
                url += $"&orderby={config.OrderBy}&sortdirection={sortDirection}";
            }
            return new Uri(url);
        }
    }
}
