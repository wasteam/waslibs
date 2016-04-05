using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AppStudio.DataProviders.Bing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class BingParserTestLibrary
    {
        private static BingParser Parser = new BingParser();

        [TestMethod]
        public async Task LoadBingParse1()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Bing/BingParse1.xml");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(10, data.GetItems().Count());
            var item = data.GetItems().First();
            Assert.AreEqual("http://appstudio.windows.com/", item._id);
            Assert.AreEqual("http://appstudio.windows.com/", item.Link);
            Assert.AreEqual(DateTime.Parse("Sun, 26 Jul 2015 17:25:00 GMT"), item.Published);
            Assert.AreEqual("And if you want to advanced programming features, Windows App Studio generates your source code ready for Visual Studio - a feature no other app-builder tool provides.", item.Summary);
            Assert.AreEqual("Windows App Studio - Official Site", item.Title);
        }

        [TestMethod]
        public async Task LoadBingParse2()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Bing/BingParse2.xml");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(10, data.GetItems().Count());
            var item = data.GetItems().First();
            Assert.AreEqual("https://www.mixcloud.com/tag/dj-nano/", item._id);
            Assert.AreEqual("https://www.mixcloud.com/tag/dj-nano/", item.Link);
            Assert.AreEqual(DateTime.Parse("Sun, 21 Jun 2015 15:43:00 GMT"), item.Published);
            Assert.AreEqual("Listen to free Dj Nano radio shows, DJ mix sets and Podcasts on Mixcloud", item.Summary);
            Assert.AreEqual("Dj Nano shows and Podcasts | Mixcloud", item.Title);
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
            ExceptionsAssert.Throws<XmlException>(() => Parser.Parse("<xml>"));
        }
    }
}
