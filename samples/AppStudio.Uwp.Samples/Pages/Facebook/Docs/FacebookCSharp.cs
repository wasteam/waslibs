using AppStudio.DataProviders.Facebook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class FacebookSample : Page
    {
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(FacebookSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {
            string appId = "";   
            string appSecret = "";
            string FacebookQueryParam = "";
            int MaxRecordsParam = 20;

            Items.Clear();

            var facebookDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = appId, AppSecret = appSecret });
            var config = new FacebookDataConfig
            {
                UserId = FacebookQueryParam
            };          

            var items = await facebookDataProvider.LoadDataAsync(config, MaxRecordsParam);         

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
