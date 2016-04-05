using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AppStudio.DataProviders.Flickr;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class FlickrParserTestLibrary
    {
        private static FlickrParser Parser = new FlickrParser();

        [TestMethod]
        public async Task LoadFlickrId()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Flickr/FlickrIdQuery.xml");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.GetItems().Count());
            var item = data.GetItems().First();
            Assert.AreEqual("tag:flickr.com,2005:/photo/19458239284", item._id);
            Assert.AreEqual("http://farm4.staticflickr.com/3793/19458239284_bdf3d72136_b.jpg", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-07-28T09:35:10Z"), item.Published);
            Assert.AreEqual("SFZAFRIDIMOHAMMAD posted a photo:", item.Summary);
            Assert.AreEqual("flat,800x800,070,f (11)", item.Title);
        }

        [TestMethod]
        public async Task LoadFlickrTags()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Flickr/FlickrTagsQuery.xml");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(20, data.GetItems().Count());
            var item = data.GetItems().First();
            Assert.AreEqual("tag:flickr.com,2005:/photo/19458112614", item._id);
            Assert.AreEqual("http://farm1.staticflickr.com/520/19458112614_619869f6c4_b.jpg", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-07-28T09:28:31Z"), item.Published);
            Assert.AreEqual("erik.fabler posted a photo:                via Instagram ift.tt/1fCJLVn", item.Summary);
            Assert.AreEqual("#clouds", item.Title);
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
