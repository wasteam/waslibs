using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataProvider : DataProviderBase<FlickrDataConfig, FlickrSchema>
    {
        public override async Task<IEnumerable<FlickrSchema>> LoadDataAsync(FlickrDataConfig config)
        {
            return await LoadDataAsync(config, new FlickrParser());
        }

        public override async Task<IEnumerable<FlickrSchema>> LoadDataAsync(FlickrDataConfig config, IParser<FlickrSchema> parser)
        {
            Assertions(config, parser);

            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(string.Format("http://api.flickr.com/services/feeds/photos_public.gne?{0}={1}", config.QueryType.ToString().ToLower(), WebUtility.UrlEncode(config.Query)))
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private static void Assertions(FlickrDataConfig config, IParser<FlickrSchema> parser)
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
