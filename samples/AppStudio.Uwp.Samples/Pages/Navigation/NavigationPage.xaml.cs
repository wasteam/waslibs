using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Navigation;
using System.Windows.Input;
using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "AppServices", Name = "Navigation", Order = 10)]
    public sealed partial class NavigationPage : SamplePage
    {
        public NavigationPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        public override string Caption
        {
            get { return "Navigation"; }
        }

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigationService.NavigateToPage("NavigationSample1Page");
                });
            }
        }
        #endregion
    }
}
