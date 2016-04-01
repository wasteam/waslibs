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
     
        IResponseBase<RawSchema> IPaginationParser<RawSchema>.Parse(string data)
        {
            var response = new RawRespose();
            response.Data = data;
            return response;
        }
    }

    class RawRespose : IResponseBase<RawSchema>
    {
        public string Data { get; set; }

        public string NextPageToken
        {
            get
            {
                return null;
            }           
        }

        IEnumerable<RawSchema> IResponseBase<RawSchema>.GetData()
        {
            var result = new RawSchema[] { new RawSchema { _id = "", Raw = Data } };
            return result;
        }
    }
}
