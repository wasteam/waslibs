using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace AppStudio.DataProviders.RestApi

{
    public class RestApiDataConfig<TSchema> where TSchema : SchemaBase
    {
        public Uri Url { get; set; }

        public IPaginator Paginator { get; set; }

        public string ElementsRootPath { get; set; }

        public string PageSizeParameterName { get; set; }

        public Func<string, TSchema> ItemParser { get; set; }

        //public RestApiDataConfig(IPaginator paginator)
        //{
        //    if (paginator == null)
        //    {
        //        throw new ArgumentNullException(nameof(paginator));
        //    }
        //    Paginator = paginator;
        //}
    }

    public interface IPaginator
    {
        string ContinuationTokenInitialValue { get; set; }

        string PaginationParameterName { get; set; }

        string GetContinuationToken(string data, string currentContinuationToken);

        string GetContinuationUrl(string dataProviderUrl, string currentContinuationToken);
    }

    public class NumericPaginator : IPaginator
    {
        public string ContinuationTokenInitialValue { get; set; } = "1";

        public int IncrementalValue { get; set; } = 1;

        public string PaginationParameterName { get; set; }

        public string GetContinuationUrl(string dataProviderUrl, string currentContinuationToken)
        {
            var uri = new Uri(dataProviderUrl);
            if (string.IsNullOrEmpty(uri.Query))
            {
                dataProviderUrl += $"?{PaginationParameterName}={currentContinuationToken}";
            }
            else
            {
                dataProviderUrl += $"&{PaginationParameterName}={currentContinuationToken}";
            }
            return dataProviderUrl;
        }

        public string GetContinuationToken(string data, string currentContinuationToken)
        {
            var result = (Convert.ToInt32(currentContinuationToken) + IncrementalValue).ToString();
            return result;
        }
    }

    public class TokenPaginator : IPaginator
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

        public string GetContinuationUrl(string dataProviderUrl, string currentContinuationToken)
        {
            if (ContinuationTokenIsUrl)
            {
                return currentContinuationToken;
            }
            else
            {
                var uri = new Uri(dataProviderUrl);
                if (string.IsNullOrEmpty(uri.Query))
                {
                    dataProviderUrl += $"?{PaginationParameterName}={WebUtility.UrlEncode(currentContinuationToken)}";
                }
                else
                {
                    dataProviderUrl += $"&{PaginationParameterName}={WebUtility.UrlEncode(currentContinuationToken)}";
                }
                return dataProviderUrl;
            }
        }
    }
}
