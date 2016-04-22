using System;
using System.Net;

using Newtonsoft.Json.Linq;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataConfig
    {
        public Uri Url { get; set; }

        public IPaginationConfig PaginationConfig { get; set; } = new MemoryPagination();
    }

    public interface IPaginationConfig
    {
        bool IsInMemory { get; }

        string ContinuationTokenInitialValue { get; set; }

        string PageSizeParameterName { get; set; }

        string GetContinuationToken(string data, string currentContinuationToken);

        Uri GetContinuationUrl(Uri dataProviderUrl, string currentContinuationToken);
    }

    public class NumericPagination : IPaginationConfig
    {
        public bool IsInMemory { get; } = false;

        public string ContinuationTokenInitialValue { get; set; } = "1";

        public string PageSizeParameterName { get; set; } = string.Empty;

        public string PaginationParameterName { get; set; } = string.Empty;

        public int IncrementalValue { get; set; } = 1;

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

    public class TokenPagination : IPaginationConfig
    {
        public bool IsInMemory { get; } = false;

        public string ContinuationTokenInitialValue { get; set; }

        public string PageSizeParameterName { get; set; }

        public string PaginationParameterName { get; set; }

        public bool ContinuationTokenIsUrl { get; set; }

        public string ContinuationTokenPath { get; set; }

        public string GetContinuationToken(string data, string currentContinuationToken)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty( ContinuationTokenPath))
            {
                return null;
            }
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

    public class MemoryPagination : IPaginationConfig
    {
        public bool IsInMemory { get; } = true;

        public string PageSizeParameterName { get; set; } = string.Empty;

        public string ContinuationTokenInitialValue { get; set; } = "1";

        public string GetContinuationToken(string data, string currentContinuationToken)
        {
            var token = (Convert.ToInt32(currentContinuationToken) + 1).ToString();
            return token;
        }

        public Uri GetContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            return dataProviderUrl;
        }
    }
}
