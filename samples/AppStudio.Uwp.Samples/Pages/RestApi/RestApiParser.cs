using AppStudio.DataProviders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Samples
{
    public class RestApiParser : IParser<RestApiSchema>
    {
        private string _rootPath = string.Empty;

        public RestApiParser(string rootPath)
        {           
            _rootPath = rootPath;           
        }

        public IEnumerable<RestApiSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = new Collection<RestApiSchema>();
            JObject o = JObject.Parse(data);
            IEnumerable<JToken> elements = o.SelectToken(_rootPath)?.Select(s => s);
            if (elements != null)
            {
                foreach (JToken item in elements)
                {
                    var itemResult = new RestApiSchema();
                    itemResult._id = item.SelectToken("").ToString();
                    itemResult.Title = (string)item.SelectToken("");
                    DateTime date;
                    DateTime.TryParse((string)item.SelectToken(""), out date);
                    //itemResult.DataTime = ((string)item.SelectToken("")).SafeDateTime();
                    result.Add(itemResult);
                }
            }
            return result;
        }

        //public static DateTime? SafeDateTime(this string s)
        //{
        //    DateTime date;
        //    if (DateTime.TryParse(s, out date))
        //    {
        //        return date;
        //    }
        //    return null;
        //}
    }
}
