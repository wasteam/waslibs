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
        private string _listId;

        protected override bool HasMoreItems
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ContinuationToken);
            }
        }

        public YouTubeDataProvider(YouTubeOAuthTokens tokens)
        {
            _tokens = tokens;
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(YouTubeDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            IEnumerable<TSchema> result;

            switch (config.QueryType)
            {
                case YouTubeQueryType.Channels:
                    result = await LoadChannelAsync(config.Query, pageSize, parser);
                    break;
                case YouTubeQueryType.Videos:
                    result = await SearchAsync(config.Query, pageSize, parser);
                    break;
                case YouTubeQueryType.Playlist:
                default:
                    result = await LoadPlaylistAsync(config.Query, pageSize, parser);
                    break;
            }

            return result;
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(YouTubeDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            IEnumerable<TSchema> result;

            switch (config.QueryType)
            {
                case YouTubeQueryType.Channels:
                    result = await LoadMoreDataPlaylistAsync(_listId, pageSize, parser);
                    break;
                case YouTubeQueryType.Videos:
                    result = await LoadMoreDataSearchAsync(config.Query, pageSize, parser);
                    break;
                case YouTubeQueryType.Playlist:
                default:
                    result = await LoadMoreDataPlaylistAsync(config.Query, pageSize, parser);
                    break;
            }

            return result;
        }

        public async Task<IEnumerable<YouTubeSchema>> LoadChannelAsync(string channel, int pageSize)
        {
            return await LoadChannelAsync(channel, pageSize, new YouTubePlaylistParser());
        }

        public async Task<IEnumerable<TSchema>> LoadChannelAsync<TSchema>(string channel, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            _listId = await GetUploadVideosListId(channel, pageSize);
            if (!string.IsNullOrEmpty(_listId))
            {
                return await LoadPlaylistAsync(_listId, pageSize, parser);
            }
            return new TSchema[0];
        }

        protected override IParser<YouTubeSchema> GetDefaultParserInternal(YouTubeDataConfig config)
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

        private async Task<IEnumerable<TSchema>> SearchAsync<TSchema>(string query, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(GetSearchUrl(query, pageSize), UriKind.Absolute)
            };

            return await GetDataFromProvider(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> LoadMoreDataSearchAsync<TSchema>(string query, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var requestUrl = GetSearchUrl(query, pageSize);
            var continuacionUrl = GetContinuationUrl(requestUrl);
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(continuacionUrl)
            };

            return await GetDataFromProvider(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> LoadPlaylistAsync<TSchema>(string playlistId, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(GetPlaylistUrl(playlistId, pageSize), UriKind.Absolute)
            };

            return await GetDataFromProvider(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> LoadMoreDataPlaylistAsync<TSchema>(string playlistId, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var requestUrl = GetPlaylistUrl(playlistId, pageSize);
            var continuacionUrl = GetContinuationUrl(requestUrl);
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(continuacionUrl)
            };

            return await GetDataFromProvider(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> GetDataFromProvider<TSchema>(IParser<TSchema> parser, HttpRequestSettings settings) where TSchema : SchemaBase
        {
            var requestResult = await HttpRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                ContinuationToken = GetContinuationToken(requestResult.Result);
                return parser.Parse(requestResult.Result);
            }

            if (requestResult.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException(requestResult.StatusCode, requestResult.Result);
        }

        private async Task<string> GetUploadVideosListId(string channel, int pageSize)
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(GetChannelUrl(channel, pageSize), UriKind.Absolute)
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

        private string GetChannelUrl(string channel, int pageSize)
        {
            var url = $"{BaseUrl}/channels?forUsername={channel}&part=contentDetails&maxResults={pageSize}&key={_tokens.ApiKey}";
            return url;
        }

        private string GetPlaylistUrl(string playlistId, int pageSize)
        {
            var url = $"{BaseUrl}/playlistItems?playlistId={playlistId}&part=snippet&maxResults={pageSize}&key={_tokens.ApiKey}";
            return url;
        }

        private string GetSearchUrl(string query, int pageSize)
        {
            var url = $"{BaseUrl}/search?q={query}&part=snippet&maxResults={pageSize}&key={_tokens.ApiKey}&type=video";
            return url;
        }

        private string GetContinuationUrl(string url)
        {
            url += $"&pageToken={ContinuationToken}";
            return url;
        }

        private string GetContinuationToken(string data)
        {
            var youTubeResponse = JsonConvert.DeserializeObject<YouTubeResult<dynamic>>(data);
            return youTubeResponse?.nextPageToken;
        }
    }
}
