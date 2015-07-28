using System.Linq;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;

namespace AppStudio.Common.Services
{
    public static class InternetConnection
    {
        public static bool IsInternetAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return false;
            }

            return NetworkInformation.GetConnectionProfiles().Any(p => IsInternetProfile(p));
        }

        private static bool IsInternetProfile(ConnectionProfile connectionProfile)
        {
            if (connectionProfile == null)
            {
                return false;
            }

            var connectivityLevel = connectionProfile.GetNetworkConnectivityLevel();
            return connectivityLevel != NetworkConnectivityLevel.None;
        }
    }
}
