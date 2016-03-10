using AppStudio.Uwp.Navigation;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "AppServices", Name = "Navigation", Order = 10)]
    public sealed partial class NavigationPage : SamplePage
    {
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
            base.OnNavigatedTo(e);
            NavigationService.Initialize(typeof(App), navigationSampleFrame);
            NavigationService.NavigateToPage(typeof(NavigationSample1Page));            
        }
    }
}
