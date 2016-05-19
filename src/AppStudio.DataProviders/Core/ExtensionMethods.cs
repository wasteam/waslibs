// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.DataProviders.Core
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AppStudio.DataProviders.Bing;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    public static class ExtensionMethods
    {
        private static Regex RemoveHtmlTagsRegex = new Regex(@"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>");

        public static string ToSafeString(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }


        public static string DecodeHtml(this string htmlText)
        {
            if (htmlText == null)
            {
                return null;
            }

            var ret = InternalExtensionMethods.FixHtml(htmlText);

            //Remove html tags
            ret = RemoveHtmlTagsRegex.Replace(ret, string.Empty);

            return WebUtility.HtmlDecode(ret);
        }

        public static string GetStringValue(this BingCountry value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetRuntimeField(value.ToString());
            StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }

    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    internal static class InternalExtensionMethods
    {
        private static Regex RemoveCommentsRegex = new Regex("<!--.*?-->", RegexOptions.Singleline);
        private static Regex RemoveScriptsRegex = new Regex(@"(?s)<script.*?(/>|</script>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex RemoveStylesRegex = new Regex(@"(?s)<style.*?(/>|</style>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="value">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this String value, int length)
        {
            return Truncate(value, length, false);
        }

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="value">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="ellipsis">if set to <c>true</c> add a text ellipsis.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this String value, int length, bool ellipsis)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                if (value.Length > length)
                {
                    if (ellipsis)
                    {
                        return value.Substring(0, length) + "...";
                    }
                    else
                    {
                        return value.Substring(0, length);
                    }
                }
            }
            return value ?? string.Empty;
        }

        public static string FixHtml(this string html)
        {
            // Remove comments
            var withoutComments = RemoveCommentsRegex.Replace(html, string.Empty);

            // Remove scripts
            var withoutScripts = RemoveScriptsRegex.Replace(withoutComments, string.Empty);

            // Remove styles
            var withoutStyles = RemoveStylesRegex.Replace(withoutScripts, string.Empty);

            return withoutStyles;
        }

        public static Dictionary<string, string> ParseQueryString(this Uri uri)
        {
            if (uri != null && !string.IsNullOrEmpty(uri.Query))
            {
                var result = uri.Query.Split(new char[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Split('=')).GroupBy(p => p[0]).
                   Select(p => p.First()).ToDictionary(p => p[0], p => p.Length > 1 ? Uri.UnescapeDataString(p[1]) : null);
                return result;
            }
            return new Dictionary<string, string>();
        }

        public static string ToQueryString(this Dictionary<string, string> dictionary)
        {
            if (dictionary.Count > 0)
            {
                var array = dictionary.Select(p => $"{Uri.UnescapeDataString(p.Key)}={Uri.UnescapeDataString(p.Value)}").ToArray();
                return $"?{string.Join("&", array)}";
            }
            return string.Empty;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, SortDirection sortDirection)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return (IOrderedQueryable<T>)source;
            }

            if (sortDirection == SortDirection.Ascending)
            {
                return ApplyOrder(source, propertyName, nameof(Queryable.OrderBy));
            }
            else
            {
                return ApplyOrder(source, propertyName, nameof(Queryable.OrderByDescending));
            }
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string propertyName, string methodName)
        {
            try
            {               
                Type type = typeof(T);
                ParameterExpression arg = Expression.Parameter(type, "x");
                Expression expr = arg;              
                PropertyInfo pInfo = type.GetRuntimeProperty(propertyName);
                expr = Expression.Property(expr, pInfo);
                type = pInfo.PropertyType;
              
                Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
                LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

                var result = typeof(Queryable).GetRuntimeMethods().Single(
                        method => method.Name == methodName
                                && method.IsGenericMethodDefinition
                                && method.GetGenericArguments().Length == 2
                                && method.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(T), type)
                            .Invoke(source, new object[] { source, lambda });

                return (IOrderedQueryable<T>)result;
            }
            catch (Exception)
            {
                return (IOrderedQueryable<T>)source;
            }
        }
    }
}
