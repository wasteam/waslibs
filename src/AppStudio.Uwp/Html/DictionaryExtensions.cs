using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Html
{
    static class DictionaryExtensions
    {
        public static string GetValue(this Dictionary<string, string> dict, string attrName)
        {
            if (dict.ContainsKey(attrName))
            {
                return dict[attrName];
            }
            return null;
        }

        public static int GetValueInt(this Dictionary<string, string> dict, string attrName)
        {
            int result;
            var value = dict.GetValue(attrName);
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (int.TryParse(value, out result))
                {
                    return result;
                }
            }
            return 0;
        }
    }
}
