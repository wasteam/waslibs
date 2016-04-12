using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "GifControl", Order = 10)]
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
    }
}
