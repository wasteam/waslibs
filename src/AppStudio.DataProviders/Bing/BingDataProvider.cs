using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Bing
{
    public class BingDataProvider : DataProviderBase<BingDataConfig, BingSchema>
    {
        private const string BaseUrl = "http://www.bing.com";

        bool _hasMoreItems = false;
        public override bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = "1";
            var url = GetSearchUrl(config, pageSize);
            return await GetDataFromProvider<TSchema>(parser, url, pageSize);
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(BingDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            var url = GetSearchUrl(config, pageSize);
            var continuationUrl = GetContinuationUrl(url);
            return await GetDataFromProvider<TSchema>(parser, continuationUrl, pageSize);
        }

        protected override IParser<BingSchema> GetDefaultParserInternal(BingDataConfig config)
        {
            return new BingParser();
        }

        protected override void ValidateConfig(BingDataConfig config)
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

        private static string GetSearchUrl(BingDataConfig config, int pageSize)
        {
            var countryValue = config.Country.GetStringValue();
            var locParameter = string.IsNullOrEmpty(countryValue) ? string.Empty : $"loc:{countryValue}+";
            var url = $"{BaseUrl}/search?q={locParameter}{ WebUtility.UrlEncode(config.Query)}&format=rss&count={pageSize}";
            return url;
        }

        private static string GetContinuationToken(string currentToken, int pageSize)
        {
            var token = (Convert.ToInt32(currentToken) + pageSize).ToString();
            return token;
        }

        private string GetContinuationUrl(string url)
        {
            url += $"&first={ContinuationToken}";
            return url;
        }

        private async Task<IEnumerable<TSchema>> GetDataFromProvider<TSchema>(IParser<TSchema> parser, string url, int pageSize) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(url)
            };

            HttpRequestResult requestResult = await HttpRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                var items = await parser.ParseAsync(requestResult.Result);
                ContinuationToken = GetContinuationToken(ContinuationToken, pageSize);
                _hasMoreItems = items.Any();
                return items;
            }

            throw new RequestFailedException(requestResult.StatusCode, requestResult.Result);
        }
    }
}
