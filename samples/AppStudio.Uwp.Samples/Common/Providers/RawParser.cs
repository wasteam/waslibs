using AppStudio.DataProviders;
using System.Collections.Generic;

namespace AppStudio.Uwp.Samples
{
    class RawParser : IParser<RawSchema>
    {
        public IEnumerable<RawSchema> Parse(string data)
        {
            var result = new RawSchema[] { new RawSchema { _id = "", Raw = data } };
            return result;
        }
    }
}
