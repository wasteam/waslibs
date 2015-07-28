using System;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.TouchDevelop
{
    public class TouchDevelopSchema : SchemaBase
    {
        public DateTime Time { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string IconBackground { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public bool UserHasPicture { get; set; }

        public int UserScore { get; set; }

        public string ScreenshotThumbUrl { get; set; }

        public int CumulativePositiveReviews { get; set; }

        [JsonIgnore]
        public string UserPictureUrl
        {
            get { return this.UserHasPicture ? string.Format("http://www.touchdevelop.com/api/{0}/picture?type=large", this.UserId) : string.Empty; }
        }

        [JsonIgnore]
        public string ScreenshotUrl
        {
            get { return string.IsNullOrEmpty(ScreenshotThumbUrl) ? string.Empty : ScreenshotThumbUrl.Replace("/thumb/", "/pub/"); }
        }

        [JsonIgnore]
        public bool HasScreenshot
        {
            get { return !string.IsNullOrEmpty(this.ScreenshotThumbUrl); }
        }
    }
}