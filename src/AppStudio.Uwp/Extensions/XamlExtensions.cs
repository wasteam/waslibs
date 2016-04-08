using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppStudio.Uwp
{
    public static class XamlExtensions
    {
        public static object Resource(this string self)
        {
            if (Application.Current.Resources.ContainsKey(self))
            {
                return Application.Current.Resources[self];
            }
            else
            {
                return null;
            }
        }

        public static T Resource<T>(this string self) where T : class
        {
            return self.Resource() as T;
        }
    }
}
