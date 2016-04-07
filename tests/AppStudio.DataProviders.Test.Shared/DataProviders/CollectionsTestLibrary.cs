using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.DynamicStorage;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.LocalStorage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class CollectionsTestLibrary
    {
        [TestMethod]
        public async Task LoadDynamicCollection()
        {
            var config = new DynamicStorageDataConfig
            {
                AppId = Guid.Empty.ToString(),
                StoreId = Guid.Empty.ToString(),
                DeviceType = "WINDOWS",
                Url = new Uri("http://appstudio-dev.cloudapp.net/api/data/collection?dataRowListId=6db1e7d0-5216-4519-8978-d51f1452f9f2&appId=7c181582-15d0-42f7-b3eb-ab5d2e7d2c8a")
            };

            var dataProvider = new DynamicStorageDataProvider<CollectionSchema>();
            IEnumerable<CollectionSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadStaticCollection()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalCollectionData.json"
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            IEnumerable<CollectionSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task TestMaxRecordsLocalStaticCollection_Min()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalCollectionData.json"
            };
            var maxRecords = 1;
            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            IEnumerable<CollectionSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsNotNull(data);
            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsLocalDynamicCollection_Min()
        {

            var config = new DynamicStorageDataConfig
            {
                AppId = Guid.Empty.ToString(),
                StoreId = Guid.Empty.ToString(),
                DeviceType = "WINDOWS",
                Url = new Uri("http://appstudio-dev.cloudapp.net/api/data/collection?dataRowListId=6db1e7d0-5216-4519-8978-d51f1452f9f2&appId=7c181582-15d0-42f7-b3eb-ab5d2e7d2c8a")
            };

            var maxRecords = 1;
            var dataProvider = new DynamicStorageDataProvider<CollectionSchema>();
            IEnumerable<CollectionSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsNotNull(data);          
            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestDynamicNullUrlConfig()
        {
            var config = new DynamicStorageDataConfig
            {
                Url = null
            };

            var dataProvider = new DynamicStorageDataProvider<CollectionSchema>();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Url"));
        }

        [TestMethod]
        public async Task TestLocalNullUrlConfig()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = null
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("FilePath"));
        }

        [TestMethod]
        public async Task TestDynamicNullConfig()
        {
            var dataProvider = new DynamicStorageDataProvider<CollectionSchema>();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestLocalNullConfig()
        {
            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestDynamicNullParser()
        {
            var dataProvider = new DynamicStorageDataProvider<CollectionSchema>();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<CollectionSchema>(new DynamicStorageDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestLocalNullParser()
        {
            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<CollectionSchema>(new LocalStorageDataConfig(), 20, null));
        }
    }

    // This is a Windows App Studio template schema from Generic Layout page.
    public class CollectionSchema : SchemaBase
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string PersonalSummary { get; set; }

        public string Image { get; set; }

        public string Other { get; set; }

        public string Phone { get; set; }

        public string Mail { get; set; }

        public string Thumbnail { get; set; }
    }
}
