using System;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Facebook
{
    public class FacebookDataProvider : DataProviderBase<FacebookDataConfig, FacebookSchema>
    {
        private const string BaseUrl = @"https://graph.facebook.com/v2.4";

        private FacebookOAuthTokens _tokens;

        public FacebookDataProvider(FacebookOAuthTokens tokens)
        { 
            _tokens = tokens;
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FacebookDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(string.Format("{0}/{1}/posts?&access_token={2}|{3}&fields=id,message,from,created_time,link,full_picture", BaseUrl, config.UserId, _tokens.AppId, _tokens.AppSecret), UriKind.Absolute)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        public override IParser<FacebookSchema> GetDefaultParser(FacebookDataConfig config)
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
}
