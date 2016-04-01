using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Rss;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace AppStudio.DataProviders.Bing
{
    public class BingDataProvider : DataProviderBase<BingDataConfig, BingSchema>
    {
        private const string BaseUrl = "http://www.bing.com";

        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingDataConfig config, int maxRecords, IPaginationParser<TSchema> parser)
        {
            var countryValue = config.Country.GetStringValue();
            var locParameter = string.IsNullOrEmpty(countryValue) ? string.Empty : $"loc:{countryValue}+";
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri($"{BaseUrl}/search?q={locParameter}{ WebUtility.UrlEncode(config.Query)}&format=rss&count={maxRecords}")
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result)?.GetData();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }       

        protected override IPaginationParser<BingSchema> GetDefaultParserInternal(BingDataConfig config)
        {
            return new BingParser();
        }

        protected override void ValidateConfig(BingDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }
    }
    //public class BingDataProvider : IncrementalDataProviderBase<BingDataConfig, RssSchema, BingSchema>
    //{
    //    private const string BaseUrl = "http://www.bing.com";

    //    protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingDataConfig config, int maxRecords, IParserIncremental<RssSchema, TSchema> parser)
    //    {
    //        var countryValue = config.Country.GetStringValue();
    //        var locParameter = string.IsNullOrEmpty(countryValue) ? string.Empty : $"loc:{countryValue}+";
    //        var settings = new HttpRequestSettings()
    //        {
    //            RequestedUri = new Uri($"{BaseUrl}/search?q={locParameter}{ WebUtility.UrlEncode(config.Query)}&format=rss&count={maxRecords}")
    //        };

    //        HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
    //        if (result.Success)
    //        {
    //            var items = InternalParse(result.Result);
    //            Collection<TSchema> resultToReturn = new Collection<TSchema>();

    //            foreach (var item in items)
    //            {
    //                resultToReturn.Add(parser.Parse(item));
    //            }
    //            return resultToReturn;
    //        }

    //        throw new RequestFailedException(result.StatusCode, result.Result);
    //    }


    //    protected override IParserIncremental<RssSchema, BingSchema> GetDefaultParserInternal(BingDataConfig config)
    //    {
    //        return new BingParser();
    //    }

    //    private IEnumerable<RssSchema> InternalParse(string data)
    //    {
    //        if (string.IsNullOrEmpty(data))
    //        {
    //            return null;
    //        }

    //        var doc = XDocument.Parse(data);
    //        var type = BaseRssParser.GetFeedType(doc);

    //        BaseRssParser rssParser;
    //        if (type == RssType.Rss)
    //        {
    //            rssParser = new Rss2Parser();
    //        }
    //        else
    //        {
    //            rssParser = new AtomParser();
    //        }

    //        return rssParser.LoadFeed(doc);
    //    }

    //    protected override void ValidateConfig(BingDataConfig config)
    //    {
    //        if (config.Query == null)
    //        {
    //            throw new ConfigParameterNullException("Query");
    //        }
    //    }
    //}
}
