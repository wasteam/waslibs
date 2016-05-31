using AppStudio.DataProviders;
using System.Collections.Generic;
using System;
using AppStudio.DataProviders.Facebook;

namespace AppStudio.Uwp.Samples
{
    class RawParser : IParser<RawSchema>
    {
        IEnumerable<RawSchema> IParser<RawSchema>.Parse(string data)
        {
            var result = new RawSchema[] { new RawSchema { _id = "", Raw = data } };
            return result;
        }
    }
}
