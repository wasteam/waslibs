using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.WordPress;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class WordPressTestLibrary
    {
        [TestMethod]
        public async Task LoadWordPressPost()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }
        [TestMethod]
        public async Task LoadWordPressCategory()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadWordPressTag()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task TestNullQueryConfig()
        {
            var config = new WordPressDataConfig
            {
                Query = null,
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new WordPressDataProvider();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new WordPressDataProvider();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync(new WordPressDataConfig(), null));
        }
    }
}
