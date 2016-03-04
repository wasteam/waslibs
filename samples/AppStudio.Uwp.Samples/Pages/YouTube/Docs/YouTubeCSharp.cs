using AppStudio.DataProviders.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class YouTubeSample : Page
    {
        public YouTubeSample()
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(YouTubeSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {
            string apiKey = "YourApiKey";
            string queryParam = "PLZCHH_4VqpRjpQP36-XM1jb1E_JIxJZFJ";
            YouTubeQueryType queryType = YouTubeQueryType.Playlist;
            int maxRecordsParam = 20;

            this.Items = new ObservableCollection<object>();
            var _youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = apiKey });
            var config = new YouTubeDataConfig
            {
                Query = queryParam,
                QueryType = queryType
            };

            var items = await _youTubeDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
