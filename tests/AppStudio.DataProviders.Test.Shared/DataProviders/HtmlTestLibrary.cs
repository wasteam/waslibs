using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Html;
using AppStudio.DataProviders.LocalStorage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class HtmlTestLibrary
    {
        [TestMethod]
        public async Task LoadHtml()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalHtml.htm"
            };

            var dataProvider = new HtmlDataProvider();
            IEnumerable<HtmlSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Count(), 1);
        }

        [TestMethod]
        public async Task TestNullUrlConfig()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = null
            };

            var dataProvider = new LocalStorageDataProvider<HtmlSchema>();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("FilePath"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new LocalStorageDataProvider<HtmlSchema>();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new LocalStorageDataProvider<HtmlSchema>();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync(new LocalStorageDataConfig(), null));
        }
    }
}
