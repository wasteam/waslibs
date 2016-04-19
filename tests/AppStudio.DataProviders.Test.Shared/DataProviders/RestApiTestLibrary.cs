using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppStudio.DataProviders.RestApi;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AppStudio.DataProviders.Exceptions;
using Windows.Web.Http;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class RestApiTestLibrary
    {
        [TestMethod]
        public async Task LoadDataNoPagination()
        {
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsFalse(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is true");

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataNumericPagination()
        {
            var pager = new NumericPager() { IncrementalValue = 1, PaginationParameterName = "page" };
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                Pager = pager
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataTokenPagination()
        {
            var pager = new TokenPager() { PaginationParameterName = "page_handle", ContinuationTokenPath = "next_page" };
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                Pager = pager
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataTokenPagination_QueryString()
        {
            var pager = new TokenPager() { PaginationParameterName = "page_handle", ContinuationTokenPath = "next_page" };
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/?tag=wordpress"),
                Pager = pager
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataTokenPagination_TokenIsUrl()
        {
            var pager = new TokenPager() { ContinuationTokenPath = "paging.next", ContinuationTokenIsUrl = true };
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://graph.facebook.com/v2.5/8195378771/posts?&access_token=351842111678417|74b187b46cf37a8ef6349b990bc039c2&fields=id,message,from,created_time,link,full_picture"),
                Pager = pager
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<Facebook.FacebookSchema> data = await dataProvider.LoadDataAsync(config, 20, new Facebook.FacebookParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<Facebook.FacebookSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperation()
        {
            var pager = new TokenPager() { PaginationParameterName = "page_handle", ContinuationTokenPath = "next_page" };
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                Pager = pager
            };

            var dataProvider = new RestApiDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>());
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new RestApiDataProvider();
            ConfigNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null, 20, new WordPress.WordPressParser()));
        }

        [TestMethod]
        public async Task TestValidateConfig()
        {
            var config = new RestApiDataConfig();
            var dataProvider = new RestApiDataProvider();
            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser()));
            Assert.IsTrue(exception.Message.Contains(nameof(RestApiDataConfig.Url)));
        }

        [TestMethod]
        public async Task TestValidateConfig_GetApiDataAsync()
        {
            var config = new RestApiDataConfig();
            var dataProvider = new RestApiDataProvider();
            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.GetApiDataAsync(config, 20, new WordPress.WordPressParser()));
            Assert.IsTrue(exception.Message.Contains(nameof(RestApiDataConfig.Url)));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };
            var dataProvider = new RestApiDataProvider();
            ParserNullException exception = await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<WordPress.WordPressSchema>(config, 20, null));
        }

        [TestMethod]
        public async Task TestNullParser_GetApiDataAsync()
        {
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };
            var dataProvider = new RestApiDataProvider();
            ParserNullException exception = await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.GetApiDataAsync<WordPress.WordPressSchema>(config, 20, null));
        }

        [TestMethod]
        public async Task TestGetApiDataAsync()
        {
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };
            var dataProvider = new RestApiDataProvider();
            var data = await dataProvider.GetApiDataAsync(config, 20, new WordPress.WordPressParser());
            Assert.AreEqual((int)HttpStatusCode.Ok, data.HttpStatusCode);
        }

        [TestMethod]
        public async Task TestGetMoreApiDataAsync()
        {
            var pager = new TokenPager() { PaginationParameterName = "page_handle", ContinuationTokenPath = "next_page" };
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                Pager = pager
            };
            var dataProvider = new RestApiDataProvider();
            var data = await dataProvider.GetApiDataAsync(config, 20, new WordPress.WordPressParser());
            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");

            data = await dataProvider.GetMoreApiDataAsync<WordPress.WordPressSchema>();
            Assert.AreEqual((int)HttpStatusCode.Ok, data.HttpStatusCode);
        }

        [TestMethod]
        public async Task TestGetMoreApiDataAsyncInvalidOperation()
        {
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };
            var dataProvider = new RestApiDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.GetMoreApiDataAsync<WordPress.WordPressSchema>());
        }

        [TestMethod]
        public async Task TestMaxRecordsRestApiDataProvider()
        {
            int maxRecords = 50;
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PageSizeParameterName = "number"
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.IsTrue(data.Count() > 20);
        }

        [TestMethod]
        public async Task TestMaxRecordsRestApiDataProvider_Min()
        {
            int maxRecords = 1;
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PageSizeParameterName = "number"
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.AreEqual(maxRecords, data.Count());
        }


        [TestMethod]
        public async Task TestMaxRecordsRestApiDataProvider_QueryString()
        {
            int maxRecords = 50;
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/?tag=wordpress"),
                PageSizeParameterName = "number"
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.IsTrue(data.Count() > 20);
        }
    }
}
