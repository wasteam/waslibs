using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

using Newtonsoft.Json;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;


namespace AppStudio.DataProviders.YouTube
{
    public class YouTubeDataProvider : DataProviderBase<YouTubeDataConfig, YouTubeSchema>
    {
        private const string BaseUrl = @"https://www.googleapis.com/youtube/v3";
        private YouTubeOAuthTokens _tokens;
        private string _continuationToken;

        public override bool HasMoreItems
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_continuationToken);
            }
        }

        public YouTubeDataProvider(YouTubeOAuthTokens tokens)
        {
            _tokens = tokens;
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(YouTubeDataConfig config, int maxRecords, IPaginationParser<TSchema> parser)
        {
            IEnumerable<TSchema> result;

            switch (config.QueryType)
            {
                case YouTubeQueryType.Channels:
                    result = await LoadChannelAsync(config.Query, maxRecords, parser);
                    break;
                case YouTubeQueryType.Videos:
                    result = await SearchAsync(config.Query, maxRecords, parser);
                    break;
                case YouTubeQueryType.Playlist:
                default:
                    result = await LoadPlaylistAsync(config.Query, maxRecords, parser);
                    break;
            }

            return result;
        }

        protected override IPaginationParser<YouTubeSchema> GetDefaultParserInternal(YouTubeDataConfig config)
        {
            switch (config.QueryType)
            {
                case YouTubeQueryType.Videos:
                    return new YouTubeSearchParser();
                case YouTubeQueryType.Channels:
                case YouTubeQueryType.Playlist:
                default:
                    return new YouTubePlaylistParser();
            }
        }

        public async Task<IEnumerable<YouTubeSchema>> LoadChannelAsync(string channel, int maxRecords)
        {
            return await LoadChannelAsync(channel, maxRecords, new YouTubePlaylistParser());
        }

        public async Task<IEnumerable<TSchema>> LoadChannelAsync<TSchema>(string channel, int maxRecords, IPaginationParser<TSchema> parser) where TSchema : SchemaBase
        {
            var listId = await GetUploadVideosListId(channel, maxRecords);
            if (!string.IsNullOrEmpty(listId))
            {
                return await LoadPlaylistAsync(listId, maxRecords, parser);
            }
            return new TSchema[0];
        }

        protected override void ValidateConfig(YouTubeDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
            if (_tokens == null)
            {
                throw new ConfigParameterNullException("Tokens");
            }
            if (string.IsNullOrEmpty(_tokens.ApiKey))
            {
                throw new OAuthKeysNotPresentException("ApiKey");
            }
        }

        private async Task<IEnumerable<TSchema>> SearchAsync<TSchema>(string query, int maxRecords, IPaginationParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = GetSearchUrl(query, maxRecords)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var responseResult = parser.Parse(result.Result);
                _continuationToken = responseResult.ContinuationToken;
                return responseResult.GetItems();
            }

            if (result.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private async Task<IEnumerable<TSchema>> LoadPlaylistAsync<TSchema>(string playlistId, int maxRecords, IPaginationParser<TSchema> parser) where TSchema : SchemaBase
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = GetPlaylistUrl(playlistId, maxRecords)
            };

            var requestResult = await HttpRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                var responseResult = parser.Parse(requestResult.Result);
                _continuationToken = responseResult.ContinuationToken;
                return responseResult.GetItems();
            }              

            if (requestResult.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException(requestResult.StatusCode, requestResult.Result);
        }

        private async Task<string> GetUploadVideosListId(string channel, int maxRecords)
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = GetChannelUrl(channel, maxRecords)
            };

            var requestResult = await HttpRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                var channelResult = JsonConvert.DeserializeObject<YouTubeResult<YouTubeChannelLookupResult>>(requestResult.Result);
                if (channelResult != null
                    && channelResult.items != null
                    && channelResult.items.Count > 0
                    && channelResult.items[0].contentDetails != null
                    && channelResult.items[0].contentDetails.relatedPlaylists != null
                    && !string.IsNullOrEmpty(channelResult.items[0].contentDetails.relatedPlaylists.uploads))
                {
                    return channelResult.items[0].contentDetails.relatedPlaylists.uploads;
                }
                else
                {
                    return string.Empty;
                }
            }

            throw new RequestFailedException(requestResult.StatusCode, requestResult.Result);
        }

        private Uri GetChannelUrl(string channel, int maxRecords)
        {
            var url = $"{BaseUrl}/channels?forUsername={channel}&part=contentDetails&maxResults={maxRecords}&key={_tokens.ApiKey}";
            url = BuildPaginationUrl(url);
            return new Uri(url, UriKind.Absolute);
        }

        private Uri GetPlaylistUrl(string playlistId, int maxRecords)
        {
            var url = $"{BaseUrl}/playlistItems?playlistId={playlistId}&part=snippet&maxResults={maxRecords}&key={_tokens.ApiKey}";
            url = BuildPaginationUrl(url);
            return new Uri(url, UriKind.Absolute);
        }

        private Uri GetSearchUrl(string query, int maxRecords)
        {
            var url = $"{BaseUrl}/search?q={query}&part=snippet&maxResults={maxRecords}&key={_tokens.ApiKey}&type=video";
            url = BuildPaginationUrl(url);
            return new Uri(url, UriKind.Absolute);
        }

        private string BuildPaginationUrl(string url)
        {
            if (HasMoreItems)
            {
                url += $"&pageToken={_continuationToken}";
            }
            return url;
        }
    }
}
