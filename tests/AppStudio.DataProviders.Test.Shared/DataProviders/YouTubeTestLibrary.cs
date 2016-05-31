using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.YouTube;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

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
            Assert.IsTrue(dataProvider.IsInitialized);
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
            Assert.IsTrue(dataProvider.IsInitialized);
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
            Assert.IsTrue(dataProvider.IsInitialized);
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

            Assert.IsTrue(result.Count() > 20);
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

            Assert.IsTrue(result.Count() > 20);
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

            Assert.IsTrue(result.Count() > 20);
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

        [TestMethod]
        public async Task TestPaginationPlaylist()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Playlist,
                Query = @"PLB9EA94DACBEC74A9"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            await dataProvider.LoadDataAsync(config, 2);

            IEnumerable<YouTubeSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationPlaylist()
        {

            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Playlist,
                Query = @"PLB9EA94DACBEC74A9"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task TestPaginationVideos()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"windows app studio"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            await dataProvider.LoadDataAsync(config, 2);

            IEnumerable<YouTubeSchema> result = await dataProvider.LoadMoreDataAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationVideos()
        {

            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = @"windows app studio"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task TestPaginationChannel()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Channels,
                Query = @"elrubiusOMG"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            await dataProvider.LoadDataAsync(config, 2);

            IEnumerable<YouTubeSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationChannel()
        {

            var config = new YouTubeDataConfig
            {

                QueryType = YouTubeQueryType.Channels,
                Query = @"elrubiusOMG"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task TestLoadChannel()
        {
            var channel = @"elrubiusOMG";
            var page = 1;

            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadChannelAsync(channel, page);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestPaginationLoadChannel()
        {
            var channel = @"elrubiusOMG";
            var page = 1;

            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            await dataProvider.LoadChannelAsync(channel, page);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<YouTubeSchema> result = await dataProvider.LoadMoreChannelAsync(channel, page);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestInvalidOperationLoadChannel()
        {
            var channel = @"elrubiusOMG";
            var pageSize = 1;
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreChannelAsync(channel, pageSize));
            Assert.IsFalse(dataProvider.IsInitialized);
        }

        [TestMethod]
        public async Task TestVideos_Sorting()
        {
            var config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = "windows app studio"
            };
            var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config);
            IEnumerable<YouTubeSchema> moreResult = await dataProvider.LoadMoreDataAsync();

            config = new YouTubeDataConfig
            {
                QueryType = YouTubeQueryType.Videos,
                Query = "windows app studio",
                SearchVideosOrderBy = YouTubeSearchOrderBy.Date
            };
            var sortingDataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
            IEnumerable<YouTubeSchema> sortedResult = await sortingDataProvider.LoadDataAsync(config);
            IEnumerable<YouTubeSchema> moreSortedResult = await sortingDataProvider.LoadMoreDataAsync();


            Assert.AreNotEqual(result.FirstOrDefault().Title, sortedResult.FirstOrDefault().Title, "LoadDataAsync: YouTube sorting is not working");
            Assert.AreNotEqual(moreResult.FirstOrDefault().Title, moreSortedResult.FirstOrDefault().Title, "LoadMoreDataAsync: YouTube sorting is not working");
        }

        [TestMethod]
        public async Task TestVideos_AllOrderBy()
        {
            var enums = Enum.GetValues(typeof(YouTubeSearchOrderBy)).Cast<YouTubeSearchOrderBy>().Where(x => x != YouTubeSearchOrderBy.None);
            foreach (YouTubeSearchOrderBy orderby in enums)
            {
                var config = new YouTubeDataConfig
                {
                    QueryType = YouTubeQueryType.Videos,
                    Query = "windows app studio",
                    SearchVideosOrderBy = orderby
                };
                var dataProvider = new YouTubeDataProvider(OAuthKeys.YouTubeValidKeys);
                IEnumerable<YouTubeSchema> result = await dataProvider.LoadDataAsync(config, 5);              
            }
        }
    }
}
