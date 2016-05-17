using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Facebook;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class FacebookSample : Page
    {
        private FacebookDataProvider _facebookDataProvider;

        public FacebookSample()
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
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(FacebookSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {
            string appId = "YourAppId";   
            string appSecret = "YourAppSecret";
            string FacebookQueryParam = "8195378771";
            int MaxRecordsParam = 20;

            Items.Clear();

            _facebookDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = appId, AppSecret = appSecret });
            var config = new FacebookDataConfig
            {
                UserId = FacebookQueryParam
            };          

            var items = await _facebookDataProvider.LoadDataAsync(config, MaxRecordsParam);         

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _facebookDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
