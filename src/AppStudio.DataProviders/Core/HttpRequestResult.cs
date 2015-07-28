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
}
