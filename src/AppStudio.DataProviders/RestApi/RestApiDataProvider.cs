using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Net;

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
                if (Config?.PaginationConfig?.IsServerSidePagination == false)
                {
                    return _hasMoreItems;
                }
                return ContinuationToken != Config?.PaginationConfig?.TokenInitialValue.ToString();
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
            if (config == null)
            {
                throw new ConfigNullException();
            }
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
            ContinuationToken = config?.PaginationConfig?.TokenInitialValue.ToString();
            var url = GetUrl(config, pageSize);
            var result = await GetDataFromProvider(new Uri(url), parser, config.Headers);

            if (Config?.PaginationConfig?.IsServerSidePagination == false)
            {
                _totalItems = result.Items;

                var totalAsTSchema = (_totalItems as IEnumerable<TSchema>);
                _hasMoreItems = totalAsTSchema.Count() > pageSize;
                var pagination = config?.PaginationConfig as IMemorySorting;
                if (pagination != null)
                {
                    result.Items = totalAsTSchema.AsQueryable().OrderBy(pagination.OrderBy, pagination.SortDirection).Take(pageSize).ToList();
                }
                else
                {
                    result.Items = totalAsTSchema.Take(pageSize).ToList();
                }               
            }
            return result;
        }

        private async Task<HttpRequestResult<TSchema>> GetMoreAsync<TSchema>(RestApiDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (Config?.PaginationConfig.IsServerSidePagination == false)
            {
                int page = Convert.ToInt32(ContinuationToken);
                var totalAsTSchema = (_totalItems as IEnumerable<TSchema>);               

                ContinuationToken = GetContinuationToken(string.Empty);
                var result = new HttpRequestResult<TSchema>();

                var pagination = config?.PaginationConfig as IMemorySorting;
                if (pagination != null)
                {
                    result.Items = totalAsTSchema.AsQueryable().OrderBy(pagination.OrderBy, pagination.SortDirection).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                }
                else
                {
                    result.Items = totalAsTSchema.Skip(PageSize * (page - 1)).Take(PageSize).ToList();
                }     
                return result;
            }
            else
            {
                var url = GetUrl(config, PageSize);
                var uri = GetContinuationUrl(url);
                return await GetDataFromProvider(uri, parser, config.Headers);
            }
        }

        private async Task<HttpRequestResult<TSchema>> GetDataFromProvider<TSchema>(Uri uri, IParser<TSchema> parser, IDictionary<string, string> headers) where TSchema : SchemaBase
        {

            if (uri == null)
            {
                return new HttpRequestResult<TSchema>();
            }
            var result = await HttpRequest.ExecuteGetAsync(parser, uri, headers);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return result;
            }
            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private static string GetUrl(RestApiDataConfig config, int pageSize)
        {
            Uri uri = config.Url;
            var absoluteUri = uri.AbsoluteUri;
            var query = uri.ParseQueryString();          

            var pageSizeParameterName = config?.PaginationConfig?.PageSizeParameterName;            
            if (!string.IsNullOrEmpty(pageSizeParameterName))
            {
                query[pageSizeParameterName] = pageSize.ToString();  
            }

            var pagination = config?.PaginationConfig as IQueryStringSorting;
            if (pagination != null)
            {
                string orderByName = pagination.OrderByParameterName;
                string orderByValue = pagination.OrderByValue;
                if (!string.IsNullOrEmpty(orderByName) && !string.IsNullOrEmpty(orderByValue))
                {
                    query[orderByName] = orderByValue;
                }

                string sortDirectionName = pagination.SortDirectionParameterName;
                string sortDirectionValue = pagination.SortDirectionValue;
                if (!string.IsNullOrEmpty(sortDirectionName) && !string.IsNullOrEmpty(sortDirectionValue))
                {
                    query[sortDirectionName] = sortDirectionValue;
                }
            }

            var queryString = query.ToQueryString();
            if (string.IsNullOrEmpty(uri.Query))
            {
                return $"{absoluteUri}{queryString}";
            }
            
            var uriWithoutQuery = absoluteUri.Replace(uri.Query, string.Empty);
            return $"{uriWithoutQuery}{queryString}";
        }

        private Uri GetContinuationUrl(string url)
        {
            var uri = new Uri(url);
            return Config.PaginationConfig?.BuildContinuationUrl(uri, ContinuationToken);
        }

        private string GetContinuationToken(string data)
        {
            return Config.PaginationConfig?.GetContinuationToken(data, ContinuationToken);
        }       
    }
}
