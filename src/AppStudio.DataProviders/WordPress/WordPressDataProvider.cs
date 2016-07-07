using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataProvider : DataProviderBase<WordPressDataConfig, WordPressSchema>
    {
        private const string BaseUrl = "https://public-api.wordpress.com/rest/v1.1";
        private string _commentsContinuationToken = "1";

        private string _postId;
        private string _site;
        int _commentsPageSize;
        object _commentsParser;

        public override bool HasMoreItems
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ContinuationToken);
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
            var url = GetUrl(config, pageSize);
            return await GetDataFromProvider(url, parser);
        }

        protected override async Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(WordPressDataConfig config, int pageSize, IParser<TSchema> parser)
        {
            var url = GetUrl(config, pageSize);
            var continuationUrl = GetContinuationUrl(url, ContinuationToken);
            return await GetDataFromProvider(continuationUrl, parser);
        }

        public async Task<IEnumerable<WordPressCommentSchema>> GetComments(string site, string postId, int pageSize)
        {
            return await GetComments(site, postId, pageSize, new WordPressCommentParser());
        }

        public async Task<IEnumerable<TSchema>> GetComments<TSchema>(string site, string postId, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (parser == null)
            {
                throw new ParserNullException();
            }

            if (string.IsNullOrEmpty(site))
            {
                throw new ArgumentNullException(nameof(site));
            }

            if (string.IsNullOrEmpty(postId))
            {
                throw new ArgumentNullException(nameof(postId));
            }
            _commentsContinuationToken = "1";
            _commentsParser = parser;
            _commentsPageSize = pageSize;
            _site = site;
            _postId = postId;           
            return await GetCommentsFromProvider(site, postId, pageSize, parser);
        }

        public async Task<IEnumerable<WordPressCommentSchema>> GetMoreComments()
        {
            return await GetMoreComments<WordPressCommentSchema>();
        }

        public async Task<IEnumerable<TSchema>> GetMoreComments<TSchema>() where TSchema : SchemaBase
        {
            if (_commentsParser == null)
            {
                throw new InvalidOperationException("GetMoreComments can not be called. You must call the GetComments method prior to calling this method");
            }

            if (HasMoreComments)
            {
                var parser = _commentsParser as IParser<TSchema>;
                return await GetCommentsFromProvider(_site, _postId, _commentsPageSize, parser);
            }
            return new TSchema[0];
        }

        protected override IParser<WordPressSchema> GetDefaultParserInternal(WordPressDataConfig config)
        {
            return new WordPressParser();
        }

        protected override void ValidateConfig(WordPressDataConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }

        private static string GetUrl(WordPressDataConfig config, int pageSize)
        {
            var url = string.Empty;
            switch (config.QueryType)
            {
                case WordPressQueryType.Tag:
                    url = $"{BaseUrl}/sites/{config.Query}/posts/?tag={config.FilterBy}&number={pageSize}";
                    break;
                case WordPressQueryType.Category:
                    url = $"{BaseUrl}/sites/{config.Query}/posts/?category={config.FilterBy}&number={pageSize}";
                    break;
                default:
                    url = $"{BaseUrl}/sites/{config.Query}/posts/?number={pageSize}";
                    break;
            }
            if (config.OrderBy != WordPressOrderBy.None)
            {
                var order = config.OrderDirection == SortDirection.Ascending ? "ASC" : "DESC";
                url += $@"&order_by={config.OrderBy.GetStringValue()}&order={order}";
            }
            return url;
        }

        private static string GetContinuationUrl(string url, string currentContinuationToken)
        {
            url += $"&page_handle={WebUtility.UrlEncode(currentContinuationToken)}";
            return url;
        }

        private static string GetContinuationToken(string data)
        {
            var wordPressResponse = JsonConvert.DeserializeObject<WordPressResponse>(data);
            return wordPressResponse?.meta?.next_page;
        }

        private static string GetCommentsContinuationToken(string currentToken)
        {
            var token = (Convert.ToInt32(currentToken) + 1).ToString();
            return token;
        }

        private async Task<IEnumerable<TSchema>> GetDataFromProvider<TSchema>(string url, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(url)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                ContinuationToken = GetContinuationToken(result.Result);
                return await parser.ParseAsync(result.Result);
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
                var comments = await parser.ParseAsync(result.Result);
                if (comments != null)
                {
                    _hasMoreComments = comments.Count() >= pageSize;
                    _commentsContinuationToken = GetCommentsContinuationToken(_commentsContinuationToken);
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
