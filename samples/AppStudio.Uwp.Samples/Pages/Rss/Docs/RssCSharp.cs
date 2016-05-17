using AppStudio.DataProviders;
using AppStudio.DataProviders.Rss;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class RssSample : Page
    {
        private RssDataProvider _rssDataProvider;

        public RssSample()
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(RssSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetItems();
        }

        public async void GetItems()
        {
            string rssQuery = "http://www.blogger.com/feeds/6781693/posts/default";
            int maxRecordsParam = 20;
            string orderBy = "PublishDate";
            SortDirection sortDirection = SortDirection.Descending;

            _rssDataProvider = new RssDataProvider();
            this.Items = new ObservableCollection<object>();

            var config = new RssDataConfig
            {
                Url = new Uri(rssQuery, UriKind.Absolute),
                OrderBy = orderBy,
                SortDirection = sortDirection
            };

            var items = await _rssDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _rssDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
