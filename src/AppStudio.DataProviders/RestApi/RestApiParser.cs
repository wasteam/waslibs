using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppStudio.DataProviders.RestApi
{
    public class RestApiParser<TSchema> : IParser<TSchema> where TSchema : SchemaBase, new()
    {
        private string _rootPath = string.Empty;

        private Func<string, TSchema> _itemParser = (x) => DefaultItemParser(x);

        public RestApiParser(string rootPath, Func<string, TSchema> parserItems)
        {                   
            if (parserItems == null)
            {
                throw new ArgumentNullException(nameof(parserItems));
            }
            _rootPath = rootPath;
            _itemParser = parserItems;
        }

        IEnumerable<TSchema> IParser<TSchema>.Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = new Collection<TSchema>();
            JObject o = JObject.Parse(data);
            IEnumerable<string> elements = o.SelectToken(_rootPath).Select(s => s.ToString());

            foreach (string item in elements)
            {
                var itemResult = _itemParser(item);
                result.Add(itemResult);
            }
            return result;
        }

        private static TSchema DefaultItemParser(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<TSchema>(data);
            }
            catch (Exception)
            {
                return new TSchema();
            }      
            
        }
    }
}

