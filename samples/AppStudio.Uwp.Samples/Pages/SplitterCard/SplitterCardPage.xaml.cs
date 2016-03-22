using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "LayoutControls", Name = "SplitterCard", Order = 60)]
    public sealed partial class SplitterCardPage : SamplePage
    {
        public SplitterCardPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "SplitterCard Control"; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new SplitterCardSettings() { DataContext = control });
        }
    }
}
