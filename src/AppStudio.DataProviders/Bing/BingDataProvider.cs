using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Bing
{
    public class BingDataProvider : DataProviderBase<BingDataConfig, BingSchema>
    {
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(string.Format("http://www.bing.com/search?q={0}&loc:{1}&format=rss", WebUtility.UrlEncode(config.Query), config.Country.GetStringValue()))
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        public override IParser<BingSchema> GetDefaultParser(BingDataConfig config)
        {
            return new BingParser();
        }

        protected override void ValidateConfig(BingDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }
    }
}
