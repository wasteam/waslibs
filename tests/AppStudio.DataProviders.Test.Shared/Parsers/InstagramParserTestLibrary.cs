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
            Assert.AreEqual("1121786263690425541_1709772956", item._id);
            Assert.AreEqual("a.mid.e", item.Author);
            Assert.AreEqual(GetInstagramDateTime("1447947365"), item.Published);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xat1/t51.2885-15/s640x640/sh0.08/e35/12276732_1656184051306956_1776538106_n.jpg", item.ImageUrl);
            Assert.AreEqual("https://instagram.com/p/-RYqnRLjTF/", item.SourceUrl);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xat1/t51.2885-15/s320x320/e35/12276732_1656184051306956_1776538106_n.jpg", item.ThumbnailUrl);
            Assert.AreEqual("#sunset #unedited #noedit #lumiacamera #dubai #lowlightphotography #globalvillage #mydubai #lumiaphotography #microsoftlumia #lumia640 #nban #windowsphone #thelumians #globalvillageuae", item.Title);
        }

        [TestMethod]
        public async Task LoadInstagramTag()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Instagram/InstagramTagQuery.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.Count());
            var item = data.First();
            Assert.AreEqual("1038988244373352542_2112437955", item._id);
            Assert.AreEqual("iind26", item.Author);
            Assert.AreEqual(GetInstagramDateTime("1438077072"), item.Published);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xft1/t51.2885-15/e15/11264664_537348226422324_77284209_n.jpg", item.ImageUrl);
            Assert.AreEqual("https://instagram.com/p/5rOk2vrjBe/", item.SourceUrl);
            Assert.AreEqual("https://scontent.cdninstagram.com/hphotos-xft1/t51.2885-15/s320x320/e15/11264664_537348226422324_77284209_n.jpg", item.ThumbnailUrl);
            Assert.AreEqual("Tidak jauh dari pohon kelapa....#PohonKelapa #CoconutTree #MatahariTerbenam #Sunset #Nokia #NokiaLumia #nokiaLover #lumia #Lumia800 #Lumialover #WindowsPhone #grateful #Alhamdulillah #LatePost", item.Title);
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
