using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Windows.UI;

namespace AppStudio.Uwp.Samples
{
    class ColorsDataSource
    {
        public IEnumerable<object> GetItems()
        {
            return GetSystemColors().Select(r => r.Key).Cast<object>();
        }

        private Dictionary<string, Color> GetSystemColors()
        {
            var colors = typeof(Colors).GetRuntimeProperties().Select(c => new { Key = c.Name, Value = (Color)c.GetValue(null) });
            return colors.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
