using System.Linq;
using System.Reflection;

using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Samples.Extensions
{
    public static class PageExtensions
    {
        public static string GetResourceString(this Page self, string resource)
        {
            return Singleton<ResourceLoader>.Instance.GetString(resource);
        }

        public static bool IsSamplePageByCategory(this Page self, CustomAttributeData attr, string category)
        {
            return attr.NamedArguments.Any(arg => attr.AttributeType == typeof(SamplePageAttribute) && arg.MemberName == "Category" && arg.TypedValue.Value.ToString() == category);
        }       

        public static object GetResource(this string resourceName)
        {            
            if (!Application.Current.Resources.ContainsKey(resourceName))
            {
                return null;
            }
            return Application.Current.Resources[resourceName];
        }

        public static Brush GetCategoryBackground(this Page self, string categoryName)
        {
            return string.Format("{0}Background", categoryName).GetResource() as Brush;
        }

        public static string GetPageCategory(this SamplePage self)
        {
            var attr = self.GetType().GetTypeInfo().GetCustomAttribute(typeof(SamplePageAttribute)) as SamplePageAttribute;
            if (attr != null)
            {
                return attr.Category;
            }
            return string.Empty;
        }

        public static Brush GetCategoryBackground(this SamplePage self)
        {
            string category = self.GetPageCategory();
            return self.GetCategoryBackground(category);
        }
    }
}
