using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Controls
{
    internal static class Extensions
    {
        public static bool IsNumeric(this string num)
        {
            if (string.IsNullOrWhiteSpace(num))
            {
                return false;
            }
            int i;
            return int.TryParse(num.Trim(), out i);
        }
    }
}
