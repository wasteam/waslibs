using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.DynamicStorage
{
    public class DynamicStorageDataProvider<T> : DataProviderBase<DynamicStorageDataConfig, T> where T : SchemaBase
    {
        public override async Task<IEnumerable<T>> LoadDataAsync(DynamicStorageDataConfig config)
        {
            return await LoadDataAsync(config, new GenericParser<T>());
        }

        public override async Task<IEnumerable<T>> LoadDataAsync(DynamicStorageDataConfig config, IParser<T> parser)
        { 
            Assertions(config, parser);

            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(string.Format("{0}&pageIndex={1}&blockSize={2}", config.Url, config.PageIndex, config.BlockSize)),
                UserAgent = "NativeHost"
            };

            settings.Headers["WAS-APPID"] = config.AppId;
            settings.Headers["WAS-STOREID"] = config.StoreId;
            settings.Headers["WAS-DEVICETYPE"] = config.DeviceType;
            settings.Headers["WAS-ISBACKGROUND"] = config.IsBackgroundTask.ToString();

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private static void Assertions(DynamicStorageDataConfig config, IParser<T> parser)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }
    }
}
