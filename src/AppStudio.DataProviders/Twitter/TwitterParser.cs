using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterSearchParser : IParser<TwitterSchema>, IPaginationParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<TwitterSearchResult>(data);

            return result.statuses.Select(r => r.Parse()).ToList();
        }

        IParserResponse<TwitterSchema> IPaginationParser<TwitterSchema>.Parse(string data)
        {
            TwitterResultCollection result = new TwitterResultCollection();
            if (string.IsNullOrEmpty(data))
            {
                return result;
            }

            var jsonResult = JsonConvert.DeserializeObject<TwitterSearchResult>(data);
            if (jsonResult.statuses.Any())
            {
                result = new TwitterResultCollection(jsonResult.statuses.Select(r => r.Parse()).ToList());
            }
            return result;
        }
    }

    internal static class TwitterParser
    {
        public static TwitterSchema Parse(this TwitterTimelineItem item)
        {
            TwitterSchema tweet = new TwitterSchema
            {
                _id = item.Id,
                Text = item.Text.DecodeHtml(),
                CreationDateTime = TryParse(item.CreatedAt)
            };

            if (item.User == null)
            {
                tweet.UserId = string.Empty;
                tweet.UserName = string.Empty;
                tweet.UserScreenName = string.Empty;
                tweet.UserProfileImageUrl = string.Empty;
                tweet.Url = string.Empty;
            }
            else
            {
                tweet.UserId = item.User.Id;
                tweet.UserName = item.User.Name.DecodeHtml();
                tweet.UserScreenName = string.Concat("@", item.User.ScreenName.DecodeHtml());
                tweet.UserProfileImageUrl = item.User.ProfileImageUrl;
                tweet.Url = string.Format("https://twitter.com/{0}/status/{1}", item.User.ScreenName, item.Id);
                if (!string.IsNullOrEmpty(tweet.UserProfileImageUrl))
                {
                    tweet.UserProfileImageUrl = tweet.UserProfileImageUrl.Replace("_normal", string.Empty);
                }
            }

            return tweet;
        }

        private static DateTime TryParse(string dateTime)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Today;
            }

            return dt;
        }
    }

    public class TwitterTimelineParser : IParser<TwitterSchema>, IPaginationParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<TwitterTimelineItem[]>(data);

            return result.Select(r => r.Parse()).ToList();
        }

        IParserResponse<TwitterSchema> IPaginationParser<TwitterSchema>.Parse(string data)
        {
            TwitterResultCollection result = new TwitterResultCollection();
            if (string.IsNullOrEmpty(data))
            {
                return result;
            }

            var jsonResult = JsonConvert.DeserializeObject<TwitterTimelineItem[]>(data);
            if (jsonResult.Any())
            {
                result = new TwitterResultCollection(jsonResult.Select(r => r.Parse()).ToList());
            }
            return result;
        }


    }


    public class TwitterResultCollection : Collection<TwitterSchema>, IParserResponse<TwitterSchema>
    {
        public TwitterResultCollection()
        {

        }
        public TwitterResultCollection(IEnumerable<TwitterSchema> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    Items.Add(item);
                }

                ContinuationToken = GetMaxId(Items.LastOrDefault()._id);
            }
        }
        public string ContinuationToken { get; set; }

        private static string GetMaxId(string id_str)
        {
            long id;
            if (long.TryParse(id_str, out id))
            {
                var result = id - 1;
                return result.ToString();
            }
            return string.Empty;
        }

        public IEnumerable<TwitterSchema> GetItems()
        {
            return Items;
        }
    }

    public class TwitterTimelineItem
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("user")]
        public TwitterUser User { get; set; }
    }

    public class TwitterUser
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
    }

    internal class TwitterSearchResult
    {
        public TwitterTimelineItem[] statuses { get; set; }
    }
}
