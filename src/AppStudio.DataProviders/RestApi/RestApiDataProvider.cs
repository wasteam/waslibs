using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataProvider<TRestApiSchema> : DataProviderBase<RestApiDataConfig<TRestApiSchema>, TRestApiSchema> where TRestApiSchema : SchemaBase, new()
    {
        public override bool HasMoreItems
        {
            get
            {
                return ContinuationToken != Config?.Paginator?.ContinuationTokenInitialValue;
            }
        }        

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RestApiDataConfig<TRestApiSchema> config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = config?.Paginator?.ContinuationTokenInitialValue;
            var result = await GetAsync(config, pageSize, parser);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return result.Items;
            }
            return new TSchema[0];
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RestApiDataConfig<TRestApiSchema> config, int pageSize, IParser<TSchema> parser)
        {
            var result = await GetMoreAsync(config, pageSize, parser);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return result.Items;
            }

            return new TSchema[0];
        }

        protected override void ValidateConfig(RestApiDataConfig<TRestApiSchema> config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException(nameof(config.Url));
            }
            if (config.ItemParser == null)
            {
                throw new ConfigParameterNullException(nameof(config.ItemParser));
            }          
        }

        protected override IParser<TRestApiSchema> GetDefaultParserInternal(RestApiDataConfig<TRestApiSchema> config)
        {
            RestApiParser<TRestApiSchema> result = new RestApiParser<TRestApiSchema>(config.ElementsRootPath, config.ItemParser);           
            return result;
        }

        public async Task<HttpRequestResult<TSchema>> GetAsync<TSchema>(RestApiDataConfig<TRestApiSchema> config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            ContinuationToken = config?.Paginator?.ContinuationTokenInitialValue;
            var url = GetUrl(config, pageSize);
            var result = await HttpRequest.ExecuteGetAsync(new Uri(url), parser);
            return result;
        }

        public async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig<TRestApiSchema> config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var url = GetUrl(config, pageSize);
            var continuationUrl = GetContinuationUrl(url);
            var result = await HttpRequest.ExecuteGetAsync(new Uri(continuationUrl), parser);
            return result;
        }

        private string GetUrl<TSchema>(RestApiDataConfig<TSchema> config, int pageSize) where TSchema : SchemaBase
        {
            Uri uri = config.Url;
            var absoluteUri = uri.AbsoluteUri;
            if (!string.IsNullOrEmpty(config?.PageSizeParameterName))
            {
                if (string.IsNullOrEmpty(uri.Query))
                {
                    return $"{absoluteUri}?{config.PageSizeParameterName}={pageSize}";
                }
                return $"{absoluteUri}&{config.PageSizeParameterName}={pageSize}";
            }
            return absoluteUri;
        }

        private string GetContinuationUrl(string url)
        {
            return Config.Paginator?.GetContinuationUrl(url, ContinuationToken);
        }

        private string GetContinuationToken(string data)
        {
            return Config.Paginator?.GetContinuationToken(data, ContinuationToken);
        }        
    }
}
