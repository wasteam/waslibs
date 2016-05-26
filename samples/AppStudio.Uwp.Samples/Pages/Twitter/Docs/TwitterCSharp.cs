using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Twitter;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class TwitterSample : Page
    {
        private TwitterDataProvider _twitterDataProvider;

        public TwitterSample()
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
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(TwitterSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetItems();
        }

        public async void GetItems()
        {
            string consumerKey = "YourConsumerKey";
            string consumerSecret = "YourConsumerSecret";
            string accessToken = "YourAccessToken";
            string accessTokenSecret = "YourAccessTokenSecret";
            string twitterQueryParam = "WindowsAppStudio";
            TwitterQueryType queryType = TwitterQueryType.Search;
            int maxRecordsParam = 20;

            Items.Clear();

            _twitterDataProvider = new TwitterDataProvider(new TwitterOAuthTokens
            {
                AccessToken = accessToken,
                AccessTokenSecret = accessTokenSecret,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            });

            var config = new TwitterDataConfig
            {
                Query = twitterQueryParam,
                QueryType = queryType
            };          

            var items = await _twitterDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _twitterDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
