using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.RestApi
{  
    public class RestApiDataProvider : DataProviderBase<RestApiDataConfig>
    {     
        public override bool HasMoreItems
        {
            get
            {
                return ContinuationToken != Config?.Pager?.ContinuationTokenInitialValue;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = config?.Pager?.ContinuationTokenInitialValue;
            var result = await GetAsync(config, pageSize, parser);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return result.Items;
            }
            return new TSchema[0];
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            var result = await GetMoreAsync(config, pageSize, parser);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return result.Items;
            }

            return new TSchema[0];
        }

        protected override void ValidateConfig(RestApiDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException(nameof(config.Url));
            }
        }       

        public async Task<HttpRequestResult<TSchema>> GetAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            ContinuationToken = config?.Pager?.ContinuationTokenInitialValue;
            var url = GetUrl(config, pageSize);
            var result = await HttpRequest.ExecuteGetAsync(new Uri(url), parser);
            return result;
        }

        public async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var url = GetUrl(config, pageSize);
            var continuationUrl = GetContinuationUrl(url);
            var result = await HttpRequest.ExecuteGetAsync(new Uri(continuationUrl), parser);
            return result;
        }

        private string GetUrl(RestApiDataConfig config, int pageSize)
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
            return Config.Pager?.GetContinuationUrl(url, ContinuationToken);
        }

        private string GetContinuationToken(string data)
        {
            return Config.Pager?.GetContinuationToken(data, ContinuationToken);
        }
    }
}
