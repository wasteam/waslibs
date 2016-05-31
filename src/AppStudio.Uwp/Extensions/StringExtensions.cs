using Windows.ApplicationModel.Resources;

namespace AppStudio.Uwp
{
    public static class StringExtensions
    {
        public static string StringResource(this string self)
        {
            return Singleton<ResourceLoader>.Instance.GetString(self);
        }
    }
}
