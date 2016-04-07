using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "SearchBox", Order = 40)]
    public sealed partial class SearchBoxPage : SamplePage
    {
        public SearchBoxPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
            this.SearchCommand = new RelayCommand<string>((searchTerm) => { this.SearchText = searchTerm; }, (searchTerm) => { return (!string.IsNullOrEmpty(searchTerm) && searchTerm.Length >= 3); });
        }

        #region SearchText
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBoxPage), new PropertyMetadata(string.Empty));
        #endregion

        #region SearchCommand
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchBoxPage), new PropertyMetadata(null));
        #endregion

        public override string Caption
        {
            get { return "SearchBox Control"; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new SearchBoxSettings() { DataContext = control });
        }
    }
}
