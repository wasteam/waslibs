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
        private const string BaseUrl = @"https://graph.facebook.com/v2.2";

        private FacebookOAuthTokens _tokens;

        public FacebookDataProvider(FacebookOAuthTokens tokens)
        { 
            _tokens = tokens;
        }

        public override async Task<IEnumerable<FacebookSchema>> LoadDataAsync(FacebookDataConfig config)
        {
            return await LoadDataAsync(config, new FacebookParser());
        }

        public override async Task<IEnumerable<FacebookSchema>> LoadDataAsync(FacebookDataConfig config, IParser<FacebookSchema> parser)
        {
            Assertions(config, parser);

            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(string.Format("{0}/{1}/posts?&access_token={2}|{3}", BaseUrl, config.UserId, _tokens.AppId, _tokens.AppSecret), UriKind.Absolute)
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

        private void Assertions(FacebookDataConfig config, IParser<FacebookSchema> parser)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
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
