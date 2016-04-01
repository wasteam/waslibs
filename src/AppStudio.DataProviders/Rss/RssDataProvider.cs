using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace AppStudio.DataProviders.Rss
{

    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        public override bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RssDataConfig config, int maxRecords, IPaginationParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = config.Url
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {               
                return parser.Parse(result.Result).GetData();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private IEnumerable<RssSchema> InternalParse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var doc = XDocument.Parse(data);
            var type = BaseRssParser.GetFeedType(doc);

            BaseRssParser rssParser;
            if (type == RssType.Rss)
            {
                rssParser = new Rss2Parser();
            }
            else
            {
                rssParser = new AtomParser();
            }

            return rssParser.LoadFeed(doc);
        }

        protected override IPaginationParser<RssSchema> GetDefaultParserInternal(RssDataConfig config)
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
    }


    #region Poc

    //public class RssDataProvider : IncrementalDataProviderBase<RssDataConfig, RssSchema, RssSchema>
    //{

    //    protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RssDataConfig config, int maxRecords, IParserIncremental<RssSchema, TSchema> parser)
    //    {
    //        var settings = new HttpRequestSettings()
    //        {
    //            RequestedUri = config.Url
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

    //    //public override T InternalParse<T>(string data)
    //    //{
    //    //    if (string.IsNullOrEmpty(data))
    //    //    {
    //    //        return default(T);
    //    //    }

    //    //    var doc = XDocument.Parse(data);
    //    //    var type = BaseRssParser.GetFeedType(doc);

    //    //    BaseRssParser rssParser;
    //    //    if (type == RssType.Rss)
    //    //    {
    //    //        rssParser = new Rss2Parser();
    //    //    }
    //    //    else
    //    //    {
    //    //        rssParser = new AtomParser();
    //    //    }

    //    //    T response = rssParser.LoadFeed<T>(doc);
    //    //    return response;
    //    //}

    //    protected override IParserIncremental<RssSchema, RssSchema> GetDefaultParserInternal(RssDataConfig config)
    //    {
    //        return new RssParser();
    //    }

    //    protected override void ValidateConfig(RssDataConfig config)
    //    {
    //        if (config.Url == null)
    //        {
    //            throw new ConfigParameterNullException("Url");
    //        }
    //    }
    //}

    #endregion
}
