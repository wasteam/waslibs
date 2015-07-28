using System;
using Windows.Web.Http;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;

namespace AppStudio.DataProviders.Core
{
    internal static class HttpRequest
    {
        internal static async Task<HttpRequestResult> DownloadAsync(HttpRequestSettings settings)
        {
            var result = new HttpRequestResult();

            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            var httpClient = new HttpClient(filter);

            if (!string.IsNullOrEmpty(settings.UserAgent))
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(settings.UserAgent);
            }

            HttpResponseMessage response = await httpClient.GetAsync(settings.RequestedUri);
            result.StatusCode = response.StatusCode;
            FixInvalidCharset(response);
            result.Result = await response.Content.ReadAsStringAsync();

            return result;
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
