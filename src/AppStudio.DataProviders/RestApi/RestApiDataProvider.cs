using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataProvider : DataProviderBase<RestApiDataConfig>
    {
        object _totalItems;

        bool _hasMoreItems;

        public override bool HasMoreItems
        {
            get
            {
                if (Config?.PaginationConfig?.IsInMemory == true)
                {
                    return _hasMoreItems;
                }
                return ContinuationToken != Config?.PaginationConfig?.ContinuationTokenInitialValue;
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

            if (config.PaginationConfig == null)
            {
                throw new ConfigParameterNullException(nameof(config.PaginationConfig));
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
            ContinuationToken = config?.PaginationConfig?.ContinuationTokenInitialValue;
            var url = GetUrl(config, pageSize);
            var result = await GetDataFromProvider(new Uri(url), parser);

            if (Config?.PaginationConfig?.IsInMemory == true)
            {
                _totalItems = result.Items;
                var total = (_totalItems as IEnumerable<TSchema>);
                _hasMoreItems = total.Count() > pageSize;
                var items = total.Take(pageSize).ToList();
                result.Items = items;
            }
            return result;
        }

        private async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (Config?.PaginationConfig.IsInMemory == true)
            {
                int page = Convert.ToInt32(ContinuationToken);
                var total = (_totalItems as IEnumerable<TSchema>);
                var nextItems = total.Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                ContinuationToken = GetContinuationToken(string.Empty);
                var result = new HttpRequestResult<TSchema>();
                result.Items = nextItems;
                return result;
            }
            else
            {
                var url = GetUrl(config, PageSize);
                var uri = GetContinuationUrl(url);
                return await GetDataFromProvider(uri, parser);
            }
        }

        private async Task<HttpRequestResult<TSchema>> GetDataFromProvider<TSchema>(Uri uri, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var result = await HttpRequest.ExecuteGetAsync(uri, parser);
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
            var pageSizeParameterName = config?.PaginationConfig?.PageSizeParameterName;
            if (!string.IsNullOrEmpty(pageSizeParameterName))
            {
                if (string.IsNullOrEmpty(uri.Query))
                {
                    return $"{absoluteUri}?{pageSizeParameterName}={pageSize}";
                }
                return $"{absoluteUri}&{pageSizeParameterName}={pageSize}";
            }
            return absoluteUri;
        }

        private Uri GetContinuationUrl(string url)
        {
            var uri = new Uri(url);
            return Config.PaginationConfig?.GetContinuationUrl(uri, ContinuationToken);
        }

        private string GetContinuationToken(string data)
        {
            return Config.PaginationConfig?.GetContinuationToken(data, ContinuationToken);
        }
    }
}
