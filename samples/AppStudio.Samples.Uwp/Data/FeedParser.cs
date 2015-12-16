using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace AppStudio.Samples.Uwp
{
    static class FeedParser
    {
        static private readonly XNamespace nmspc = "http://www.w3.org/2005/Atom";
        static private readonly XNamespace nmspcm = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        static private readonly XNamespace nmspcd = "http://schemas.microsoft.com/ado/2007/08/dataservices";

        static public IEnumerable<FeedSchema> Parse(string xml)
        {
            var xdoc = XDocument.Parse(xml);

            foreach (var entry in xdoc.Descendants(nmspc + "entry"))
            {
                var properties = entry.Descendants(nmspcm + "properties").Elements();
                yield return new FeedSchema
                {
                    Id = properties.FirstOrDefault(r => r.Name.LocalName == "ID").Value,
                    Title = properties.FirstOrDefault(r => r.Name.LocalName == "Title").Value,
                    MediaUrl = properties.FirstOrDefault(r => r.Name.LocalName == "MediaUrl").Value,
                };
            }
        }
    }
}
