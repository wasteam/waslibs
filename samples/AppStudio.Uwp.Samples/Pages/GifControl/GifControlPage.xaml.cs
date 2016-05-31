
namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "GifControl", Order = 12)]
    public sealed partial class GifControlPage : SamplePage
    {
        public GifControlPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        public override string Caption
        {
            get { return "GifControl"; }
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new GifControlSettings() { DataContext = control });
        }

        private void OnStop(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            control.Stop();
        }

        private void OnPlay(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            control.Play();
        }

        private void OnPause(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            control.Pause();
        }
    }
}
