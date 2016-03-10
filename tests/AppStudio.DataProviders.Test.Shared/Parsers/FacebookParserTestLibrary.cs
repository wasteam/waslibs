using System;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Facebook;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class FacebookParserTestLibrary
    {
        private static FacebookParser Parser = new FacebookParser();

        [TestMethod]
        public async Task LoadFacebook1()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Facebook/FacebookQuery.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(24, data.Count());
            var item = data.First();
            Assert.AreEqual("8195378771_10153381796463772", item._id);
            Assert.AreEqual("Microsoft Lumia", item.Author);
            Assert.AreEqual("https://www.facebook.com/MicrosoftLumia/photos/a.107698418771.94817.8195378771/10153381796463772/?type=3", item.FeedUrl);
            Assert.AreEqual("https://scontent.xx.fbcdn.net/hphotos-xta1/v/t1.0-9/p720x720/12239745_10153381796463772_6633006179156403040_n.jpg?oh=ffda67cb1682805215074cfb323b90d7&oe=56B03F40", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-11-18T12:00:00+0000"), item.PublishDate);
            Assert.AreEqual("Lumia and our Lotus F1 Team #ambassador, Romain Grosjean, are focused on one goal, to be the best that we can be: http://lumia.ms/1j3kWUa", item.Content);
            Assert.AreEqual("Lumia and our Lotus F1 Team #ambassador, Romain Grosjean, are focused on one goal, to be the best that we can be: http://lumia.ms/1j3kWUa", item.Summary);
            Assert.AreEqual("Lumia and our Lotus F1 Team #ambassador, Romain Grosjean, are focused on one goal, to be the best that we can be: http://lumia.ms/1j3kWUa", item.Title);
        }

        [TestMethod]
        public async Task LoadFacebook2()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Facebook/FacebookQuery2.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(22, data.Count());
            var item = data.First();
            Assert.AreEqual("19034719952_10150941103834953", item._id);
            Assert.AreEqual("Real Madrid C.F.", item.Author);
            Assert.AreEqual("https://www.facebook.com/RealMadrid/videos/10150941103834953/", item.FeedUrl);
            Assert.AreEqual("https://scontent.xx.fbcdn.net/hvthumb-xpf1/v/t15.0-10/11919429_10150941173049953_871677567_n.jpg?oh=cacaee6eb5d5c9ed7f94bcfbf595677d&oe=56F7994B", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-11-19T15:06:16+0000"), item.PublishDate);
            Assert.AreEqual("We are Live from #RMCity\n\u26bd\ufe0f\ud83d\ude4c\nEstamos en Vivo desde #RMCity", item.Content);
            Assert.AreEqual("We are Live from #RMCity\n\u26bd\ufe0f\ud83d\ude4c\nEstamos en Vivo desde #RMCity", item.Summary);
            Assert.AreEqual("We are Live from #RMCity\n\u26bd\ufe0f\ud83d\ude4c\nEstamos en Vivo desde #RMCity", item.Title);
        }

        [TestMethod]
        public void LoadNullQuery()
        {
            var data = Parser.Parse(null);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadEmptyQuery()
        {
            var data = Parser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadInvalidXml()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => Parser.Parse("["));
        }
    }
}
