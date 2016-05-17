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
            Assert.IsTrue(dataProvider.IsInitialized);
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
            Assert.IsTrue(dataProvider.IsInitialized);
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

        [TestMethod]
        public async Task LoadPaginationStaticCollection()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalCollectionData.json"
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<CollectionSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationStaticCollection()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalCollectionData.json"
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }


        [TestMethod]
        public async Task LoadPaginationDynamicCollection()
        {
            var config = new DynamicStorageDataConfig
            {
                AppId = Guid.Empty.ToString(),
                StoreId = Guid.Empty.ToString(),
                DeviceType = "WINDOWS",
                Url = new Uri("http://appstudio-dev.cloudapp.net/api/data/collection?dataRowListId=6db1e7d0-5216-4519-8978-d51f1452f9f2&appId=7c181582-15d0-42f7-b3eb-ab5d2e7d2c8a")
            };

            var dataProvider = new DynamicStorageDataProvider<CollectionSchema>();
            await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<CollectionSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }


        [TestMethod]
        public async Task LoadMoreDataInvalidOperationDynamicCollection()
        {
            var config = new DynamicStorageDataConfig
            {
                AppId = Guid.Empty.ToString(),
                StoreId = Guid.Empty.ToString(),
                DeviceType = "WINDOWS",
                Url = new Uri("http://appstudio-dev.cloudapp.net/api/data/collection?dataRowListId=6db1e7d0-5216-4519-8978-d51f1452f9f2&appId=7c181582-15d0-42f7-b3eb-ab5d2e7d2c8a")
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task LoadDynamicCollection_Sorting()
        {

            var config = new DynamicStorageDataConfig
            {
                AppId = Guid.Empty.ToString(),
                StoreId = Guid.Empty.ToString(),
                DeviceType = "WINDOWS",
                Url = new Uri("http://appstudio-dev.cloudapp.net/api/data/collection?dataRowListId=6389c5e8-788e-42cc-8b74-a16fca5e4bf3&appId=d3fdeca1-ee0e-482c-bc19-82e344d2b78c")
            };

            var dataProvider = new DynamicStorageDataProvider<CollectionSchema2>();
            IEnumerable<CollectionSchema2> data = await dataProvider.LoadDataAsync(config);

            config.OrderBy = "Title";
            config.SortDirection = SortDirection.Ascending;
            IEnumerable<CollectionSchema2> dataAsc = await dataProvider.LoadDataAsync(config);
            config.SortDirection = SortDirection.Descending;
            IEnumerable<CollectionSchema2> dataDesc = await dataProvider.LoadDataAsync(config);

            Assert.AreNotEqual(data.FirstOrDefault()?.Title, dataAsc.FirstOrDefault().Title);
            Assert.AreNotEqual(dataAsc.FirstOrDefault()?.Title, dataDesc.FirstOrDefault().Title);
            var dataExpected = data.OrderBy(x => x.Title).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataAsc.ToList()[i].Title);
            }

            dataExpected = data.OrderByDescending(x => x.Title).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataDesc.ToList()[i].Title);
            }

            config.OrderBy = "Date";
            config.SortDirection = SortDirection.Ascending;
            dataAsc = await dataProvider.LoadDataAsync(config);
            config.SortDirection = SortDirection.Descending;
            dataDesc = await dataProvider.LoadDataAsync(config);

            Assert.AreNotEqual(dataAsc.FirstOrDefault()?.Title, dataDesc.FirstOrDefault().Title);
            dataExpected = data.OrderBy(x => x.Date).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataAsc.ToList()[i].Title);
            }

            dataExpected = data.OrderByDescending(x => x.Date).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataDesc.ToList()[i].Title);
            }

            config.OrderBy = "DateTime";
            config.SortDirection = SortDirection.Ascending;
            dataAsc = await dataProvider.LoadDataAsync(config);
            config.SortDirection = SortDirection.Descending;
            dataDesc = await dataProvider.LoadDataAsync(config);

            Assert.AreNotEqual(dataAsc.FirstOrDefault()?.Title, dataDesc.FirstOrDefault().Title);

            dataExpected = data.OrderBy(x => x.DateTime).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataAsc.ToList()[i].Title);
            }

            dataExpected = data.OrderByDescending(x => x.DateTime).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataDesc.ToList()[i].Title);
            }
        }

        [TestMethod]
        public async Task LoadMoreDynamicCollection_Sorting()
        {

            var config = new DynamicStorageDataConfig
            {
                AppId = Guid.Empty.ToString(),
                StoreId = Guid.Empty.ToString(),
                DeviceType = "WINDOWS",
                Url = new Uri("http://appstudio-dev.cloudapp.net/api/data/collection?dataRowListId=6389c5e8-788e-42cc-8b74-a16fca5e4bf3&appId=d3fdeca1-ee0e-482c-bc19-82e344d2b78c")
            };

            var dataProvider = new DynamicStorageDataProvider<CollectionSchema2>();           

            config.OrderBy = "Title";
            config.SortDirection = SortDirection.Ascending;
            IEnumerable<CollectionSchema2> dataAsc = await dataProvider.LoadDataAsync(config, 2);
            dataAsc = await dataProvider.LoadMoreDataAsync();
            config.SortDirection = SortDirection.Descending;
            IEnumerable<CollectionSchema2> dataDesc = await dataProvider.LoadDataAsync(config, 2);
            dataDesc = await dataProvider.LoadMoreDataAsync();            

            var dataExpected = dataAsc.OrderBy(x => x.Title).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataAsc.ToList()[i].Title);
            }

            dataExpected = dataDesc.OrderByDescending(x => x.Title).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataDesc.ToList()[i].Title);
            }   

            config.OrderBy = "Date";
            config.SortDirection = SortDirection.Ascending;
            dataAsc = await dataProvider.LoadDataAsync(config, 2);
            dataAsc = await dataProvider.LoadMoreDataAsync();
            config.SortDirection = SortDirection.Descending;
            dataDesc = await dataProvider.LoadDataAsync(config, 2);
            dataDesc = await dataProvider.LoadMoreDataAsync();
         
            dataExpected = dataAsc.OrderBy(x => x.Date).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataAsc.ToList()[i].Title);
            }

            dataExpected = dataDesc.OrderByDescending(x => x.Date).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataDesc.ToList()[i].Title);
            }

            config.OrderBy = "DateTime";
            config.SortDirection = SortDirection.Ascending;
            dataAsc = await dataProvider.LoadDataAsync(config, 2);
            dataAsc = await dataProvider.LoadMoreDataAsync();
            config.SortDirection = SortDirection.Descending;
            dataDesc = await dataProvider.LoadDataAsync(config, 2);
            dataDesc = await dataProvider.LoadMoreDataAsync();                      

            dataExpected = dataAsc.OrderBy(x => x.DateTime).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataAsc.ToList()[i].Title);
            }

            dataExpected = dataDesc.OrderByDescending(x => x.DateTime).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Title, dataDesc.ToList()[i].Title);
            }
        }

        [TestMethod]
        public async Task LoadStaticCollection_Sorting()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalCollectionData.json"
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            IEnumerable<CollectionSchema> data = await dataProvider.LoadDataAsync(config);

            config.OrderBy = "Name";
            config.SortDirection = SortDirection.Ascending;
            IEnumerable<CollectionSchema> dataAsc = await dataProvider.LoadDataAsync(config);
            config.SortDirection = SortDirection.Descending;
            IEnumerable<CollectionSchema> dataDesc = await dataProvider.LoadDataAsync(config);

            Assert.AreNotEqual(data.FirstOrDefault()?.Name, dataAsc.FirstOrDefault().Name);
            Assert.AreNotEqual(dataAsc.FirstOrDefault()?.Name, dataDesc.FirstOrDefault().Name);
            var dataExpected = data.OrderBy(x => x.Name).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Name, dataAsc.ToList()[i].Name);
            }

            dataExpected = data.OrderByDescending(x => x.Name).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Name, dataDesc.ToList()[i].Name);
            }
        }

        [TestMethod]
        public async Task LoadMoreStaticCollection_Sorting()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalCollectionData.json"
            };

            var dataProvider = new LocalStorageDataProvider<CollectionSchema>();
            await dataProvider.LoadDataAsync(config, 2);
            IEnumerable<CollectionSchema> data = await dataProvider.LoadMoreDataAsync();

            config.OrderBy = "Name";
            config.SortDirection = SortDirection.Ascending;
            IEnumerable<CollectionSchema> dataAsc = await dataProvider.LoadDataAsync(config, 2);
            dataAsc = await dataProvider.LoadMoreDataAsync();
            config.SortDirection = SortDirection.Descending;
            IEnumerable<CollectionSchema> dataDesc = await dataProvider.LoadDataAsync(config, 2);
            dataDesc = await dataProvider.LoadMoreDataAsync();

            Assert.AreNotEqual(data.FirstOrDefault()?.Name, dataAsc.FirstOrDefault().Name);
           
            var dataExpected = dataAsc.OrderBy(x => x.Name).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Name, dataAsc.ToList()[i].Name);
            }

            dataExpected = dataDesc.OrderByDescending(x => x.Name).ToList();
            for (int i = 0; i < dataExpected.Count() - 1; i++)
            {
                Assert.AreEqual(dataExpected[i].Name, dataDesc.ToList()[i].Name);
            }
        }
    }

    public class CollectionSchema2 : SchemaBase
    {
        public string Title { get; set; }

        public DateTime? DateTime { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }
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
