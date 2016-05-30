using System;
using Windows.Networking.Connectivity;

namespace AppStudio.Uwp.Services
{
    public static class InternetConnection
    {
        public static bool IsInternetAvailable()
        {
            try
            {
                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                return connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
