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
    }
}
