using System;
using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;

namespace AppStudio.DataProviders.Bing
{
    public class BingParser : IParserIncremental<RssSchema, BingSchema>, IPaginationParser<BingSchema>
    {
        public IResponseBase<BingSchema> Parse(string data)
        {
            var result = new GenericResponse<BingSchema>();
            if (string.IsNullOrEmpty(data))
            {
                return result;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> syndicationItems = rssParser.Parse(data).GetData();
                       
            var items =  (from r in syndicationItems
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

        public BingSchema Parse(RssSchema data)
        {
            if (data == null)
            {
                return null;
            }

            return new BingSchema()
            {
                _id = data._id,
                Title = data.Title,
                Summary = data.Summary,
                Link = data.FeedUrl,
                Published = data.PublishDate
            };
        }
        
    }
}
