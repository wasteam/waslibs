using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AppStudio.Uwp.Html;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class HtmlWriterFactory
    {
        private static List<HtmlWriter> _writers;

        public static HtmlWriter Find(HtmlFragment fragment)
        {
            if (_writers == null)
            {
                _writers = new List<HtmlWriter>();
                _writers.AddRange(ScanWriters());
            }
            return _writers.FirstOrDefault(w => w.Match(fragment)); ;
        }

        private static IEnumerable<HtmlWriter> ScanWriters()
        {
            return typeof(HtmlWriter)
                            .GetTypeInfo()
                            .Assembly.DefinedTypes
                                        .Where(t => t.BaseType == typeof(HtmlWriter))
                                        .Select(t => Activator.CreateInstance(t.AsType()))
                                        .Cast<HtmlWriter>();
        }
    }
}
