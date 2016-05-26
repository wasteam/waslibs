using System;
using System.Net;

namespace AppStudio.DataProviders.Core
{
    internal class HttpRequestSettings
    {
        public HttpRequestSettings()
        {
            this.Headers = new WebHeaderCollection();
        }

        public Uri RequestedUri { get; set; }

        private string _userAgent = $"Mozilla/5.0 (Windows NT 10.0;) {HttpRequest.GetWASSufix()}";
        public string UserAgent
        {
            get
            {
                return _userAgent;
            }
            set
            {
                _userAgent = value;
                if (!string.IsNullOrEmpty(_userAgent))
                {
                    _userAgent += $" {HttpRequest.GetWASSufix()}";
                }
            }
        }

        public WebHeaderCollection Headers { get; private set; }
    }
}
