using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using AppStudio.Uwp.Controls;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class ImageExSettings : UserControl
    {
        public ImageExSettings()
        {
            this.InitializeComponent();
        }

        public ImageExPage Page { get; set; }

        private async void OnClearCache(object sender, RoutedEventArgs e)
        {
            Page.Clear();
            await Task.Delay(1000);
            await BitmapCache.ClearCacheAsync();
        }

        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            Page.Refresh();
        }
    }
}
