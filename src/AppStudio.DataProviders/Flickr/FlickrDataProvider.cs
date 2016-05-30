using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Linq;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataProvider : DataProviderBase<FlickrDataConfig, FlickrSchema>
    {
        const string BaseUrl = "http://api.flickr.com/services/feeds";

        object _totalItems;

        bool _hasMoreItems = false;
        public override bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FlickrDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = "1";
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(GetUrl(config))
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var items = await parser.ParseAsync(result.Result);
                if (items != null && items.Any())
                {
                    _totalItems = items.ToList();

                    var totalAsTSchema = (_totalItems as IEnumerable<TSchema>);

                    _hasMoreItems = totalAsTSchema.Count() > pageSize;
                    ContinuationToken = GetContinuationToken(ContinuationToken);

                    var resultToReturn = totalAsTSchema.AsQueryable().OrderBy(config.OrderBy, config.OrderDirection).Take(pageSize).ToList();
                    return resultToReturn;
                }
                _hasMoreItems = false;
                return new TSchema[0];
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }      

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(FlickrDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            int page = Convert.ToInt32(ContinuationToken);
            var task = Task.Run(() => { return GetMoreData<TSchema>(pageSize, page); });
            var items = await task;

            _hasMoreItems = items.Any();
            ContinuationToken = GetContinuationToken(ContinuationToken);

            return items;
        }

        protected override IParser<FlickrSchema> GetDefaultParserInternal(FlickrDataConfig config)
        {
            return new FlickrParser();
        }

        protected override void ValidateConfig(FlickrDataConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }

        private static string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }

        private static string GetUrl(FlickrDataConfig config)
        {
            var result = $"{BaseUrl}/photos_public.gne?{config.QueryType.ToString().ToLower()}={WebUtility.UrlEncode(config.Query)}";
            if (config.QueryType == FlickrQueryType.Tags)
            {
                result += "&tagmode=any";
            }
            return result;
        }

        private IEnumerable<TSchema> GetMoreData<TSchema>(int pageSize, int page)
        {
            if (_totalItems == null)
            {
                throw new InvalidOperationException("LoadMoreDataAsync can not be called. You must call the LoadDataAsync method prior to calling this method");
            }
            var totalAsTSchema = (_totalItems as IEnumerable<TSchema>);
            var resultToReturn = totalAsTSchema.AsQueryable().OrderBy(Config.OrderBy, Config.OrderDirection).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            return resultToReturn;
        }        
    }
}

