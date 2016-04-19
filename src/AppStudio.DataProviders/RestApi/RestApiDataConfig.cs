using System;
using System.Net;

using Newtonsoft.Json.Linq;

namespace AppStudio.DataProviders.RestApi
{  
    public class RestApiDataConfig
    {
        public Uri Url { get; set; }

        public IPager Pager { get; set; }       

        public string PageSizeParameterName { get; set; }
      
    }

    public interface IPager
    {
        string ContinuationTokenInitialValue { get; set; }

        string PaginationParameterName { get; set; }

        string GetContinuationToken(string data, string currentContinuationToken);

        Uri GetContinuationUrl(Uri dataProviderUrl, string currentContinuationToken);
    }

    public class NumericPager : IPager
    {
        public string ContinuationTokenInitialValue { get; set; } = "1";

        public int IncrementalValue { get; set; } = 1;

        public string PaginationParameterName { get; set; }        

        public string GetContinuationToken(string data, string currentContinuationToken)
        {
            var result = (Convert.ToInt32(currentContinuationToken) + IncrementalValue).ToString();
            return result;
        }

        public Uri GetContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            if (dataProviderUrl == null)
            {
                throw new ArgumentNullException(nameof(dataProviderUrl));
            } 
                   
            var url = dataProviderUrl.AbsoluteUri;
            if (string.IsNullOrEmpty(dataProviderUrl.Query))
            {
                url += $"?{PaginationParameterName}={currentContinuationToken}";
            }
            else
            {
                url += $"&{PaginationParameterName}={currentContinuationToken}";
            }
            return new Uri(url);
        }
    }

    public class TokenPager : IPager
    {
        public string ContinuationTokenInitialValue { get; set; }

        public string PaginationParameterName { get; set; }

        public bool ContinuationTokenIsUrl { get; set; }

        public string ContinuationTokenPath { get; set; }

        public string GetContinuationToken(string data, string currentContinuationToken)
        {
            var result = string.Empty;
            JObject jsonObj = JObject.Parse(data);
            var token = jsonObj.SelectToken(ContinuationTokenPath);
            if (token != null)
            {
                result = token.ToString();
            }
            return result;
        }

        public Uri GetContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            if (dataProviderUrl == null)
            {
                throw new ArgumentNullException(nameof(dataProviderUrl));
            }

            if (ContinuationTokenIsUrl)
            {
                return new Uri(currentContinuationToken);
            }
            else
            {               
                var url = dataProviderUrl.AbsoluteUri;
                if (string.IsNullOrEmpty(dataProviderUrl.Query))
                {
                    url += $"?{PaginationParameterName}={WebUtility.UrlEncode(currentContinuationToken)}";
                }
                else
                {
                    url += $"&{PaginationParameterName}={WebUtility.UrlEncode(currentContinuationToken)}";
                }
                return new Uri(url);
            }
        }
    }
}
