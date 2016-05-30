using System;
using System.Net;

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiDataConfig
    {
        public Uri Url { get; set; }

        public IPagination PaginationConfig { get; set; } = new MemoryPagination();

        public IDictionary<string, string> Headers { get; set; }
    }

    public interface IPagination
    {
        bool IsServerSidePagination { get; }

        string PageSizeParameterName { get; }

        int TokenInitialValue { get; }

        string GetContinuationToken(object data, object currentToken);

        Uri BuildContinuationUrl(Uri dataProviderUrl, string currentContinuationToken);
    }

    public interface IQueryStringSorting
    {
        string OrderByParameterName { get; set; }

        string OrderByParameterValue { get; set; }

        string OrderDirectionParameterName { get; set; }

        string OrderDirectionParameterValue { get; set; }
    }

    public interface IMemorySorting
    {
        string OrderBy { get; set; }

        SortDirection OrderDirection { get; set; }
    }

    public class PageNumberPagination : IPagination, IQueryStringSorting
    {
        public bool IsPageNumberZeroIndexed { get; }

        public string PageSizeParameterName { get; }

        public string PageParameterName { get; }

        public bool IsServerSidePagination { get; } = true;

        public int TokenInitialValue { get { return IsPageNumberZeroIndexed ? 0 : 1; } }

        public string OrderByParameterName { get; set; }

        public string OrderByParameterValue { get; set; }

        public string OrderDirectionParameterName { get; set; }

        public string OrderDirectionParameterValue { get; set; }

        public PageNumberPagination(string pageParameterName, bool isPageNumberZeroIndexed, string pageSizeParameterName = null)
        {
            PageParameterName = pageParameterName;
            IsPageNumberZeroIndexed = isPageNumberZeroIndexed;
            PageSizeParameterName = pageSizeParameterName;
        }

        public string GetContinuationToken(object data, object currentToken)
        {
            return GetNextPageNumber(Convert.ToInt32(currentToken)).ToString();
        }

        public Uri BuildContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            int pageNumber;
            if (int.TryParse(currentContinuationToken, out pageNumber))
            {
                return GetPageUrl(dataProviderUrl, pageNumber);
            }
            return GetPageUrl(dataProviderUrl, TokenInitialValue);
        }


        private static int GetNextPageNumber(int currentPage)
        {
            return currentPage + 1;
        }

        private Uri GetPageUrl(Uri dataProviderUrl, int pageNumber)
        {
            if (dataProviderUrl == null)
            {
                throw new ArgumentNullException(nameof(dataProviderUrl));
            }

            var url = dataProviderUrl.AbsoluteUri;
            if (string.IsNullOrEmpty(dataProviderUrl.Query))
            {
                url += $"?{PageParameterName}={pageNumber}";
            }
            else
            {
                url += $"&{PageParameterName}={pageNumber}";
            }
            return new Uri(url);
        }
    }

    public class ItemOffsetPagination : IPagination, IQueryStringSorting
    {
        public bool IsServerSidePagination { get; } = true;

        public string PageSizeParameterName { get; }

        public string OffsetParameterName { get; }

        public int TokenInitialValue { get { return IsOffsetZeroIndexed ? 0 : 1; } }

        public int PageSize { get; private set; }

        public bool IsOffsetZeroIndexed { get; set; }

        public string OrderByParameterName { get; set; }

        public string OrderByParameterValue { get; set; }

        public string OrderDirectionParameterName { get; set; }

        public string OrderDirectionParameterValue { get; set; }

        public ItemOffsetPagination(string offsetParameterName, bool isOffsetZeroIndexed, string pageSizeParameterName, int pageSize)
        {
            OffsetParameterName = offsetParameterName;
            IsOffsetZeroIndexed = isOffsetZeroIndexed;
            PageSizeParameterName = pageSizeParameterName;
            PageSize = pageSize;
        }

        public string GetContinuationToken(object data, object currentToken)
        {
            return GetNextOffsetValue(Convert.ToInt32(currentToken)).ToString();
        }

        public Uri BuildContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            int offset;
            if (int.TryParse(currentContinuationToken, out offset))
            {
                return GetPageUrl(dataProviderUrl, offset);
            }
            return GetPageUrl(dataProviderUrl, TokenInitialValue);
        }

        private int GetNextOffsetValue(int currentOffset)
        {
            var result = currentOffset + PageSize;
            return result;
        }

        private Uri GetPageUrl(Uri dataProviderUrl, int offset)
        {
            if (dataProviderUrl == null)
            {
                throw new ArgumentNullException(nameof(dataProviderUrl));
            }

            var url = dataProviderUrl.AbsoluteUri;
            if (string.IsNullOrEmpty(dataProviderUrl.Query))
            {
                url += $"?{OffsetParameterName}={offset}";
            }
            else
            {
                url += $"&{OffsetParameterName}={offset}";
            }
            return new Uri(url);
        }


    }

    public class NextPageUrlPagination : IPagination, IQueryStringSorting
    {
        public bool IsServerSidePagination { get; } = true;

        public string PageSizeParameterName { get; }

        public int TokenInitialValue { get { return 0; } }

        public string NextPagePath { get; }

        public string OrderByParameterName { get; set; }

        public string OrderByParameterValue { get; set; }

        public string OrderDirectionParameterName { get; set; }

        public string OrderDirectionParameterValue { get; set; }

        public NextPageUrlPagination(string nextPageResponsePath, string pageSizeParameterName)
        {
            PageSizeParameterName = pageSizeParameterName;
            NextPagePath = nextPageResponsePath;
        }

        public string GetContinuationToken(object data, object currentToken)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return GetNextUrlValue(data.ToString());
        }

        public Uri BuildContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            return GetNextPageUrl(currentContinuationToken);
        }

        private static Uri GetNextPageUrl(string nextUrl)
        {
            if (string.IsNullOrEmpty(nextUrl))
            {
                throw new ArgumentNullException(nameof(nextUrl));
            }
            return new Uri(nextUrl);
        }

        private string GetNextUrlValue(string data)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(NextPagePath))
            {
                return null;
            }
            var result = string.Empty;
            JObject jsonObj = JObject.Parse(data);
            var token = jsonObj.SelectToken(NextPagePath);
            if (token != null)
            {
                result = token.ToString();
            }
            return result;
        }


    }

    public class TokenPagination : IPagination, IQueryStringSorting
    {
        public bool IsServerSidePagination { get; } = true;

        public string PageSizeParameterName { get; }

        public int TokenInitialValue { get { return 0; } }

        public string TokenParameterName { get; }

        public string TokenPath { get; }

        public string OrderByParameterName { get; set; }

        public string OrderByParameterValue { get; set; }

        public string OrderDirectionParameterName { get; set; }

        public string OrderDirectionParameterValue { get; set; }

        public TokenPagination(string tokenParameterName, string tokenResponsePath, string pageSizeParameterName)
        {
            PageSizeParameterName = pageSizeParameterName;
            TokenParameterName = tokenParameterName;
            TokenPath = tokenResponsePath;
        }

        public Uri BuildContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            if (dataProviderUrl == null)
            {
                throw new ArgumentNullException(nameof(dataProviderUrl));
            }

            var url = dataProviderUrl.AbsoluteUri;
            if (string.IsNullOrEmpty(dataProviderUrl.Query))
            {
                url += $"?{TokenParameterName}={WebUtility.UrlEncode(currentContinuationToken)}";
            }
            else
            {
                url += $"&{TokenParameterName}={WebUtility.UrlEncode(currentContinuationToken)}";
            }
            return new Uri(url);
        }

        public string GetContinuationToken(object data, object currentToken)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return GetContinuationToken(data.ToString());
        }

        private string GetContinuationToken(string data)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(TokenPath))
            {
                return null;
            }
            var result = string.Empty;
            JObject jsonObj = JObject.Parse(data);
            var token = jsonObj.SelectToken(TokenPath);
            if (token != null)
            {
                result = token.ToString();
            }
            return result;
        }
    }

    public class MemoryPagination : IPagination, IMemorySorting, IQueryStringSorting
    {
        public bool IsServerSidePagination { get; } = false;

        public string PageSizeParameterName { get; }

        public int TokenInitialValue { get { return 1; } }

        public string OrderByParameterName { get; set; }

        public string OrderByParameterValue { get; set; }

        public string OrderDirectionParameterName { get; set; }

        public string OrderDirectionParameterValue { get; set; }

        public string OrderBy { get; set; }

        public SortDirection OrderDirection { get; set; }  


        public Uri BuildContinuationUrl(Uri dataProviderUrl, string currentContinuationToken)
        {
            return dataProviderUrl;
        }

        public string GetContinuationToken(object data, object currentToken)
        {
            return GetNextPageNumber(Convert.ToInt32(currentToken)).ToString();
        }

        private static int GetNextPageNumber(int currentPage)
        {
            return currentPage + 1;
        }
    }
}
