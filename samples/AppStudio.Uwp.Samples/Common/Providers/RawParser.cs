using AppStudio.DataProviders;
using System.Collections.Generic;
using System;
using AppStudio.DataProviders.Facebook;

namespace AppStudio.Uwp.Samples
{
    class RawParser : IParser<RawSchema>, IPaginationParser<RawSchema>
    {
        IEnumerable<RawSchema> IParser<RawSchema>.Parse(string data)
        {
            var result = new RawSchema[] { new RawSchema { _id = "", Raw = data } };
            return result;
        }
     
        IParserResponse<RawSchema> IPaginationParser<RawSchema>.Parse(string data)
        {
            var response = new RawRespose();
            response.Data = data;
            return response;
        }
    }

    class RawRespose : IParserResponse<RawSchema>
    {
        public string Data { get; set; }

        public string ContinuationToken
        {
            get
            {
                return null;
            }           
        }

        IEnumerable<RawSchema> IParserResponse<RawSchema>.GetItems()
        {
            var result = new RawSchema[] { new RawSchema { _id = "", Raw = Data } };
            return result;
        }
    }
}
