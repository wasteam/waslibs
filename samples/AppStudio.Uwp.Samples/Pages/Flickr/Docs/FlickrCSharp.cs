using AppStudio.DataProviders.Flickr;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class FlickrSample : Page
    {
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(FlickrSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {
            string flickrQueryParam = "Seatle";
            FlickrQueryType queryType = FlickrQueryType.Tags;
            int maxRecordsParam = 10;
            Items.Clear();

            var flickrDataProvider = new FlickrDataProvider();
            var config = new FlickrDataConfig
            {
                Query = flickrQueryParam,
                QueryType = queryType
            };          

            var items = await flickrDataProvider.LoadDataAsync(config, maxRecordsParam);
          
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
