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
                FilterBy = "themes"
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
        public async Task GetComments()
        {
            var dataProvider = new WordPressDataProvider();
            var result = await dataProvider.GetComments("en.blog.wordpress.com", "32497", 20);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.Count(), 0);
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

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<WordPressSchema>(new WordPressDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressPost_Min()
        {
            int maxRecords = 1;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);         

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressPost()
        {
            int maxRecords = 70;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(data.Count() > 25);
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressTag_Min()
        {
            int maxRecords = 1;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressTag()
        {
            int maxRecords = 70;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "wordpress"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(data.Count() > 25);
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressCategory_Min()
        {
            int maxRecords = 1;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressCategory()
        {
            int maxRecords = 70;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(data.Count() > 25);
        }

        [TestMethod]
        public async Task LoadPaginationWordPressPost()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<WordPressSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataBeforeLoadDataWordPressPost()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

        [TestMethod]
        public async Task LoadPaginationWordPressCategory()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<WordPressSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationWordPressCategory()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }


        [TestMethod]
        public async Task LoadPaginationWordPressTag()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<WordPressSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationWordPressTag()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

        [TestMethod]
        public async Task LoadCommentsPaginationWordPress()
        {
            var dataProvider = new WordPressDataProvider();
            var site = "en.blog.wordpress.com";
            var postId = "32497";
            var maxId = 5;
            await dataProvider.GetComments(site, postId, maxId);

            Assert.IsTrue(dataProvider.HasMoreComments, nameof(dataProvider.HasMoreComments));

            var result = await dataProvider.GetMoreComments();

            Assert.IsNotNull(result, $"{nameof(result)} is not null");
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreCommentsPagination_InvalidOperationWordPress()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }
    }
}
