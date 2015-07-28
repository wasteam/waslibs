using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppStudio.Samples.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IList<NavigableItem> _items;
        public IList<NavigableItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<NavigableItem>()
                    {
                        new NavigableItem("Navigation", NavigationInfo.FromPage("Navigation")),
                        new NavigableItem("ActionCommandBar", NavigationInfo.FromPage("ActionCommandBar")),
                        new NavigableItem("App Cache", NavigationInfo.FromPage("AppCache")),
                        new NavigableItem("Html to Xaml", NavigationInfo.FromPage("HtmlToXaml")),
                        new NavigableItem("Converters", NavigationInfo.FromPage("Converters"))
                    };
                }
                return _items;
            }
        }
        public RelayCommand<NavigableItem> ItemClickCommand
        {
            get
            {
                return new RelayCommand<NavigableItem>((item) => { NavigationService.NavigateTo(item); });
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
