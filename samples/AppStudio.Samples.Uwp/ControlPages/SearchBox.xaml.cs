using AppStudio.Uwp.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class SearchBox : BaseControlPage
    {
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>((text) => this.SearchText = text, (text) => text.Length >= 3 );
            }
        }

        public SearchBox()
        {
            this.InitializeComponent();
        }
    }
}
