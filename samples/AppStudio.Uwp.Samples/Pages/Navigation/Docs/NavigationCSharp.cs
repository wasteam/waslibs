using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationService.Initialize(typeof(App), navigationSampleFrame);
            NavigationService.NavigateToPage(typeof(NavigationSample1Page));
        }
    }

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
