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
            Assert.IsTrue(dataProvider.IsInitialized);
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
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task LoadRss_RequestFailedException_Apache403()
        {
            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = null;
            var config = new RssDataConfig()
            {
                Url = new Uri("http://www.brunswickcps.org/apps/news/news_rss.jsp?id=0")
            };
            rssItems = await dataProvider.LoadDataAsync(config);
            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);

            config = new RssDataConfig()
            {
                Url = new Uri("http://www.vicoteka.mk/tvitoteka/feed/")
            };
            rssItems = await dataProvider.LoadDataAsync(config);
            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);

            config = new RssDataConfig()
            {
                Url = new Uri("http://www.vicoteka.mk/vicovi/vic-na-denot/feed")
            };
            rssItems = await dataProvider.LoadDataAsync(config);
            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
        }

        [TestMethod]
        public async Task LoadRss_GermanSpecialCharacters()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://www.inforadio.de/nachrichten/index.xml/feed=rss.xml")
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
            Assert.IsFalse(rssItems.Any(x => x.Content.Contains("Ã")));
        }


        [TestMethod]
        public async Task LoadRss_GreekSpecialCharacters()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("https://www.tuc.gr/754.html?&tx_mmforum_pi1%5Bfid%5D=27")
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
            Assert.IsFalse(rssItems.Any(x => x.Title.Contains("Î")));
        }

        [TestMethod]
        public async Task LoadRss_CatalanSpecialCharacters()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://www.hacesfalta.org/oportunidades/presencial/buscador/rss.aspx?c=oportunidadesPresenciales_hf&&idPais=60&enfamilia=0&engrupo=0")
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
            Assert.IsFalse(rssItems.Any(x => x.Content.Contains("Ã")));
        }

        [TestMethod]
        public async Task LoadRss_Encoding_ISO88591()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://feeds.folha.uol.com.br/colunas/jaimespitzcovsky/rss091.xml")
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
        }

        [TestMethod]
        public async Task LoadRss_OrderByTitle()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx"),
                OrderBy = nameof(RssSchema.Title),
                Direction = SortDirection.Ascending
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(rssItems.OrderBy(x => x.Title).Select(x => x.Title).FirstOrDefault(), rssItems.ToList()[0].Title);
            Assert.AreEqual(rssItems.OrderBy(x => x.Title).Select(x => x.Title).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].Title);

            rssItems = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(rssItems.OrderBy(x => x.Title).Select(x => x.Title).FirstOrDefault(), rssItems.ToList()[0].Title);
            Assert.AreEqual(rssItems.OrderBy(x => x.Title).Select(x => x.Title).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].Title);

            config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx"),
                OrderBy = nameof(RssSchema.Title),
                Direction = SortDirection.Descending
            };

            dataProvider = new RssDataProvider();
            rssItems = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(rssItems.OrderByDescending(x => x.Title).Select(x => x.Title).FirstOrDefault(), rssItems.ToList()[0].Title);
            Assert.AreEqual(rssItems.OrderByDescending(x => x.Title).Select(x => x.Title).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].Title);

            rssItems = await dataProvider.LoadMoreDataAsync();

            Assert.AreEqual(rssItems.OrderByDescending(x => x.Title).Select(x => x.Title).FirstOrDefault(), rssItems.ToList()[0].Title);
            Assert.AreEqual(rssItems.OrderByDescending(x => x.Title).Select(x => x.Title).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].Title);
        }

        [TestMethod]
        public async Task LoadRss_OrderByPublishDate()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx"),
                OrderBy = nameof(RssSchema.PublishDate),
                Direction = SortDirection.Ascending
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(rssItems.OrderBy(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault(), rssItems.ToList()[0].PublishDate);
            Assert.AreEqual(rssItems.OrderBy(x => x.PublishDate).Select(x => x.PublishDate).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].PublishDate);

            rssItems = await dataProvider.LoadMoreDataAsync();

            Assert.AreEqual(rssItems.OrderBy(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault(), rssItems.ToList()[0].PublishDate);
            Assert.AreEqual(rssItems.OrderBy(x => x.PublishDate).Select(x => x.PublishDate).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].PublishDate);

            config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx"),
                OrderBy = nameof(RssSchema.PublishDate),
                Direction = SortDirection.Descending
            };

            dataProvider = new RssDataProvider();
            rssItems = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(rssItems.OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault(), rssItems.ToList()[0].PublishDate);
            Assert.AreEqual(rssItems.OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].PublishDate);

            rssItems = await dataProvider.LoadMoreDataAsync();

            Assert.AreEqual(rssItems.OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault(), rssItems.ToList()[0].PublishDate);
            Assert.AreEqual(rssItems.OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).LastOrDefault(), rssItems.ToList()[rssItems.Count() - 1].PublishDate);
        }

        [TestMethod]
        public async Task LoadRss_OrderBy_InvalidProperty()
        {
            var config = new RssDataConfig()
            {
                Url = new Uri("http://blogs.msdn.com/b/windows_app_studio_news/rss.aspx"),
                OrderBy = "InvalidProperty"                
            };

            var dataProvider = new RssDataProvider();
            IEnumerable<RssSchema> rssItems = await dataProvider.LoadDataAsync(config, 5);

            rssItems = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(rssItems);
            Assert.AreNotEqual(rssItems.Count(), 0);
        }
    }
}
