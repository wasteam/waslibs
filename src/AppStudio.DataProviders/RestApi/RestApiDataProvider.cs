using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataProvider : DataProviderBase<RestApiDataConfig>
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
            var result = await HttpRequest.ExecuteGetAsync(config.Url, parser);
            return result;        
        }

        public async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var url = GetContinuationUrl(config.Url);
            var result = await HttpRequest.ExecuteGetAsync(new Uri(url), parser);
            return result;
        }

        private string GetContinuationUrl(Uri url)
        {
            return url.AbsolutePath;
        }

        private string GetContinuationToken(string data)
        {
            return string.Empty;
        }
    }
}
