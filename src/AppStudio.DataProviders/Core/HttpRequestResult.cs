using System.Collections.Generic;
using Windows.Web.Http;

namespace AppStudio.DataProviders.Core
{
    internal class HttpRequestResult
    {
        public HttpRequestResult()
        {
            this.StatusCode = HttpStatusCode.Ok;
            this.Result = string.Empty;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Result { get; set; }

        public bool Success { get { return (this.StatusCode == HttpStatusCode.Ok && !string.IsNullOrEmpty(this.Result)); } }
    }

    public class HttpRequestResult<TSchema> where TSchema : SchemaBase
    {
        internal HttpRequestResult(HttpRequestResult result)
        {
            StatusCode = (int)result.StatusCode;
            Content = result.Result;
        }

        public int StatusCode { get; set; }

        public string Content { get; set; }

        public IEnumerable<TSchema> Items { get; set; }

        public bool Success { get { return (this.StatusCode == 200 && !string.IsNullOrEmpty(this.Content)); } }
    }
}
