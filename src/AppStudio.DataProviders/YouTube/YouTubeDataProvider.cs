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

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(YouTubeDataConfig config, int maxRecords, IParser<TSchema> parser)
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

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(YouTubeDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            IEnumerable<TSchema> result;

            switch (config.QueryType)
            {
                case YouTubeQueryType.Channels:
                    result = await LoadMoreDataPlaylistAsync(_listId, maxRecords, parser);
                    break;
                case YouTubeQueryType.Videos:
                    result = await LoadMoreDataSearchAsync(config.Query, maxRecords, parser);
                    break;
                case YouTubeQueryType.Playlist:
                default:
                    result = await LoadMoreDataPlaylistAsync(config.Query, maxRecords, parser);
                    break;
            }

            return result;
        }

        public async Task<IEnumerable<YouTubeSchema>> LoadChannelAsync(string channel, int maxRecords)
        {           
            return await LoadChannelAsync(channel, maxRecords, new YouTubePlaylistParser());
        }

        public async Task<IEnumerable<TSchema>> LoadChannelAsync<TSchema>(string channel, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            _listId = await GetUploadVideosListId(channel, maxRecords);
            if (!string.IsNullOrEmpty(_listId))
            {
                return await LoadPlaylistAsync(_listId, maxRecords, parser);
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

        private async Task<IEnumerable<TSchema>> SearchAsync<TSchema>(string query, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = GetSearchUrl(query, maxRecords)
            };

            return await GetDataInternal(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> LoadMoreDataSearchAsync<TSchema>(string query, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var requestUri = GetSearchUrl(query, maxRecords);
            var continuacionUrl = GetContinuationUrl(requestUri.AbsoluteUri);
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(continuacionUrl)
            };

            return await GetDataInternal(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> LoadPlaylistAsync<TSchema>(string playlistId, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = GetPlaylistUrl(playlistId, maxRecords)
            };

            return await GetDataInternal(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> LoadMoreDataPlaylistAsync<TSchema>(string playlistId, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var requestUri = GetPlaylistUrl(playlistId, maxRecords);
            var continuacionUrl = GetContinuationUrl(requestUri.AbsoluteUri);
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(continuacionUrl)
            };

            return await GetDataInternal(parser, settings);
        }

        private async Task<IEnumerable<TSchema>> GetDataInternal<TSchema>(IParser<TSchema> parser, HttpRequestSettings settings) where TSchema : SchemaBase
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
            return new Uri(url, UriKind.Absolute);
        }

        private Uri GetPlaylistUrl(string playlistId, int maxRecords)
        {
            var url = $"{BaseUrl}/playlistItems?playlistId={playlistId}&part=snippet&maxResults={maxRecords}&key={_tokens.ApiKey}";         
            return new Uri(url, UriKind.Absolute);
        }

        private Uri GetSearchUrl(string query, int maxRecords)
        {
            var url = $"{BaseUrl}/search?q={query}&part=snippet&maxResults={maxRecords}&key={_tokens.ApiKey}&type=video";          
            return new Uri(url, UriKind.Absolute);
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
