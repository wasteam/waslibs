using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppStudio.DataProviders.Rss
{

    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RssDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = config.Url
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {         
                var items = parser.Parse(result.Result); 
                return items.Take(maxRecords).ToList();              
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<RssSchema> GetDefaultParserInternal(RssDataConfig config)
        {
            return new RssParser();
        }

        protected override Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RssDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            throw new NotSupportedException();
        }

        protected override void ValidateConfig(RssDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }
    }
}
