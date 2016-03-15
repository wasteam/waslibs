using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "AppServices", Name = "Navigation", Order = 10)]
    public sealed partial class NavigationPage : SamplePage
    {
        private Frame _frameBackup;

        public NavigationPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "Navigation"; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _frameBackup = NavigationService.RootFrame;

            NavigationService.Initialize(typeof(App), navigationSampleFrame);
            NavigationService.NavigateToPage(typeof(NavigationSample1Page));

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationService.Initialize(typeof(App), _frameBackup);

            base.OnNavigatedFrom(e);
        }
    }
}
