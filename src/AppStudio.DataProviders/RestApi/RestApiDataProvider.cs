using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

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
            var result = await GetAsync(config, pageSize, parser);
            return result.Items;
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            var result = await GetMoreAsync(config, pageSize, parser);
            return result.Items;
        }

        protected override void ValidateConfig(RestApiDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException(nameof(config.Url));
            }
        }

        public async Task<HttpRequestResult<TSchema>> GetApiDataAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            ValidateConfig(config);

            Parser = parser;
            Config = config;
            PageSize = pageSize;

            return await GetAsync(config, pageSize, parser);
        }

        public async Task<HttpRequestResult<TSchema>> GetMoreApiDataAsync<TSchema>() where TSchema : SchemaBase
        {
            if (Config == null || Parser == null)
            {
                throw new InvalidOperationException("GetMoreAsync can not be called. You must call the GetAsync method prior to calling this method");
            }

            var parser = Parser as IParser<TSchema>;
            return await GetMoreAsync(Config, PageSize, parser);
        }

        private async Task<HttpRequestResult<TSchema>> GetAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            ContinuationToken = config?.Pager?.ContinuationTokenInitialValue;
            var url = GetUrl(config, pageSize);
            return await GetDataFromProvider(url, parser);
        }

        private async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var url = GetUrl(Config, PageSize);
            var uri = GetContinuationUrl(url);
            return await GetDataFromProvider(url, parser);
        }

        private async Task<HttpRequestResult<TSchema>> GetDataFromProvider<TSchema>(string url, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var result = await HttpRequest.ExecuteGetAsync(new Uri(url), parser);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
            }
            return result;
        }

        private static string GetUrl(RestApiDataConfig config, int pageSize)
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

        private Uri GetContinuationUrl(string url)
        {
            var uri = new Uri(url);
            return Config.Pager?.GetContinuationUrl(uri, ContinuationToken);
        }

        private string GetContinuationToken(string data)
        {
            return Config.Pager?.GetContinuationToken(data, ContinuationToken);
        }
    }
}
