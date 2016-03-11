using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Bing;
using AppStudio.DataProviders.Exceptions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
        public async Task TestMaxRecords_1()
        {
            int maxRecords = 1;
            var config = new BingDataConfig
            {
                Query = "Windows App Studio",
                Country = BingCountry.UnitedStates
            };

            var dataProvider = new BingDataProvider();
            IEnumerable<BingSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecords_50()
        {
            int maxRecords = 50;
            var config = new BingDataConfig
            {
                Query = "Windows App Studio",
                Country = BingCountry.UnitedStates
            };

            var dataProvider = new BingDataProvider();
            IEnumerable<BingSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords - 1, data.Count());
        }
    }
}
