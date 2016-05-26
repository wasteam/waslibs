using System;

namespace AppStudio.Uwp.Navigation
{
    [Obsolete("Implement your custom navigation logic")]
    public interface INavigable
    {
        NavigationInfo NavigationInfo { get; set; }
    }
}
