using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Html;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class HtmlParserTestLibrary
    {
        private static HtmlParser Parser = new HtmlParser();

        [TestMethod]
        public async Task LoadBigBlogPost3()
        {
            string htmlContent = await Common.ReadAssetFile("/Assets/Html/BigBlogPost3.htm");

            var data = Parser.Parse(htmlContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual("html", data.First()._id);
            Assert.IsNotNull(data.First().Content);
            Assert.AreEqual(167947, data.First().Content.Length);
        }

        [TestMethod]
        public async Task LoadBigLoremIpsum()
        {
            string htmlContent = await Common.ReadAssetFile("/Assets/Html/BigLoremIpsum.htm");

            var data = Parser.Parse(htmlContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual("html", data.First()._id);
            Assert.IsNotNull(data.First().Content);
            Assert.AreEqual(69772, data.First().Content.Length);
        }

        [TestMethod]
        public async Task LoadRussian()
        {
            string htmlContent = await Common.ReadAssetFile("/Assets/Html/Russian.htm");

            var data = Parser.Parse(htmlContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual("html", data.First()._id);
            Assert.IsNotNull(data.First().Content);
            Assert.AreEqual(3003, data.First().Content.Length);
        }

        [TestMethod]
        public async Task LoadLegendsOfManchesterUnitedDataSource()
        {
            string htmlContent = await Common.ReadAssetFile("/Assets/Html/LegendsOfManchesterUnitedDataSource.htm");

            var data = Parser.Parse(htmlContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual("html", data.First()._id);
            Assert.IsNotNull(data.First().Content);
            Assert.AreEqual(6255, data.First().Content.Length);
        }
    }
}
