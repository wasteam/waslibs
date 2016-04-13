using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataProvider<TSchema0> : DataProviderBase<RestApiDataConfig<TSchema0>, TSchema0> where TSchema0 : SchemaBase, new()
    {
        public override bool HasMoreItems
        {
            get
            {
                return ContinuationToken != Config?.Paginator?.ContinuationTokenInitialValue;
            }
        }        

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RestApiDataConfig<TSchema0> config, int pageSize, IParser<TSchema> parser)
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

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RestApiDataConfig<TSchema0> config, int pageSize, IParser<TSchema> parser)
        {
            var result = await GetMoreAsync(config, pageSize, parser);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return result.Items;
            }

            return new TSchema[0];
        }

        protected override void ValidateConfig(RestApiDataConfig<TSchema0> config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException(nameof(config.Url));
            }
            if (config.ItemParser == null)
            {
                throw new ConfigParameterNullException(nameof(config.ItemParser));
            }
            if (config.Paginator == null)
            {
                throw new ConfigParameterNullException(nameof(config.Paginator));
            }
        }

        protected override IParser<TSchema0> GetDefaultParserInternal(RestApiDataConfig<TSchema0> config)
        {
            RestApiParser<TSchema0> result = new RestApiParser<TSchema0>(config.ElementsRootPath, config.ItemParser);           
            return result;
        }

        public async Task<HttpRequestResult<TSchema>> GetAsync<TSchema>(RestApiDataConfig<TSchema0> config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            ContinuationToken = config?.Paginator?.ContinuationTokenInitialValue;
            var url = GetUrl(config, pageSize);
            var result = await HttpRequest.ExecuteGetAsync(new Uri(url), parser);
            return result;
        }

        public async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig<TSchema0> config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
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
            if (!string.IsNullOrEmpty(config?.ItemsPerPageParameterName))
            {
                if (string.IsNullOrEmpty(uri.Query))
                {
                    return $"{absoluteUri}?{config.ItemsPerPageParameterName}={pageSize}";
                }
                return $"{absoluteUri}&{config.ItemsPerPageParameterName}={pageSize}";
            }
            return absoluteUri;
        }

        private string GetContinuationUrl(string url)
        {
            return Config.Paginator.GetContinuationUrl(url, ContinuationToken);
        }

        private string GetContinuationToken(string data)
        {
            return Config.Paginator.GetNextContinuationToken(data, ContinuationToken);
        }        
    }
}
