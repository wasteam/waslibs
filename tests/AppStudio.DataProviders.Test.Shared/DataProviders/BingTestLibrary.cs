using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Bing;
using AppStudio.DataProviders.Exceptions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class BingTestLibrary
    {
        [TestMethod]
        public async Task LoadBing()
        {
            var config = new BingDataConfig
            {
                Query = "Windows App Studio",
                Country = BingCountry.UnitedStates
            };

            var dataProvider = new BingDataProvider();
            IEnumerable<BingSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);           
        }

        [TestMethod]
        public async Task TestNullQueryConfig()
        {
            var config = new BingDataConfig
            {
                Query = null,
                Country = BingCountry.UnitedStates
            };

            var dataProvider = new BingDataProvider();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new BingDataProvider();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new BingDataProvider();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<BingSchema>(new BingDataConfig(), 20, null));
        }      
       
        [TestMethod]
        public async Task LoadPaginationBing()
        {
            var config = new BingDataConfig
            {
                Query = "Windows App Studio",
                Country = BingCountry.UnitedStates
            };

            var dataProvider = new BingDataProvider();
            await dataProvider.LoadDataAsync(config, 20);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<BingSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationBing()
        {
            var config = new BingDataConfig
            {
                Query = "Windows App Studio",
                Country = BingCountry.UnitedStates
            };

            var dataProvider = new BingDataProvider();          
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());

        }
    }
}
