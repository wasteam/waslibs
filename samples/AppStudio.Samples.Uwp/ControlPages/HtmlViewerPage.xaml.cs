using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class HtmlViewerPage : Page
    {
        public HtmlViewerPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            htmlViewer.ContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
        }
    }
}
