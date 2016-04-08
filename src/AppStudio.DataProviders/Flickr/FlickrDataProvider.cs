using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Rss;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataProvider : DataProviderBase<FlickrDataConfig, FlickrSchema>
    {
        const string BaseUrl = "http://api.flickr.com/services/feeds";

        object TotalItems { get; set; }

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
                RequestedUri = new Uri($"{BaseUrl}/photos_public.gne?{config.QueryType.ToString().ToLower()}={WebUtility.UrlEncode(config.Query)}")
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var items = parser.Parse(result.Result);
                if (items != null && items.Any())
                {
                    TotalItems = items.ToList();
                    var total = (TotalItems as IEnumerable<TSchema>);
                    var resultToReturn = total.Take(pageSize).ToList();
                    _hasMoreItems = total.Count() > pageSize;
                    ContinuationToken = GetContinuationToken(ContinuationToken);
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
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }

        private string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }

        private IEnumerable<TSchema> GetMoreData<TSchema>(int pageSize, int page)
        {
            if (TotalItems == null)
            {
                throw new InvalidOperationException("LoadMoreDataAsync can not be called. You must call the LoadDataAsync method prior to calling this method");
            }
            var total = (TotalItems as IEnumerable<TSchema>);
            var resultToReturn = total.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            return resultToReturn;
        }

    }

}

