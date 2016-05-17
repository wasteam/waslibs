using AppStudio.DataProviders;
using AppStudio.DataProviders.LocalStorage;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class LocalStorageSample : Page
    {
        LocalStorageDataProvider<LocalStorageDataSchema> _localStorageDataProvider;

        public LocalStorageSample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty
            .Register("Items", typeof(ObservableCollection<object>), typeof(LocalStorageSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetItems();
        }

        public async void GetItems()
        {
            string localStorageQuery = "/Assets/LocalStorageSamples.json";
            int maxRecordsParam = 10;
            string orderBy = "Title";
            SortDirection sortDirection = SortDirection.Ascending;

            _localStorageDataProvider = new LocalStorageDataProvider<LocalStorageDataSchema>();
            var config = new LocalStorageDataConfig
            {
                FilePath = localStorageQuery,
                OrderBy = orderBy,
                SortDirection = sortDirection
            };

            var items = await _localStorageDataProvider.LoadDataAsync(config, maxRecordsParam);

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _localStorageDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
