using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Flickr;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class FlickrTestLibrary
    {
        [TestMethod]
        public async Task LoadFlickrUser()
        {
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
            Assert.IsTrue(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task LoadFlickrTags()
        {
            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
            Assert.IsTrue(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task TestNullQueryConfig()
        {
            var config = new FlickrDataConfig
            {
                Query = null,
                QueryType = FlickrQueryType.Tags
            };

            var dataProvider = new FlickrDataProvider();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new FlickrDataProvider();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new FlickrDataProvider();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<FlickrSchema>(new FlickrDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestMaxRecordsUser_Min()
        {
            int maxRecords = 1;
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsTags_Min()
        {
            int maxRecords = 1;
            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsUser()
        {
            int maxRecords = 20;
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsTags()
        {
            int maxRecords = 20;
            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task LoadPaginationFlickrTags()
        {

            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags
            };

            var dataProvider = new FlickrDataProvider();
            await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<FlickrSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationFlickrTags()
        {
            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags
            };

            var dataProvider = new FlickrDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task LoadPaginationFlickrUser()
        {

            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id
            };

            var dataProvider = new FlickrDataProvider();
            await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<FlickrSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationFlickrUser()
        {
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id
            };

            var dataProvider = new FlickrDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task LoadFlickrUser_OrderByTitle()
        {
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id,
                OrderBy = nameof(FlickrSchema.Title),
                SortDirection = SortDirection.Ascending
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadDataAsync: Flickr sorting (ascending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");

            config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id,
                OrderBy = nameof(FlickrSchema.Title),
                SortDirection = SortDirection.Descending
            };

            dataProvider = new FlickrDataProvider();
             data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadDataAsync: Flickr sorting (descending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
        }

        [TestMethod]
        public async Task LoadFlickrUser_OrderByPublished()
        {
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id,
                OrderBy = nameof(FlickrSchema.Published),
                SortDirection = SortDirection.Ascending
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadDataAsync: Flickr sorting (ascending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");

            config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id,
                OrderBy = nameof(FlickrSchema.Published),
                SortDirection = SortDirection.Descending
            };

            dataProvider = new FlickrDataProvider();
            data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadDataAsync: Flickr sorting (descending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
        }

        [TestMethod]
        public async Task LoadFlickrTags_OrderByTitle()
        {
            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags,
                OrderBy = nameof(FlickrSchema.Title),
                SortDirection = SortDirection.Ascending
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadDataAsync: Flickr sorting (ascending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");

            config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags,
                OrderBy = nameof(FlickrSchema.Title),
                SortDirection = SortDirection.Descending
            };

            dataProvider = new FlickrDataProvider();
            data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadDataAsync: Flickr sorting (descending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).FirstOrDefault(), data.ToList()[0].Title, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Title).Select(x => x.Title).LastOrDefault(), data.ToList()[data.Count() - 1].Title, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
        }

        [TestMethod]
        public async Task LoadFlickrTags_OrderByPublished()
        {
            var config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags,
                OrderBy = nameof(FlickrSchema.Published),
                SortDirection = SortDirection.Ascending
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadDataAsync: Flickr sorting (ascending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");
            Assert.AreEqual(data.OrderBy(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadMoreDataAsync: Flickr sorting (ascending) is not working");

            config = new FlickrDataConfig
            {
                Query = "windowsappstudio",
                QueryType = FlickrQueryType.Tags,
                OrderBy = nameof(FlickrSchema.Published),
                SortDirection = SortDirection.Descending
            };

            dataProvider = new FlickrDataProvider();
            data = await dataProvider.LoadDataAsync(config, 5);

            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadDataAsync: Flickr sorting (descending) is not working");

            data = await dataProvider.LoadMoreDataAsync();
            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).FirstOrDefault(), data.ToList()[0].Published, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
            Assert.AreEqual(data.OrderByDescending(x => x.Published).Select(x => x.Published).LastOrDefault(), data.ToList()[data.Count() - 1].Published, "LoadMoreDataAsync: Flickr sorting (descending) is not working");
        }

        [TestMethod]
        public async Task LoadFlickr_OrderBy_InvalidProperty()
        {
            var config = new FlickrDataConfig
            {
                Query = "100292344@N05",
                QueryType = FlickrQueryType.Id,
                OrderBy = "InvalidProperty"
            };

            var dataProvider = new FlickrDataProvider();
            IEnumerable<FlickrSchema> data = await dataProvider.LoadDataAsync(config, 5);
            data = await dataProvider.LoadMoreDataAsync();         
        }
    }
}
