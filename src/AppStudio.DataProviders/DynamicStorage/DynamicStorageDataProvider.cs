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
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }

        private string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }

        private async Task<IEnumerable<TSchema>> GetDataFromProvider<TSchema>(DynamicStorageDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(string.Format("{0}&pageIndex={1}&blockSize={2}", config.Url, ContinuationToken, pageSize)),
                UserAgent = "NativeHost"
            };

            settings.Headers["WAS-APPID"] = config.AppId;
            settings.Headers["WAS-STOREID"] = config.StoreId;
            settings.Headers["WAS-DEVICETYPE"] = config.DeviceType;
            settings.Headers["WAS-ISBACKGROUND"] = config.IsBackgroundTask.ToString();

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var items = parser.Parse(result.Result);
                _hasMoreItems = items.Any();
                ContinuationToken = GetContinuationToken(ContinuationToken);
                return items;            
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }
    }
}
