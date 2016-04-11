using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataProvider : DataProviderBase<WordPressDataConfig, WordPressSchema>
    {
        private const string BaseUrl = "https://public-api.wordpress.com/rest/v1.1";
        private string _commentsContinuationToken = "1";

        bool _hasMoreItems;
        public override bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        bool _hasMoreComments;
        public bool HasMoreComments
        {
            get
            {
                return _hasMoreComments;
            }
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(WordPressDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            ContinuationToken = "1";
            return await GetDataFromProvider(config, pageSize, parser);

        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(WordPressDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            return await GetDataFromProvider(config, pageSize, parser);
        }

        public async Task<IEnumerable<WordPressCommentSchema>> GetComments(string site, string postId, int pageSize)
        {
            return await GetComments(site, postId, pageSize, new WordPressCommentParser());
        }

        public async Task<IEnumerable<TSchema>> GetComments<TSchema>(string site, string postId, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            _commentsContinuationToken = "1";
            return await GetCommentsFromProvider(site, postId, pageSize, parser);
        }

        public async Task<IEnumerable<WordPressCommentSchema>> GetMoreComments(string site, string postId, int pageSize)
        {
            return await GetMoreComments(site, postId, pageSize, new WordPressCommentParser());
        }

        public async Task<IEnumerable<TSchema>> GetMoreComments<TSchema>(string site, string postId, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (HasMoreComments)
            {
                return await GetCommentsFromProvider(site, postId, pageSize, parser);
            }
            return new TSchema[0];            
        }

        protected override IParser<WordPressSchema> GetDefaultParserInternal(WordPressDataConfig config)
        {
            return new WordPressParser();
        }

        protected override void ValidateConfig(WordPressDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }

        private static string GetContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }


        private async Task<IEnumerable<TSchema>> GetDataFromProvider<TSchema>(WordPressDataConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var wordPressUrlRequest = string.Empty;
            switch (config.QueryType)
            {
                case WordPressQueryType.Tag:
                    wordPressUrlRequest = $"{BaseUrl}/sites/{config.Query}/posts/?tag={config.FilterBy}&number={pageSize}&page={ContinuationToken}";
                    break;
                case WordPressQueryType.Category:
                    wordPressUrlRequest = $"{BaseUrl}/sites/{config.Query}/posts/?category={config.FilterBy}&number={pageSize}&page={ContinuationToken}";
                    break;
                default:
                    wordPressUrlRequest = $"{BaseUrl}/sites/{config.Query}/posts/?number={pageSize}&page={ContinuationToken}";
                    break;
            }

            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(wordPressUrlRequest)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var items = parser.Parse(result.Result);
                _hasMoreItems = items.Any();
                ContinuationToken = GetContinuationToken(ContinuationToken);
                return items;
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private async Task<IEnumerable<TSchema>> GetCommentsFromProvider<TSchema>(string site, string postId, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var wordPressUrlRequest = $"{BaseUrl}/sites/{site}/posts/{postId}/replies?number={pageSize}&page={_commentsContinuationToken}";

            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(wordPressUrlRequest)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var comments = parser.Parse(result.Result);
                if (comments != null)
                {
                    _hasMoreComments = comments.Any();
                    _commentsContinuationToken = GetContinuationToken(_commentsContinuationToken);
                    return comments.ToList();
                }
                else
                {
                    _hasMoreComments = false;
                    return new TSchema[0];
                }
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }
    }
}
