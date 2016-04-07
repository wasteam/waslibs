using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Rss;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataProvider :DataProviderBase<FlickrDataConfig,  FlickrSchema>
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FlickrDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(string.Format("http://api.flickr.com/services/feeds/photos_public.gne?{0}={1}", config.QueryType.ToString().ToLower(), WebUtility.UrlEncode(config.Query)))
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var items = parser.Parse(result.Result);
                return items.Take(maxRecords).ToList();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<FlickrSchema> GetDefaultParserInternal(FlickrDataConfig config)
        {
            return new FlickrParser();
        }

        protected override Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(FlickrDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            throw new NotImplementedException();
        }

        protected override void ValidateConfig(FlickrDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }
    }

}

