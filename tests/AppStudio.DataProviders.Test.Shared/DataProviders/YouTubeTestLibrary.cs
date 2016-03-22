using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.YouTube;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class YouTubeTestLibrary
    {
        [TestMethod]
        public async Task TestPlaylist()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Playlist,
                Query = @"PLB9EA94DACBEC74A9"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestVideos()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"windows app studio"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestChannel()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Channels,
                Query = @"elrubiusOMG"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestRevokedOAuth()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"lumia"
            };

            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeRevokedKeys);

            await ExceptionsAssert.ThrowsAsync<RequestFailedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestInvalidOAuth()
        {
            YouTubeDataConfig config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"lumia"
            };

            var tokens = new YouTubeOAuthTokens
            {
                ApiKey = "INVALID"
            };

            YouTubeDataProvider dataProvider = new YouTubeDataProvider(tokens);

            RequestFailedException exception = await ExceptionsAssert.ThrowsAsync<RequestFailedException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("status code 400"));
        }

        [TestMethod]
        public async Task TestEmptyOAuth()
        {
            YouTubeDataConfig config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"lumia"
            };
            YouTubeDataProvider dataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens());

            OAuthKeysNotPresentException exception = await ExceptionsAssert.ThrowsAsync<OAuthKeysNotPresentException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("ApiKey"));
        }

        [TestMethod]
        public async Task TestNullOAuth()
        {
            YouTubeDataConfig config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"lumia"
            };
            YouTubeDataProvider dataProvider = new YouTubeDataProvider(null);

            await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            YouTubeDataProvider dataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens());

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            YouTubeDataProvider dataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens());

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<YouTubeSchema>(new YouTubeDataConfig(), 20, null));
        }


        [TestMethod]
        public async Task TestMaxRecordsVideos()
        {
            int maxRecords = 50;
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"Microsoft"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);
              
            Assert.AreEqual(maxRecords, result.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsPlaylist()
        {
            int maxRecords = 50;
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Playlist,
                Query = @"PLB9EA94DACBEC74A9"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsChannels()
        {
            int maxRecords = 50;
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Channels,
                Query = @"elrubiusOMG"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsVideos_Min()
        {
            int maxRecords = 1;
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"Microsoft"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsPlaylist_Min()
        {
            int maxRecords = 1;
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Playlist,
                Query = @"PLB9EA94DACBEC74A9"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsChannels_Min()
        {
            int maxRecords = 1;
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Channels,
                Query = @"elrubiusOMG"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());
        }

    }
}
