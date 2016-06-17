using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Twitter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public class TwitterTestLibrary
    {
        [TestMethod]
        public async Task TestHomeTimeLine()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Home
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestUserTimeLine()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestSearch()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestRevokedOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };

            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterRevokedKeys);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestInvalidOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };

            var tokens = new TwitterOAuthTokens
            {
                ConsumerKey = "INVALID",
                ConsumerSecret = "INVALID",
                AccessToken = "INVALID",
                AccessTokenSecret = "INVALID"
            };

            var dataProvider = new TwitterDataProvider(tokens);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestEmptyOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            OAuthKeysNotPresentException exception = await ExceptionsAssert.ThrowsAsync<OAuthKeysNotPresentException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("ConsumerKey"));
        }

        [TestMethod]
        public async Task TestNullQuery()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User
            };
            var dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(null);

            await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            TwitterDataProvider dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            TwitterDataProvider dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<TwitterSchema>(new TwitterDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestMaxRecordsHomeTimeLine_Min()
        {
            int maxRecords = 1;
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Home
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(result.Count() <= maxRecords);
        }

        [TestMethod]
        public async Task TestMaxRecordsHomeTimeLine()
        {
            int maxRecords = 70;
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Home
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(result.Count() > 15, $"There are {result.Count()} elements");
        }

        [TestMethod]
        public async Task TestMaxRecordsUserTimeLine_Min()
        {
            int maxRecords = 1;
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(result.Count() <= maxRecords);
        }

        [TestMethod]
        public async Task TestMaxRecordsUserTimeLine()
        {
            int maxRecords = 70;
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());

            Assert.IsTrue(result.Count() > 15, $"There are {result.Count()} elements");
        }

        [TestMethod]
        public async Task TestMaxRecordsSearch_Min()
        {
            int maxRecords = 1;
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = "microsoft"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(result.Count() <= maxRecords);
        }

        [TestMethod]
        public async Task TestMaxRecordsSearch()
        {
            int maxRecords = 70;
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = "microsoft"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(result.Count() > 15, $"There are {result.Count()} elements");
        }

        [TestMethod]
        public async Task LoadPaginationTwitterHomeTimeLine()
        {

            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Home
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<TwitterSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationTwitterHomeTimeLine()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Home
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

        [TestMethod]
        public async Task LoadPaginationTwitterUserTimeLine()
        {

            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<TwitterSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationTwitterUserTimeLine()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

        [TestMethod]
        public async Task LoadPaginationTwitterSearch()
        {

            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = "microsoft"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            await dataProvider.LoadDataAsync(config, 2);

            Assert.IsTrue(dataProvider.HasMoreItems, "HasMoreItems is false");

            IEnumerable <TwitterSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(), "There aren't any elements");
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationTwitterSearch()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }
    }
}
