using System;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.YouTube;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class YouTubeParserTestLibrary
    {
        private static YouTubePlaylistParser PlaylistParser = new YouTubePlaylistParser();
        private static YouTubeSearchParser SearchParser = new YouTubeSearchParser();

        [TestMethod]
        public async Task LoadYouTubeChannels()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/YouTube/YouTubeChannelsQuery.json");

            var data = PlaylistParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();
            Assert.AreEqual("UUWd-MOd82aDxHg88hCsIMhIK-_7fAVHgN", item._id);
            Assert.AreEqual("http://www.youtube.com/embed/xaChxmLssbI?rel=0&fs=0", item.EmbedHtmlFragment);
            Assert.AreEqual("http://www.youtube.com/watch?v=xaChxmLssbI", item.ExternalUrl);
            Assert.AreEqual("https://i.ytimg.com/vi/xaChxmLssbI/hqdefault.jpg", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-07-01T16:04:10.000Z").ToUniversalTime(), item.Published);
            Assert.AreEqual("See how National Geographic photographer Stephen Alvarez uses Lumia, Surface Pro 3, Band, OneDrive, OneNote, and Bing to plan, prepare, and power his latest assignment shooting Mexico’s Paricutin volcano — one of the Seven Natural Wonders of the World.\n\nRead more: http://lumiaconversations.microsoft.com/?p=186586", item.Summary);
            Assert.AreEqual("Lumia x Nat Geo and the Mexican Ring of Fire: Achieving more with Microsoft", item.Title);
            Assert.AreEqual("xaChxmLssbI", item.VideoId);
            Assert.IsNull(item.VideoUrl);
        }

        [TestMethod]
        public async Task LoadYouTubePlaylist()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/YouTube/YouTubePlaylistQuery.json");

            var data = PlaylistParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();
            Assert.AreEqual("PLc4tAG4a7kJ-jSpWEsSc7Lin8UqUqx9UB", item._id);
            Assert.AreEqual("http://www.youtube.com/embed/QenqTgnRkrA?rel=0&fs=0", item.EmbedHtmlFragment);
            Assert.AreEqual("http://www.youtube.com/watch?v=QenqTgnRkrA", item.ExternalUrl);
            Assert.AreEqual("https://i.ytimg.com/vi/QenqTgnRkrA/hqdefault.jpg", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2009-10-31T18:40:37.000Z").ToUniversalTime(), item.Published);
            Assert.AreEqual("Sylver - forever in love music video", item.Summary);
            Assert.AreEqual("Sylver - Forever In Love", item.Title);
            Assert.AreEqual("QenqTgnRkrA", item.VideoId);
            Assert.IsNull(item.VideoUrl);
        }

        [TestMethod]
        public async Task LoadYouTubeVideos()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/YouTube/YouTubeVideosQuery.json");

            var data = SearchParser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();
            Assert.AreEqual("hIJzjTUql6w", item._id);
            Assert.AreEqual("http://www.youtube.com/embed/hIJzjTUql6w?rel=0&fs=0", item.EmbedHtmlFragment);
            Assert.AreEqual("http://www.youtube.com/watch?v=hIJzjTUql6w", item.ExternalUrl);
            Assert.AreEqual("https://i.ytimg.com/vi/hIJzjTUql6w/hqdefault.jpg", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-01-26T16:18:36.000Z").ToUniversalTime(), item.Published);
            Assert.AreEqual("Amazing passes & beautiful combinations from Real Madrid players 2014-2015 https://www.facebook.com/mrbundesteam - facebook page Music: Hopium ...", item.Summary);
            Assert.AreEqual("Real Madrid CF ● Amazing Team Play 2014/2015 ● Best Passes & Combinations ● HD", item.Title);
            Assert.AreEqual("hIJzjTUql6w", item.VideoId);
            Assert.IsNull(item.VideoUrl);
        }

        [TestMethod]
        public void LoadPlaylistNullQuery()
        {
            var data = PlaylistParser.Parse(null);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadSearchNullQuery()
        {
            var data = SearchParser.Parse(null);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadPlaylistEmptyQuery()
        {
            var data = PlaylistParser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadSearchEmptyQuery()
        {
            var data = SearchParser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadPlaylistInvalidJson()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => PlaylistParser.Parse("["));
        }

        [TestMethod]
        public void LoadSearchInvalidJson()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => SearchParser.Parse("["));
        }
    }
}
