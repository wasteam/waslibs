using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;
using System.Linq;


namespace AppStudio.DataProviders.Facebook
{
    //public class FacebookParser : IParserIncremental<GraphData, FacebookSchema>
    //{
    //    public FacebookSchema Parse(GraphData data)
    //    {
    //        if (data == null)
    //        {
    //            return null;
    //        }

    //        return new FacebookSchema
    //        {
    //            _id = data.id,
    //            Author = data.from.name,
    //            PublishDate = data.created_time,
    //            Title = data.message.DecodeHtml(),
    //            Summary = data.message.DecodeHtml(),
    //            Content = data.message,
    //            ImageUrl = ConvertImageUrlFromParameter(data.full_picture),
    //            FeedUrl = BuildFeedUrl(data.from.id, data.id, data.link)
    //        };
    //    }

    //    //public IEnumerable<FacebookSchema> Parse(GraphData[] data)
    //    //{

    //    //    if (data == null)
    //    //    {
    //    //        return null;
    //    //    }
    //    //    Collection<FacebookSchema> resultToReturn = new Collection<FacebookSchema>();
    //    //    foreach (var i in data)
    //    //    {
    //    //        resultToReturn.Add(new FacebookSchema
    //    //        {
    //    //            _id = i.id,
    //    //            Author = i.from.name,
    //    //            PublishDate = i.created_time,
    //    //            Title = i.message.DecodeHtml(),
    //    //            Summary = i.message.DecodeHtml(),
    //    //            Content = i.message,
    //    //            ImageUrl = ConvertImageUrlFromParameter(i.full_picture),
    //    //            FeedUrl = BuildFeedUrl(i.from.id, i.id, i.link)
    //    //        });
    //    //    }

    //    //    return resultToReturn;
    //    //}

    //    private static string ConvertImageUrlFromParameter(string imageUrl)
    //    {
    //        string parsedImageUrl = null;
    //        if (!string.IsNullOrEmpty(imageUrl) && imageUrl.IndexOf("url=") > 0)
    //        {
    //            Uri imageUri = new Uri(imageUrl);
    //            var imageUriQuery = imageUri.Query.Replace("?", string.Empty).Replace("&amp;", "&");

    //            var imageUriQueryParameters = imageUriQuery.Split('&').Select(q => q.Split('='))
    //                    .Where(s => s != null && s.Length >= 2)
    //                    .ToDictionary(k => k[0], v => v[1]);

    //            string url;
    //            if (imageUriQueryParameters.TryGetValue("url", out url) && !string.IsNullOrEmpty(url))
    //            {
    //                parsedImageUrl = WebUtility.UrlDecode(url);
    //            }
    //        }
    //        else if (!string.IsNullOrEmpty(imageUrl))
    //        {
    //            parsedImageUrl = WebUtility.HtmlDecode(imageUrl);
    //        }

    //        return parsedImageUrl;
    //    }

    //    private static string BuildFeedUrl(string authorId, string id, string link)
    //    {
    //        if (!string.IsNullOrEmpty(link))
    //        {
    //            return link;
    //        }

    //        const string baseUrl = "https://www.facebook.com";
    //        var Ids = id.Split('_');
    //        if (Ids.Length > 1)
    //        {
    //            var postId = id.Split('_')[1];
    //            return $"{baseUrl}/{authorId}/posts/{postId}";
    //        }

    //        return $"{baseUrl}/{authorId}";
    //    }


    //}

    public class FacebookParser : IPaginationParser<FacebookSchema>
    {
        public IResponseBase<FacebookSchema> Parse(string data)
        {
            var result = new FacebookResponse<FacebookSchema>();
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            Collection<FacebookSchema> resultToReturn = new Collection<FacebookSchema>();
            var searchList = JsonConvert.DeserializeObject<FacebookGraphResponse>(data);
            foreach (var i in searchList.data.Where(w => !string.IsNullOrEmpty(w.message)).OrderByDescending(o => o.created_time))
            {
                var item = new FacebookSchema
                {
                    _id = i.id,
                    Author = i.from.name,
                    PublishDate = i.created_time,
                    Title = i.message.DecodeHtml(),
                    Summary = i.message.DecodeHtml(),
                    Content = i.message,
                    ImageUrl = ConvertImageUrlFromParameter(i.full_picture),
                    FeedUrl = BuildFeedUrl(i.from.id, i.id, i.link)
                };
                resultToReturn.Add(item);
            }

            result.data = resultToReturn.ToArray();
            result.NextPageToken = searchList?.paging?.next;

            return result;
        }


        private static string ConvertImageUrlFromParameter(string imageUrl)
        {
            string parsedImageUrl = null;
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.IndexOf("url=") > 0)
            {
                Uri imageUri = new Uri(imageUrl);
                var imageUriQuery = imageUri.Query.Replace("?", string.Empty).Replace("&amp;", "&");

                var imageUriQueryParameters = imageUriQuery.Split('&').Select(q => q.Split('='))
                        .Where(s => s != null && s.Length >= 2)
                        .ToDictionary(k => k[0], v => v[1]);

                string url;
                if (imageUriQueryParameters.TryGetValue("url", out url) && !string.IsNullOrEmpty(url))
                {
                    parsedImageUrl = WebUtility.UrlDecode(url);
                }
            }
            else if (!string.IsNullOrEmpty(imageUrl))
            {
                parsedImageUrl = WebUtility.HtmlDecode(imageUrl);
            }

            return parsedImageUrl;
        }

        private static string BuildFeedUrl(string authorId, string id, string link)
        {
            if (!string.IsNullOrEmpty(link))
            {
                return link;
            }

            const string baseUrl = "https://www.facebook.com";
            var Ids = id.Split('_');
            if (Ids.Length > 1)
            {
                var postId = id.Split('_')[1];
                return $"{baseUrl}/{authorId}/posts/{postId}";
            }

            return $"{baseUrl}/{authorId}";
        }


    }


    public class FacebookResponse<T> : IResponseBase<T>
    {
        public T[] data { get; set; }

        public string NextPageToken { get; set; }

        public IEnumerable<T> GetData()
        {
            return data;
        }
    }

    public class FacebookGraphResponse : IResponseBase<GraphData>
    {
        public GraphData[] data { get; set; }

        public Paging paging { get; set; }





        public string NextPageToken
        {
            get
            {
                throw new NotImplementedException();
            }
        }

       

        public IEnumerable<GraphData> GetData()
        {
            return data;
        }
    }

    public class From
    {
        public string category { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class GraphData
    {
        public string id { get; set; }
        public From from { get; set; }
        public string type { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string message { get; set; }
        public string full_picture { get; set; }
        public string link { get; set; }
    }

    public class Paging
    {
        public string previous { get; set; }
        public string next { get; set; }
    }
}
