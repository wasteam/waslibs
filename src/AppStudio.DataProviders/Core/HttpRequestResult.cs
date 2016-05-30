using System.Collections.Generic;
using Windows.Web.Http;

namespace AppStudio.DataProviders.Core
{
    public class HttpRequestResult
    {
        public HttpRequestResult()
        {
            this.StatusCode = HttpStatusCode.Ok;
            this.Result = string.Empty;
        }
        internal HttpStatusCode StatusCode { get; set; }

        public string Result { get; set; }

        public bool Success { get { return (this.StatusCode == HttpStatusCode.Ok && !string.IsNullOrEmpty(this.Result)); } }
    }

    public class HttpRequestResult<TSchema> : HttpRequestResult where TSchema : SchemaBase
    {
        internal HttpRequestResult(HttpRequestResult result)
        {
            StatusCode = result.StatusCode;
            Result = result.Result;
        }

        public HttpRequestResult():base()
        {
           
        }

        public IEnumerable<TSchema> Items { get; set; } = new TSchema[0];

        public int HttpStatusCode
        {
            get
            {
                return (int)StatusCode;
            }
        }
    }
}
