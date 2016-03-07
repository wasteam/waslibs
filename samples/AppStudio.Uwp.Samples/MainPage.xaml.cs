using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public string Caption
        {
            get { return "Windows App Studio Uwp Samples"; }
        }
    }
}
