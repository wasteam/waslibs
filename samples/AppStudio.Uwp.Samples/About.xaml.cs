using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class About : UserControl
    {
        public AboutViewModel ViewModel { get; set; } = new AboutViewModel();
        public About()
        {
            this.InitializeComponent();
            Loaded += About_Loaded;

            DataContext = ViewModel;
        }

        private void About_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadState();
        }
    }
}
