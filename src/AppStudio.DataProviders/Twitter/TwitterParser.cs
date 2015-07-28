using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterSearchParser : IParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            var result = JsonConvert.DeserializeObject<TwitterSearchResult>(data);

            return result.statuses.Select(r => r.Parse()).ToList();
        }
    }

    internal static class TwitterParser
    {
        public static TwitterSchema Parse(this TwitterTimelineItem item)
        {
            TwitterSchema twit = new TwitterSchema
            {
                _id = item.Id,
                Text = item.Text.DecodeHtml(),
                CreationDateTime = TryParse(item.CreatedAt)
            };
            
            if (item.User == null)
            {
                twit.UserId = string.Empty;
                twit.UserName = string.Empty;
                twit.UserScreenName = string.Empty;
                twit.UserProfileImageUrl = string.Empty;
                twit.Url = string.Empty;
            }
            else
            {
                twit.UserId = item.User.Id;
                twit.UserName = item.User.Name.DecodeHtml();
                twit.UserScreenName = string.Concat("@", item.User.ScreenName.DecodeHtml());
                twit.UserProfileImageUrl = item.User.ProfileImageUrl;
                twit.Url = string.Format("https://twitter.com/{0}/status/{1}", item.User.ScreenName, item.Id);
                if (!string.IsNullOrEmpty(twit.UserProfileImageUrl))
                {
                    twit.UserProfileImageUrl = twit.UserProfileImageUrl.Replace("_normal", string.Empty);
                }
            }

            return twit;
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
