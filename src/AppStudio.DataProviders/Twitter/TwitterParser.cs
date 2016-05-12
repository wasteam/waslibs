using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Newtonsoft.Json;

using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterSearchParser : IParser<TwitterSchema>
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
    }

    internal static class TwitterParser
    {
        public static TwitterSchema Parse(this TwitterTimelineItem item)
        {
            TwitterSchema tweet = new TwitterSchema()
            {
                _id = item.Id
            };
            FillUserData(ref tweet, item);
            FillTweet(ref tweet, item);
            return tweet;
        }

        private static void FillUserData(ref TwitterSchema tweet, TwitterTimelineItem item)
        {
            TwitterUser user = null;
            if (item.RetweetedStatus != null)
            {
                user = item.RetweetedStatus.User;                
                tweet.UserName = $"{item.RetweetedStatus.User.Name.DecodeHtml()} (RT @{item.User.ScreenName.DecodeHtml()})";                                
            }
            else if (item.User != null)
            {
                user = item.User;                
                tweet.UserName = item.User.Name.DecodeHtml();                
            }

            tweet.UserId = user.Id;
            tweet.UserScreenName = string.Concat("@", user.ScreenName.DecodeHtml());
            tweet.UserProfileImageUrl = user.ProfileImageUrl;
            tweet.Url = string.Format("https://twitter.com/{0}/status/{1}", user.ScreenName, item.Id);
            if (!string.IsNullOrEmpty(tweet.UserProfileImageUrl))
            {
                tweet.UserProfileImageUrl = tweet.UserProfileImageUrl.Replace("_normal", string.Empty);
            }
        }

        private static void FillTweet(ref TwitterSchema tweet, TwitterTimelineItem item)
        {
            if (item.RetweetedStatus == null)
            {
                tweet.Text = item.Text.DecodeHtml();
            }
            else
            {
                tweet.Text = item.RetweetedStatus.Text.DecodeHtml();                
            }
            tweet.CreationDateTime = TryParse(item.CreatedAt);
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

    public class TwitterTimelineParser : IParser<TwitterSchema>
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

        [JsonProperty("retweeted_status")]
        public TwitterTimelineItem RetweetedStatus { get; set; }
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
