using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders;
using AppStudio.DataProviders.Flickr;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class FlickrSample : Page
    {
        private FlickrDataProvider _flickrDataProvider;

        public FlickrSample()
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
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(FlickrSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetItems();
        }

        public async void GetItems()
        {
            string flickrQueryParam = "Abstract";
            FlickrQueryType queryType = FlickrQueryType.Tags;
            int maxRecordsParam = 12;
            string orderBy = "Published";
            SortDirection sortDirection = SortDirection.Descending;

            _flickrDataProvider = new FlickrDataProvider();
            this.Items = new ObservableCollection<object>();

            var config = new FlickrDataConfig
            {
                Query = flickrQueryParam,
                QueryType = queryType,
                OrderBy = orderBy,
                SortDirection = sortDirection
            };

            var items = await _flickrDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _flickrDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
