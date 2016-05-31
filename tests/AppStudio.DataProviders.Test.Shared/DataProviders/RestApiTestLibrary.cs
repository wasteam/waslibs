using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Windows.Web.Http;

using AppStudio.DataProviders.RestApi;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Core;

using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
            Assert.IsTrue(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task LoadDataPageNumberPagination()
        {
            var pagination = new PageNumberPagination("page", false, "");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataPageNumberPagination_NullParameters()
        {
            var pagination = new PageNumberPagination(null, false, null);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }       

        [TestMethod]
        public async Task LoadDataItemOffsetPagination()
        {
            var pagination = new ItemOffsetPagination("offset", true, "", 20);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataItemOffsetPagination_NullParameters()
        {
            var pagination = new ItemOffsetPagination(null, true, null, 20);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
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
            var pagination = new TokenPagination("page_handle", "next_page", "number");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataTokenPagination_NullParameters()
        {
            var pagination = new TokenPagination(null, null, null);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataTokenPagination_InvalidTokenPathName()
        {
            var pagination = new TokenPagination(null, "invalid", null);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataNextPageUrlPagination()
        {
            var pagination = new NextPageUrlPagination("paging.next", string.Empty);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://graph.facebook.com/v2.5/8195378771/posts?&access_token=351842111678417|74b187b46cf37a8ef6349b990bc039c2&fields=id,message,from,created_time,link,full_picture"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<Facebook.FacebookSchema> data = await dataProvider.LoadDataAsync(config, 20, new Facebook.FacebookParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<Facebook.FacebookSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataNextPageUrlPagination_NullParameters()
        {
            var pagination = new NextPageUrlPagination(null, null);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://graph.facebook.com/v2.5/8195378771/posts?&access_token=351842111678417|74b187b46cf37a8ef6349b990bc039c2&fields=id,message,from,created_time,link,full_picture"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<Facebook.FacebookSchema> data = await dataProvider.LoadDataAsync(config, 20, new Facebook.FacebookParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            ArgumentNullException exception = await ExceptionsAssert.ThrowsAsync<ArgumentNullException>(async () => await dataProvider.LoadMoreDataAsync<Facebook.FacebookSchema>());          
        }

        [TestMethod]
        public async Task LoadDataNextPageUrlPagination_NextPagePath()
        {
            var pagination = new NextPageUrlPagination("invalid", null);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://graph.facebook.com/v2.5/8195378771/posts?&access_token=351842111678417|74b187b46cf37a8ef6349b990bc039c2&fields=id,message,from,created_time,link,full_picture"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<Facebook.FacebookSchema> data = await dataProvider.LoadDataAsync(config, 20, new Facebook.FacebookParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            ArgumentNullException exception = await ExceptionsAssert.ThrowsAsync<ArgumentNullException>(async () => await dataProvider.LoadMoreDataAsync<Facebook.FacebookSchema>());
        }

        [TestMethod]
        public async Task LoadDataMemoryPagination()
        {
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 5, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadDataTokenPagination_QueryString()
        {
            var pagination = new TokenPagination("page_handle", "next_page", "number");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/?tag=wordpress"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, 20, new WordPress.WordPressParser());

            Assert.IsTrue(dataProvider.HasMoreItems, $"{nameof(dataProvider.HasMoreItems)} is false");
            data = await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>();

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }       

        [TestMethod]
        public async Task LoadMoreDataInvalidOperation()
        {
            var pagination = new TokenPagination("page_handle", "next_page", string.Empty);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync<WordPress.WordPressSchema>());
            Assert.IsFalse(dataProvider.IsInitialized);
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
            var pagination = new TokenPagination("page_handle", "next_page", string.Empty);
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
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
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task TestMaxRecordsRestApiDataProvider()
        {
            int maxRecords = 50;
            var pagination = new TokenPagination(string.Empty, string.Empty, "number");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.IsTrue(data.Count() > 20);
        }

        [TestMethod]
        public async Task TestMaxRecordsRestApiDataProvider_Min()
        {
            int maxRecords = 1;
            var pagination = new TokenPagination(string.Empty, string.Empty, "number");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/")
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsRestApiDataProvider_QueryString()
        {
            int maxRecords = 50;
            var pagination = new TokenPagination(string.Empty, string.Empty, "number");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/?tag=wordpress"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.IsTrue(data.Count() > 20);
        }
        [TestMethod]
        public async Task LoadData_Headers()
        {
            var headers = new Dictionary<string, string>();

            var config = new RestApiDataConfig
            {
                Url = new Uri("https://www.googleapis.com/youtube/v3/search?q=Windows&part=snippet&key=AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ&type=video&maxResults=20&order=date"),
                Headers = headers
            };
            headers["Accept-Encoding"] = "gzip";
            headers["User-Agent"] = "Mozilla/5.0 (gzip)";
            headers["Test-Header"] = "Test header";
            var dataProvider = new RestApiDataProvider();
            IEnumerable<YouTube.YouTubeSchema> result = await dataProvider.LoadDataAsync(config, 20, new YouTube.YouTubeSearchParser());
        }

        [TestMethod]
        public async Task TestRestApiDataProvider_Sorting()
        {
            int maxRecords = 50;
            var pagination = new TokenPagination(string.Empty, string.Empty, "number");
            var config = new RestApiDataConfig
            {
                Url = new Uri(@"https://public-api.wordpress.com/rest/v1.1/sites/en.blog.wordpress.com/posts/?tag=wordpress"),
                PaginationConfig = pagination
            };

            var dataProvider = new RestApiDataProvider();
            IEnumerable<WordPress.WordPressSchema> sortedData = await dataProvider.LoadDataAsync(config, maxRecords, new SortingParser());
            IEnumerable<WordPress.WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords, new WordPress.WordPressParser());

            Assert.AreNotEqual(data.FirstOrDefault().Title, sortedData.FirstOrDefault().Title);
        }


        private class SortingParser : IParser<WordPress.WordPressSchema>
        {
            public IEnumerable<WordPress.WordPressSchema> Parse(string data)
            {
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                var result = new Collection<WordPress.WordPressSchema>();
                JToken jtokenData = JsonConvert.DeserializeObject<JToken>(data);
                IEnumerable<JToken> elements = jtokenData.SelectToken("posts")?.Select(s => s);
                if (elements != null)
                {
                    foreach (JToken item in elements)
                    {
                        var itemResult = new WordPress.WordPressSchema();
                        itemResult._id = (string)item.SelectToken("ID");
                        itemResult.Title = (string)item.SelectToken("title");
                        result.Add(itemResult);
                    }
                }
                return result.OrderBy(x => x._id);
            }
        }       
    }
}
