using System;
using System.Collections.Generic;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.TouchDevelop
{
    public class TouchDevelopParser : IParser<TouchDevelopSchema>
    {
        public IEnumerable<TouchDevelopSchema> Parse(string data)
        {
            var item = JsonConvert.DeserializeObject<TouchDevelopItem>(data);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            yield return new TouchDevelopSchema
            {
                _id = item.id,
                Time = epoch.AddSeconds(item.time),
                Url = item.url,
                Name = item.name.DecodeHtml(),
                Description = item.description.DecodeHtml(),
                IconUrl = item.iconurl,
                IconBackground = item.iconbackground,
                UserId = item.userid,
                UserName = item.username.DecodeHtml(),
                UserHasPicture = item.userhaspicture,
                UserScore = item.userscore,
                ScreenshotThumbUrl = item.screenshotthumburl,
                CumulativePositiveReviews = item.cumulativepositivereviews
            };
        }
    }

    internal class TouchDevelopSerialized
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<TouchDevelopSchema> Items { get; set; }
    }

    internal class TouchDevelopItem
    {
        public string kind { get; set; }
        public int time { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public int userscore { get; set; }
        public bool userhaspicture { get; set; }
        public string icon { get; set; }
        public string iconbackground { get; set; }
        public string iconurl { get; set; }
        public int positivereviews { get; set; }
        public int cumulativepositivereviews { get; set; }
        public int subscribers { get; set; }
        public int comments { get; set; }
        public int screenshots { get; set; }
        public object[] platforms { get; set; }
        public object[] capabilities { get; set; }
        public object[] flows { get; set; }
        public bool haserrors { get; set; }
        public string rootid { get; set; }
        public string updateid { get; set; }
        public int updatetime { get; set; }
        public bool ishidden { get; set; }
        public bool islibrary { get; set; }
        public string[] userplatform { get; set; }
        public int installations { get; set; }
        public int runs { get; set; }
        public object[] librarydependencyids { get; set; }
        public int art { get; set; }
        public string[] toptagids { get; set; }
        public string screenshotthumburl { get; set; }
        public string screenshoturl { get; set; }
        public object[] mergeids { get; set; }
    }
}
