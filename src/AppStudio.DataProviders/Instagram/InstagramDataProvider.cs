using System;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramDataProvider : DataProviderBase<InstagramDataConfig, InstagramSchema>
    {
        private const string URL = "https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}";
        private const string URLUserID = "https://api.instagram.com/v1/users/{0}/media/recent/?client_id={1}";

        private InstagramOAuthTokens _tokens;

        public InstagramDataProvider(InstagramOAuthTokens tokens)
        {
            _tokens = tokens;
        }

        public override async Task<IEnumerable<InstagramSchema>> LoadDataAsync(InstagramDataConfig config)
        {
            return await LoadDataAsync(config, new InstagramParser());
        }

        public override async Task<IEnumerable<InstagramSchema>> LoadDataAsync(InstagramDataConfig config, IParser<InstagramSchema> parser)
        {
            Assertions(config, parser);

            var settings = new HttpRequestSettings
            {
                RequestedUri = this.GetApiUrl(config)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            if (result.StatusCode == HttpStatusCode.BadRequest && !string.IsNullOrEmpty(result.Result) && result.Result.Contains("OAuthParameterException"))
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        private Uri GetApiUrl(InstagramDataConfig config)
        {
            if (config.QueryType == InstagramQueryType.Tag)
            {
                return new Uri(string.Format(URL, config.Query, _tokens.ClientId));
            }
            else
            {
                return new Uri(string.Format(URLUserID, config.Query, _tokens.ClientId));
            }
        }

        private void Assertions(InstagramDataConfig config, IParser<InstagramSchema> parser)
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
            if (string.IsNullOrEmpty(_tokens.ClientId))
            {
                throw new OAuthKeysNotPresentException("ClientId");
            }
        }
    }
}
