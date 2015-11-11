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
        public override async Task<IEnumerable<WordPressSchema>> LoadDataAsync(WordPressDataConfig config)
        {
            return await LoadDataAsync(config, new WordPressParser());
        }

        public override async Task<IEnumerable<WordPressSchema>> LoadDataAsync(WordPressDataConfig config, IParser<WordPressSchema> parser)
        {
            Assertions(config, parser);

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

        private static void Assertions(WordPressDataConfig config, IParser<WordPressSchema> parser)
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
        }
    }
}
