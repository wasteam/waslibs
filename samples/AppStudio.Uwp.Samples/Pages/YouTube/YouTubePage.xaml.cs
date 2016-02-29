using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "YouTube")]
    public sealed partial class YouTubePage : SamplePage
    {
        private const string DefaultApiKey = "AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ";
        public YouTubePage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "YouTube Data Provider"; }
        }
        #region ApiKey
        public string ApiKey
        {
            get { return (string)GetValue(ApiKeyProperty); }
            set { SetValue(ApiKeyProperty, value); }
        }
        public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register("ApiKey", typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultApiKey));
        #endregion
        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(YouTubePage), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(YouTubePage), new PropertyMetadata(null));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>(new BingDataSource().GetItems());
            //this.ItemTemplate = Resources["BingTemplate"] as DataTemplate;
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new YouTubeSettings() { DataContext = this });
        }
    }
}
