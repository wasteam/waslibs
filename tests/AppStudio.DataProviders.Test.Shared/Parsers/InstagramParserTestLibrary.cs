using System;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Instagram;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class InstagramParserTestLibrary
    {
        private static InstagramParser Parser = new InstagramParser();

        [TestMethod]
        public async Task LoadInstagramId()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Instagram/InstagramIdQuery.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();
            Assert.AreEqual("1038264559833689680_256679330", item._id);
            Assert.AreEqual("microsoftlumia", item.Author);
            Assert.AreEqual(GetInstagramDateTime("1437990802"), item.Published);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xfa1/t51.2885-15/e15/11325689_473148866191760_1467431584_n.jpg", item.ImageUrl);
            Assert.AreEqual("https://instagram.com/p/5oqB2_w65Q/", item.SourceUrl);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xfa1/t51.2885-15/s150x150/e15/11325689_473148866191760_1467431584_n.jpg", item.ThumbnailUrl);
            Assert.AreEqual("regram @kelvin_jayd\nEarly morning shot with the lumia535 camera @ the lakeshore.#microsoftlumia #microsoftlumiauk #shotonmylumia #Lumia535", item.Title);
        }

        [TestMethod]
        public async Task LoadInstagramTag()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Instagram/InstagramTagQuery.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();
            Assert.AreEqual("1039007807387339973_1560042946", item._id);
            Assert.AreEqual("jeet7t", item.Author);
            Assert.AreEqual(GetInstagramDateTime("1438079404"), item.Published);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xfp1/t51.2885-15/s640x640/e35/sh0.08/11199427_836943496412921_1916259112_n.jpg", item.ImageUrl);
            Assert.AreEqual("https://instagram.com/p/5rTBiOLgzF/", item.SourceUrl);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xfp1/t51.2885-15/s150x150/e15/11199427_836943496412921_1916259112_n.jpg", item.ThumbnailUrl);
            Assert.AreEqual("#microsoft #535", item.Title);
        }

        [TestMethod]
        public void LoadNullQuery()
        {
            ExceptionsAssert.Throws<ArgumentNullException>(() => Parser.Parse(null));
        }

        [TestMethod]
        public void LoadEmptyQuery()
        {
            var data = Parser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadInvalidJson()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => Parser.Parse("["));
        }

        private DateTime GetInstagramDateTime(string date)
        {
            DateTime newDate = DateTime.Parse("1970,1,1");
            return newDate.AddSeconds(int.Parse(date));
        }
    }
}
