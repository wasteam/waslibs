using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples.Extensions
{
    public static class PageExtensions
    {
        public static string GetResourceString(this Page self, string resource)
        {
            return Singleton<ResourceLoader>.Instance.GetString(resource);
        }
    }
}
