using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Twitter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class TwitterParserTestLibrary
    {
        private static TwitterTimelineParser TimelineParser = new TwitterTimelineParser();
        private static TwitterSearchParser SearchParser = new TwitterSearchParser();

        [TestMethod]
        public async Task LoadTwitterHomeTimeline()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Twitter/TwitterHomeTimelineQuery.json");

            var data = TimelineParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(19, data.Count());
            var item = data.First();

            Assert.AreEqual("626316256417677312", item._id);
            Assert.AreEqual(DateTime.ParseExact("Wed Jul 29 09:00:06 +0000 2015", "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture), item.CreationDateTime);
            Assert.AreEqual("One of our favourite #sunset shots, thanks for sharing @sszemtelen! #Lumia930 http://t.co/m2KoIKkEhZ", item.Text);
            Assert.AreEqual("https://twitter.com/Lumia/status/626316256417677312", item.Url);
            Assert.AreEqual("16425197", item.UserId);
            Assert.AreEqual("Lumia", item.UserName);
            Assert.AreEqual("http://pbs.twimg.com/profile_images/600300130202300417/MxpeawCE.png", item.UserProfileImageUrl);
            Assert.AreEqual("@Lumia", item.UserScreenName);
        }

        [TestMethod]
        public async Task LoadTwitterUserTimeline()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Twitter/TwitterUserTimelineQuery.json");

            var data = TimelineParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();

            Assert.AreEqual("626316256417677312", item._id);
            Assert.AreEqual(DateTime.ParseExact("Wed Jul 29 09:00:06 +0000 2015", "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture), item.CreationDateTime);
            Assert.AreEqual("One of our favourite #sunset shots, thanks for sharing @sszemtelen! #Lumia930 http://t.co/m2KoIKkEhZ", item.Text);
            Assert.AreEqual("https://twitter.com/Lumia/status/626316256417677312", item.Url);
            Assert.AreEqual("16425197", item.UserId);
            Assert.AreEqual("Lumia", item.UserName);
            Assert.AreEqual("http://pbs.twimg.com/profile_images/600300130202300417/MxpeawCE.png", item.UserProfileImageUrl);
            Assert.AreEqual("@Lumia", item.UserScreenName);
        }

        [TestMethod]
        public async Task LoadRetweetedInTwitterUserTimeline()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Twitter/TwitterUserTimelineQuery.json");

            var data = TimelineParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.Where(t => t._id == "626294206563254273").FirstOrDefault();

            Assert.AreEqual("626294206563254273", item._id);
            Assert.AreEqual(DateTime.ParseExact("Wed Jul 29 07:32:29 +0000 2015", "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture), item.CreationDateTime);
            Assert.AreEqual("The best Windows ever is here. Learn how to upgrade to #Windows10 for free: http://t.co/KY2hJs1mmb http://t.co/COQTtZAu5P", item.Text);
            Assert.AreEqual("https://twitter.com/Windows/status/626294206563254273", item.Url);
            Assert.AreEqual("15670515", item.UserId);
            Assert.AreEqual("Windows (RT @Lumia)", item.UserName);
            Assert.AreEqual("http://pbs.twimg.com/profile_images/571398080688181248/57UKydQS.png", item.UserProfileImageUrl);
            Assert.AreEqual("@Windows", item.UserScreenName);
        }

        [TestMethod]
        public async Task LoadTwitterSearch()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Twitter/TwitterSearchQuery.json");

            var data = SearchParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(3, data.Count());
            var item = data.First();
            Assert.AreEqual("625011042121334785", item._id);
            Assert.AreEqual(DateTime.ParseExact("Sat Jul 25 18:33:39 +0000 2015", "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture), item.CreationDateTime);
            Assert.AreEqual("Check out my BlankApp http://t.co/CreaqyJGxl #WindowsAppStudio", item.Text);
            Assert.AreEqual("https://twitter.com/LewisMCYouTube/status/625011042121334785", item.Url);
            Assert.AreEqual("3133393408", item.UserId);
            Assert.AreEqual("Lewis Button", item.UserName);
            Assert.AreEqual("http://abs.twimg.com/sticky/default_profile_images/default_profile_5.png", item.UserProfileImageUrl);
            Assert.AreEqual("@LewisMCYouTube", item.UserScreenName);
        }

        [TestMethod]
        public void LoadTimelineNullQuery()
        {
            var data = TimelineParser.Parse(null);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadSearchNullQuery()
        {
            var data = SearchParser.Parse(null);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadTimelineEmptyQuery()
        {
            var data = TimelineParser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadSearchEmptyQuery()
        {
            var data = SearchParser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadTimelineInvalidJson()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => TimelineParser.Parse("["));
        }

        [TestMethod]
        public void LoadSearchInvalidJson()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => SearchParser.Parse("["));
        }
    }
}
