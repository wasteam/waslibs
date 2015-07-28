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
        public override async Task<IEnumerable<BingSchema>> LoadDataAsync(BingDataConfig config)
        {
            return await LoadDataAsync(config, new BingParser());
        }

        public override async Task<IEnumerable<BingSchema>> LoadDataAsync(BingDataConfig config, IParser<BingSchema> parser)
        {
            Assertions(config, parser);

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

        private static void Assertions(BingDataConfig config, IParser<BingSchema> parser)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }
    }
}
