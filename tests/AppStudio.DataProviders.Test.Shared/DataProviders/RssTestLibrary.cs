using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Rss;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public class RssTestLibrary
    {
        [TestMethod]
        public async Task LoadRss()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx")
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
        }

        [TestMethod]
        public async Task TestNullUrlConfig()
        {
            var config = new RssDataConfig
            {
                Url = null
            };

            var dataProvider = new RssDataProvider();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Url"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new RssDataProvider();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new RssDataProvider();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<RssSchema>(new RssDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestMaxRecords_Min()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx")
            };
            var maxRecords = 1;
            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, rssItems.Count());
        }

        [TestMethod]
        public async Task LoadPaginationRss()
        {

            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx")
            };

            var dataProvider = new RssDataProvider();
           await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<RssSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationRss()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx")
            };

            var dataProvider = new RssDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

    }
}
