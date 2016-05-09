using System;

namespace AppStudio.Uwp.Navigation
{
    public class NavigationInfo
    {
        public string TargetPage { get; set; }
        
        public Uri TargetUri { get; set; }

        public NavigationType NavigationType { get; set; }

        public bool IncludeState { get; set; }
        
        public static NavigationInfo FromPage(string pageFullType)
        {
            return FromPage(pageFullType, false);
        }

        public static NavigationInfo FromPage(string pageFullType, bool includeState)
        {
            return new NavigationInfo
            {
                NavigationType = NavigationType.Page,
                TargetPage = pageFullType,
                IncludeState = includeState
            };
        }
    }

    public enum NavigationType
    {
        Page,
        DeepLink
    }
}
