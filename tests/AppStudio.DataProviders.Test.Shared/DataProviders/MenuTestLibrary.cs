using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Menu;
using AppStudio.DataProviders.LocalStorage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class MenuTestLibrary
    {
        [TestMethod]
        public async Task LoadMenu()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/LocalMenuData.json"
            };

            var dataProvider = new LocalStorageDataProvider<MenuSchema>();
            IEnumerable<MenuSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task TestNullUrlConfig()
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = null
            };

            var dataProvider = new LocalStorageDataProvider<MenuSchema>();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("FilePath"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new LocalStorageDataProvider<MenuSchema>();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new LocalStorageDataProvider<MenuSchema>();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync(new LocalStorageDataConfig(), null));
        }
    }
}
