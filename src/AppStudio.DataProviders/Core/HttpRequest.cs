using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace AppStudio.DataProviders.Core
{
    internal static class HttpRequest
    {
        internal static async Task<HttpRequestResult<TSchema>> ExecuteGetAsync<TSchema>(Uri uri, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = uri
            };

            HttpRequestResult httpResult = await DownloadAsync(settings);           
            HttpRequestResult<TSchema> result;
            result = new HttpRequestResult<TSchema>(httpResult);
            if (httpResult.Success)
            {
                var items = parser.Parse(httpResult.Result);
                result.Items = items;
            }

            return result;
        }

        internal static async Task<HttpRequestResult> DownloadAsync(HttpRequestSettings settings)
        {
            var result = new HttpRequestResult();

            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            var httpClient = new HttpClient(filter);

            AddRequestHeaders(httpClient, settings);

            HttpResponseMessage response = await httpClient.GetAsync(settings.RequestedUri);
            result.StatusCode = response.StatusCode;
            FixInvalidCharset(response);
            result.Result = await response.Content.ReadAsStringAsync();

            return result;
        }

        private static void AddRequestHeaders(HttpClient httpClient, HttpRequestSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.UserAgent))
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(settings.UserAgent);
            }

            if (settings.Headers != null)
            {
                foreach (var customHeaderName in settings.Headers.AllKeys)
                {
                    if (!String.IsNullOrEmpty(settings.Headers[customHeaderName]))
                    {
                        httpClient.DefaultRequestHeaders.Add(customHeaderName, settings.Headers[customHeaderName]);
                    }
                }
            }
        }

        private static void FixInvalidCharset(HttpResponseMessage response)
        {
            if (response != null && response.Content != null && response.Content.Headers != null
                && response.Content.Headers.ContentType != null && response.Content.Headers.ContentType.CharSet != null)
            {
                // Fix invalid charset returned by some web sites.
                string charset = response.Content.Headers.ContentType.CharSet;
                if (charset.Contains("\""))
                {
                    response.Content.Headers.ContentType.CharSet = charset.Replace("\"", string.Empty);
                }
            }
        }
    }
}
