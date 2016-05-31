using System;
using Windows.Web.Http;

namespace AppStudio.DataProviders.Exceptions
{
    public class RequestFailedException : Exception
    {
        public int StatusCode { get; protected set; }

        public string ResponseBody { get; protected set; }

        public RequestFailedException()
        {
        }

        public RequestFailedException(string message)
            : base(message)
        {
        }

        [CLSCompliant(false)]
        public RequestFailedException(HttpStatusCode statusCode, string reason)
            : base($"Request failed with status code {(int)statusCode} and reason '{reason}'")
        {
            StatusCode = (int)statusCode;
            ResponseBody = reason;
        }

        public RequestFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
