using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.TouchDevelop;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public class TouchDevelopTestLibrary
    {
        [TestMethod]
        public async Task LoadTD()
        {
            var config = new TouchDevelopDataConfig()
            {
                LocalDataSource = "/Assets/TouchDevelopData.json",
                ScriptId = "leasc"
            };

            var dataProvider = new TouchDevelopDataProvider();
            IEnumerable<TouchDevelopSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Count(), 1);
        }

        [TestMethod]
        public async Task LoadTDRemote()
        {
            var config = new TouchDevelopDataConfig()
            {
                ScriptId = "leasc"
            };

            var dataProvider = new TouchDevelopDataProvider();
            IEnumerable<TouchDevelopSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Count(), 1);
        }
    }
}
