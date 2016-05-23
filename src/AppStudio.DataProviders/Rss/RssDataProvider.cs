using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;


namespace AppStudio.DataProviders.Rss
{
    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        object _totalItems;

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

            HttpRequestResult result = await HttpRequest.DownloadRssAsync(settings);
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
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }

        private static string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
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
