using System;
using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;

namespace AppStudio.DataProviders.Flickr
{
    public class FlickrParser : IParserIncremental<RssSchema, FlickrSchema>, IPaginationParser<FlickrSchema>
    {
        public IResponseBase<FlickrSchema> Parse(string data)
        {
            var result = new GenericResponse<FlickrSchema>();
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> flickrItems = rssParser.Parse(data).GetData();
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

        public FlickrSchema Parse(RssSchema data)
        {
            if (data == null)
            {
                return null;
            }
            return new FlickrSchema()
            {
                _id = data._id,
                Title = data.Title,
                Summary = data.Summary,
                // Change medium images to large
                ImageUrl = data.ImageUrl.Replace("_m.jpg", "_b.jpg"),
                Published = data.PublishDate,
                FeedUrl = data.FeedUrl
            };
        }
    }
}
