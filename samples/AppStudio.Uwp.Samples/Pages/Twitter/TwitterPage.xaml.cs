using AppStudio.DataProviders.Twitter;
using AppStudio.Uwp.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Twitter", Order = 60)]
    public sealed partial class TwitterPage : SamplePage
    {
        private const string DefaultConsumerKey = "29TPMHBW0QcFIWvNrfWxUGmlV";
        private const string DefaultConsumerSecret = "7cp8HDzES42iAFGgE5yxJ3wAxsrDdu5uEHwhoOKPlN6Q2P8k6s";
        private const string DefaultAccessToken = "275442106-OdbhPuGr8biRdQsHbtzNSMVvHRrX9acsLbiyYgCF";
        private const string DefaultAccessTokenSecret = "GA4Uw2sMgvSayjWTw9qdejB8LzNfNS2cAaQPimVDVhdIP";
        private const string DefaultTwitterQueryParam = "WindowsAppStudio";
        private const TwitterQueryType DefaultQueryType = TwitterQueryType.Search;
        private const int DefaultMaxRecordsParam = 20;

        public TwitterPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "Twitter Data Provider"; }
        }

        #region DataProvider Config
        public string ConsumerKey
        {
            get { return (string)GetValue(ConsumerKeyProperty); }
            set { SetValue(ConsumerKeyProperty, value); }
        }

        public static readonly DependencyProperty ConsumerKeyProperty = DependencyProperty.Register("ConsumerKey", typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultConsumerKey));

        public string ConsumerSecret
        {
            get { return (string)GetValue(ConsumerSecretProperty); }
            set { SetValue(ConsumerSecretProperty, value); }
        }

        public static readonly DependencyProperty ConsumerSecretProperty = DependencyProperty.Register("ConsumerSecret", typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultConsumerSecret));

        public string AccessToken
        {
            get { return (string)GetValue(AccessTokenProperty); }
            set { SetValue(AccessTokenProperty, value); }
        }

        public static readonly DependencyProperty AccessTokenProperty = DependencyProperty.Register("AccessToken", typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultAccessToken));

        public string AccessTokenSecret
        {
            get { return (string)GetValue(AccessTokenSecretProperty); }
            set { SetValue(AccessTokenSecretProperty, value); }
        }

        public static readonly DependencyProperty AccessTokenSecretProperty = DependencyProperty.Register("AccessTokenSecret", typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultAccessTokenSecret));


        public string TwitterQueryParam
        {
            get { return (string)GetValue(YouTubeQueryParamProperty); }
            set { SetValue(YouTubeQueryParamProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryParamProperty = DependencyProperty.Register("TwitterQueryParam", typeof(string), typeof(TwitterPage), new PropertyMetadata(DefaultTwitterQueryParam));


        public TwitterQueryType TwitterQueryTypeSelectedItem
        {
            get { return (TwitterQueryType)GetValue(TwitterQueryTypeSelectedItemProperty); }
            set { SetValue(TwitterQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty TwitterQueryTypeSelectedItemProperty = DependencyProperty.Register("TwitterQueryTypeSelectedItemProperty", typeof(TwitterQueryType), typeof(TwitterPage), new PropertyMetadata(DefaultQueryType));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register("MaxRecordsParam", typeof(int), typeof(TwitterPage), new PropertyMetadata(DefaultMaxRecordsParam));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(TwitterPage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(TwitterPage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(TwitterPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register("NoItems", typeof(bool), typeof(TwitterPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(TwitterPage), new PropertyMetadata(false));

        #endregion

        #region ICommands
        public ICommand RefreshDataCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Request();
                });
            }
        }

        public ICommand RestoreConfigCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RestoreConfig();
                });
            }
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>();
            RestoreConfig();
            Request();

            base.OnNavigatedTo(e);
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new TwitterSettings() { DataContext = this });
        }

        private async void Request()
        {
            try
            {
                IsBusy = true;
                HasErrors = false;
                NoItems = false;
                DataProviderRawData = string.Empty;
                Items.Clear();

                var twitterDataProvider = new TwitterDataProvider(new TwitterOAuthTokens
                {
                    AccessToken = AccessToken,
                    AccessTokenSecret = AccessTokenSecret,
                    ConsumerKey = ConsumerKey,
                    ConsumerSecret = ConsumerSecret
                });

                var config = new TwitterDataConfig
                {
                    Query = TwitterQueryParam,
                    QueryType = TwitterQueryTypeSelectedItem
                };

                var rawParser = new RawParser();
                var rawData = await twitterDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;

                var items = await twitterDataProvider.LoadDataAsync(config, MaxRecordsParam);

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                DataProviderRawData += ex.Message;
                DataProviderRawData += ex.StackTrace;
                HasErrors = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void RestoreConfig()
        {
            ConsumerKey = DefaultConsumerKey;
            ConsumerSecret = DefaultConsumerSecret;
            AccessToken = DefaultAccessToken;
            AccessTokenSecret = DefaultAccessTokenSecret;
            TwitterQueryParam = DefaultTwitterQueryParam;
            TwitterQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }
    }
}
