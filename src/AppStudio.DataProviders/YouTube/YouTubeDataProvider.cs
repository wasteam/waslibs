using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using Newtonsoft.Json;
using Windows.Web.Http;

namespace AppStudio.DataProviders.YouTube
{
    public class YouTubeDataProvider : DataProviderBase<YouTubeDataConfig, YouTubeSchema>
    {
        private const string BaseUrl = @"https://www.googleapis.com/youtube/v3";
        private YouTubeOAuthTokens _tokens;

        public YouTubeDataProvider(YouTubeOAuthTokens tokens)
        {
            _tokens = tokens;
        }

        public override async Task<IEnumerable<YouTubeSchema>> LoadDataAsync(YouTubeDataConfig config)
        {
            IParser<YouTubeSchema> parser = null;
            if (config != null)
            {
                switch (config.QueryType)
                {
                    case YouTubeQueryType.Videos:
                        parser = new YouTubeSearchParser();
                        break;
                    case YouTubeQueryType.Channels:
                    case YouTubeQueryType.Playlist:
                    default:
                        parser = new YouTubePlaylistParser();
                        break;
                }
            }

            return await LoadDataAsync(config, parser);
        }

        public override async Task<IEnumerable<YouTubeSchema>> LoadDataAsync(YouTubeDataConfig config, IParser<YouTubeSchema> parser)
        {
            Assertions(config, parser);

            IEnumerable<YouTubeSchema> result;

            switch (config.QueryType)
            {
                case YouTubeQueryType.Channels:
                    result = await LoadChannelAsync(config.Query, parser);
                    break;
                case YouTubeQueryType.Videos:
                    result = await SearchAsync(config.Query, parser);
                    break;
                case YouTubeQueryType.Playlist:
                default:
                    result = await LoadPlaylistAsync(config.Query, parser);
                    break;
            }

            return result;
        }

        private void Assertions(YouTubeDataConfig config, IParser<YouTubeSchema> parser)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
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

        private async Task<IEnumerable<YouTubeSchema>> LoadChannelAsync(string channel, IParser<YouTubeSchema> parser)
        {
            IEnumerable<YouTubeSchema> result = new ObservableCollection<YouTubeSchema>();
            var listId = await GetUploadVideosListId(channel);
            if (!string.IsNullOrEmpty(listId))
            {
                result = await LoadPlaylistAsync(listId, parser);
            }
            return result;
        }

        private async Task<IEnumerable<YouTubeSchema>> SearchAsync(string query, IParser<YouTubeSchema> parser)
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = GetSearchUrl(query)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }
            
            if (result.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private async Task<string> GetUploadVideosListId(string channel)
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = GetChannelUrl(channel)
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
            }
            return string.Empty;
        }

        private async Task<IEnumerable<YouTubeSchema>> LoadPlaylistAsync(string playlistId, IParser<YouTubeSchema> parser)
        {
            HttpRequestSettings settings = new HttpRequestSettings
            {
                RequestedUri = GetPlaylistUrl(playlistId)
            };

            var requestResult = await HttpRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                return parser.Parse(requestResult.Result);
            }
            
            if (requestResult.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException();
        }

        private Uri GetChannelUrl(string channel)
        {
            return new Uri(string.Format("{0}/channels?forUsername={1}&part=contentDetails&maxResults=1&key={2}", BaseUrl, channel, _tokens.ApiKey), UriKind.Absolute);
        }

        private Uri GetPlaylistUrl(string playlistId)
        {
            return new Uri(string.Format("{0}/playlistItems?playlistId={1}&part=snippet&maxResults=20&key={2}", BaseUrl, playlistId, _tokens.ApiKey), UriKind.Absolute);
        }

        private Uri GetSearchUrl(string query)
        {
            return new Uri(string.Format("{0}/search?q={1}&part=snippet&maxResults=20&key={2}&type=video", BaseUrl, query, _tokens.ApiKey), UriKind.Absolute);
        }
    }
}
