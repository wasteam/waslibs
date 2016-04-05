using System;
using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrParser : IParser<FlickrSchema>, IPaginationParser<FlickrSchema>
    {
        IEnumerable<FlickrSchema> IParser<FlickrSchema>.Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> flickrItems = rssParser.Parse(data);
            return (from r in flickrItems
                    select new FlickrSchema()
                    {
                        _id = r._id,
                        Title = r.Title,
                        Summary = r.Summary,
                        // Change medium images to large
                        ImageUrl = r.ImageUrl.Replace("_m.jpg", "_b.jpg"),
                        Published = r.PublishDate,
                        FeedUrl = r.FeedUrl
                    });
        }

        public IParserResponse<FlickrSchema> Parse(string data)
        {
            var result = new ParserResponseCollection<FlickrSchema>();
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> flickrItems = rssParser.Parse(data);
            var items = (from r in flickrItems
                         select new FlickrSchema()
                         {
                             _id = r._id,
                             Title = r.Title,
                             Summary = r.Summary,
                             // Change medium images to large
                             ImageUrl = r.ImageUrl.Replace("_m.jpg", "_b.jpg"),
                             Published = r.PublishDate,
                             FeedUrl = r.FeedUrl
                         });

            foreach (var item in items)
            {
                result.Add(item);
            }

            return result;
        }
    }
}
