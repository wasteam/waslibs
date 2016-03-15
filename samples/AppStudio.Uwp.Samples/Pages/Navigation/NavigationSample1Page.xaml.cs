using System.Windows.Input;

using Windows.UI.Xaml.Controls;

using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class NavigationSample1Page : Page
    {
        public NavigationSample1Page()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigationService.NavigateToPage("NavigationSample2Page");
                });
            }
        }
        #endregion
    }
}
