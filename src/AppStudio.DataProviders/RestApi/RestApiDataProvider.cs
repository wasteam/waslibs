using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataProvider : DataProviderBase<RestApiDataConfig, RestApiSchema>
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            var result = await GetAsync<TSchema>(config, pageSize, parser);   
            if (result.Success)
            {
                return result.Items;
            }

            return new TSchema[0];      
        }
        protected override IParser<RestApiSchema> GetDefaultParserInternal(RestApiDataConfig config)
        {
            return new RestApiParser();
        }

        protected override Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            throw new NotImplementedException();
        }

        protected override void ValidateConfig(RestApiDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }

        public async Task<HttpRequestResult<TSchema>> GetAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = config.Url
            };

            HttpRequestResult httpResult = await HttpRequest.DownloadAsync(settings);
            HttpRequestResult<TSchema> result;
            result = new HttpRequestResult<TSchema>(httpResult);
            if (httpResult.Success)
            {
                var items = parser.Parse(httpResult.Result);
                result.Items = items;
            }

            return result;
        }
    }
}
