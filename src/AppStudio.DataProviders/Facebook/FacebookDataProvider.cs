using System;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppStudio.DataProviders.Facebook
{
    public class FacebookDataProvider : DataProviderBase<FacebookDataConfig, FacebookSchema>
    {
        private const string BaseUrl = @"https://graph.facebook.com/v2.5";
        private string _continuationToken;

        public override bool HasMoreItems
        {
            get
            {
                return !string.IsNullOrEmpty(_continuationToken);
            }
        }

        private FacebookOAuthTokens _tokens;

        //public string Json { get; set; }

        public FacebookDataProvider(FacebookOAuthTokens tokens)
        {
            _tokens = tokens;
        }
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FacebookDataConfig config, int maxRecords, IPaginationParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = GetUri(config, maxRecords)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
           
            if (result.Success)
            {
                var r = parser.Parse(result.Result);
                _continuationToken = r.ContinuationToken;
                return r.GetItems();
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new OAuthKeysRevokedException($"Request failed with status code {(int)HttpStatusCode.BadRequest} and reason '{result.Result}'");
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }


        private FacebookGraphResponse InternalParse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return default(FacebookGraphResponse);
            }
            FacebookGraphResponse response = JsonConvert.DeserializeObject<FacebookGraphResponse>(data);
            return response;
        }

        private Uri GetUri(FacebookDataConfig config, int maxRecords)
        {
            if (HasMoreItems)
            {
                return new Uri(_continuationToken);
            }
            else
            {
                return new Uri($"{BaseUrl}/{config.UserId}/posts?&access_token={_tokens.AppId}|{ _tokens.AppSecret}&fields=id,message,from,created_time,link,full_picture&limit={maxRecords}", UriKind.Absolute);
            }
        }


        protected override IPaginationParser<FacebookSchema> GetDefaultParserInternal(FacebookDataConfig config)
        {
            return new FacebookParser();
        }

        protected override void ValidateConfig(FacebookDataConfig config)
        {
            if (config.UserId == null)
            {
                throw new ConfigParameterNullException("UserId");
            }
            if (_tokens == null)
            {
                throw new ConfigParameterNullException("Tokens");
            }
            if (string.IsNullOrEmpty(_tokens.AppId))
            {
                throw new OAuthKeysNotPresentException("AppId");
            }
            if (string.IsNullOrEmpty(_tokens.AppSecret))
            {
                throw new OAuthKeysNotPresentException("AppSecret");
            }
        }


    }


    #region Poc
    //public class FacebookDataProvider : IncrementalDataProviderBase<FacebookDataConfig, GraphData, FacebookSchema>
    //{
    //    private const string BaseUrl = @"https://graph.facebook.com/v2.5";
    //    private string _nextPageUrl;
    //    public bool HasMoreElements
    //    {
    //        get
    //        {
    //            return !string.IsNullOrEmpty(_nextPageUrl);
    //        }
    //    }

    //    private FacebookOAuthTokens _tokens;
    //    public string Json { get; set; }

    //    public FacebookDataProvider(FacebookOAuthTokens tokens)
    //    {
    //        _tokens = tokens;
    //    }

    //    protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FacebookDataConfig config, int maxRecords, IParserIncremental<GraphData, TSchema> parser)
    //    {
    //        var settings = new HttpRequestSettings
    //        {
    //            RequestedUri = GetUri(config, maxRecords)
    //        };

    //        HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
    //        Json = result.Result;
    //        if (result.Success)
    //        {
    //            FacebookGraphResponse response = InternalParse(result.Result);
    //            _nextPageUrl = response?.paging?.next;
    //            Collection<TSchema> resultToReturn = new Collection<TSchema>();

    //            foreach (var item in response.data)
    //            {
    //                resultToReturn.Add(parser.Parse(item));
    //            }
    //            return resultToReturn;
    //        }

    //        if (result.StatusCode == HttpStatusCode.BadRequest)
    //        {
    //            throw new OAuthKeysRevokedException($"Request failed with status code {(int)HttpStatusCode.BadRequest} and reason '{result.Result}'");
    //        }

    //        throw new RequestFailedException(result.StatusCode, result.Result);
    //    }

    //    private FacebookGraphResponse InternalParse(string data)
    //    {
    //        if (string.IsNullOrEmpty(data))
    //        {
    //            return default(FacebookGraphResponse);
    //        }
    //        FacebookGraphResponse response = JsonConvert.DeserializeObject<FacebookGraphResponse>(data);
    //        return response;
    //    }

    //    private Uri GetUri(FacebookDataConfig config, int maxRecords)
    //    {
    //        if (HasMoreElements)
    //        {
    //            return new Uri(_nextPageUrl);
    //        }
    //        else
    //        {
    //            return new Uri($"{BaseUrl}/{config.UserId}/posts?&access_token={_tokens.AppId}|{ _tokens.AppSecret}&fields=id,message,from,created_time,link,full_picture&limit={maxRecords}", UriKind.Absolute);
    //        }
    //    }

    //    //public override T InternalParse<T>(string data)
    //    //{
    //    //    if (string.IsNullOrEmpty(data))
    //    //    {
    //    //        return default(T);
    //    //    }
    //    //    T response = JsonConvert.DeserializeObject<T>(data);
    //    //    return response;
    //    //}

    //    protected override IParserIncremental<GraphData, FacebookSchema> GetDefaultParserInternal(FacebookDataConfig config)
    //    {
    //        return new FacebookParser();
    //    }

    //    protected override void ValidateConfig(FacebookDataConfig config)
    //    {
    //        if (config.UserId == null)
    //        {
    //            throw new ConfigParameterNullException("UserId");
    //        }
    //        if (_tokens == null)
    //        {
    //            throw new ConfigParameterNullException("Tokens");
    //        }
    //        if (string.IsNullOrEmpty(_tokens.AppId))
    //        {
    //            throw new OAuthKeysNotPresentException("AppId");
    //        }
    //        if (string.IsNullOrEmpty(_tokens.AppSecret))
    //        {
    //            throw new OAuthKeysNotPresentException("AppSecret");
    //        }
    //    }

    //}

    #endregion
}
