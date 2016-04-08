using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppStudio.DataProviders.Rss
{

    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        object TotalItems { get; set; }

        bool _hasMoreItems = false;
        public override bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RssDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = "1";
            var settings = new HttpRequestSettings()
            {
                RequestedUri = config.Url
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

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(RssDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            int page = Convert.ToInt32(ContinuationToken);
            var task = Task.Run(() => { return GetMoreData<TSchema>(pageSize, page); });
            var items = await task;
            _hasMoreItems = items.Any();
            ContinuationToken = GetContinuationToken(ContinuationToken);
            return items;
        }

        protected override IParser<RssSchema> GetDefaultParserInternal(RssDataConfig config)
        {
            return new RssParser();
        }

        protected override void ValidateConfig(RssDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
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
