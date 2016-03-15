using System.Windows.Input;

using Windows.UI.Xaml.Controls;

using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class NavigationSample2Page : Page
    {
        public NavigationSample2Page()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        #region Commands
        public ICommand GoBackCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (NavigationService.CanGoBack())
                    {
                        NavigationService.GoBack();
                    }                    
                });
            }
        }
        #endregion
    }
}
