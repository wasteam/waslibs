using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Rss;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataProvider :DataProviderBase<FlickrDataConfig,  FlickrSchema>
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FlickrDataConfig config, int maxRecords, IPaginationParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(string.Format("http://api.flickr.com/services/feeds/photos_public.gne?{0}={1}", config.QueryType.ToString().ToLower(), WebUtility.UrlEncode(config.Query)))
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result)?.GetItems();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }     

        protected override IPaginationParser<FlickrSchema> GetDefaultParserInternal(FlickrDataConfig config)
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
    }

}
    #region Poc
    //public class FlickrDataProvider : IncrementalDataProviderBase<FlickrDataConfig, RssSchema, FlickrSchema>
    //{
    //    protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FlickrDataConfig config, int maxRecords, IParserIncremental<RssSchema, TSchema> parser)
    //    {
    //        var settings = new HttpRequestSettings()
    //        {
    //            RequestedUri = new Uri(string.Format("http://api.flickr.com/services/feeds/photos_public.gne?{0}={1}", config.QueryType.ToString().ToLower(), WebUtility.UrlEncode(config.Query)))
    //        };

    //        HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
    //        if (result.Success)
    //        {
    //            var items = Parse(result.Result);
    //            Collection<TSchema> resultToReturn = new Collection<TSchema>();

    //            foreach (var item in items)
    //            {
    //                resultToReturn.Add(parser.Parse(item));
    //            }
    //            return resultToReturn;
    //        }

    //        throw new RequestFailedException(result.StatusCode, result.Result);
    //    }

    //    private IEnumerable<RssSchema> Parse(string data)
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

    //    protected override IParserIncremental<RssSchema, FlickrSchema> GetDefaultParserInternal(FlickrDataConfig config)
    //    {
    //        return new FlickrParser();
    //    }

    //    protected override void ValidateConfig(FlickrDataConfig config)
    //    {
    //        if (config.Query == null)
    //        {
    //            throw new ConfigParameterNullException("Query");
    //        }
    //    }
    //}}
    #endregion

