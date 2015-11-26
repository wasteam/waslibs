using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataProvider : DataProviderBase<WordPressDataConfig, WordPressSchema>
    {
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(WordPressDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var wordPressUrlRequest = string.Empty;
            switch (config.QueryType)
            {
                case WordPressQueryType.Tag:
                    wordPressUrlRequest = $"https://public-api.wordpress.com/rest/v1.1/sites/{config.Query}/posts/?tag={config.FilterBy}";
                    break;
                case WordPressQueryType.Category:
                    wordPressUrlRequest = $"https://public-api.wordpress.com/rest/v1.1/sites/{config.Query}/posts/?category={config.FilterBy}";
                    break;
                default:
                    wordPressUrlRequest = $"https://public-api.wordpress.com/rest/v1.1/sites/{config.Query}/posts/";
                    break;

            }

            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(wordPressUrlRequest)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        public override IParser<WordPressSchema> GetDefaultParser(WordPressDataConfig config)
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
    }
}
