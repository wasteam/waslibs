using System;
using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;

namespace AppStudio.DataProviders.Bing
{
    public class BingParser : IParser<BingSchema>, IPaginationParser<BingSchema>
    {
        IEnumerable<BingSchema> IParser<BingSchema>.Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> syndicationItems = rssParser.Parse(data);
            return (from r in syndicationItems
                    select new BingSchema()
                    {
                        _id = r._id,
                        Title = r.Title,
                        Summary = r.Summary,
                        Link = r.FeedUrl,
                        Published = r.PublishDate
                    });
        }

        public IResponseBase<BingSchema> Parse(string data)
        {
            var result = new GenericResponse<BingSchema>();
            if (string.IsNullOrEmpty(data))
            {
                return result;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> syndicationItems = rssParser.Parse(data);

            var items = (from r in syndicationItems
                         select new BingSchema()
                         {
                             _id = r._id,
                             Title = r.Title,
                             Summary = r.Summary,
                             Link = r.FeedUrl,
                             Published = r.PublishDate
                         });

            foreach (var item in items)
            {
                result.Add(item);
            }

            return result;

        }
    }
}
