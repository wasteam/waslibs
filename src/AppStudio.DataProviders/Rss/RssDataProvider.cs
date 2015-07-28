using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Rss
{
    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        public override async Task<IEnumerable<RssSchema>> LoadDataAsync(RssDataConfig config)
        {
            return await LoadDataAsync(config, new RssParser());
        }
        public override async Task<IEnumerable<RssSchema>> LoadDataAsync(RssDataConfig config, IParser<RssSchema> parser)
        {
            Assertions(config, parser);

            var settings = new HttpRequestSettings()
            {
                RequestedUri = config.Url
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private static void Assertions(RssDataConfig config, IParser<RssSchema> parser)
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
