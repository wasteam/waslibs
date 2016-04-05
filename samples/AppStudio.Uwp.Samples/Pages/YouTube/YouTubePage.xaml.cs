using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.DataProviders.YouTube;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "YouTube", Order = 40)]
    public sealed partial class YouTubePage : SamplePage
    {
        private const string DefaultApiKey = "AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ";
        private const string DefaultYouTubeQueryParam = "PLZCHH_4VqpRjpQP36-XM1jb1E_JIxJZFJ";
        private const YouTubeQueryType DefaultQueryType = YouTubeQueryType.Playlist;
        private const int DefaultMaxRecordsParam = 20;
        YouTubeDataProvider youTubeDataProvider;

        public YouTubePage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = ApiKey });
        }

        public override string Caption
        {
            get { return "YouTube Data Provider"; }
        }

        #region DataProvider Config
        public string ApiKey
        {
            get { return (string)GetValue(ApiKeyProperty); }
            set { SetValue(ApiKeyProperty, value); }
        }

        public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register("ApiKey", typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultApiKey));


        public string YouTubeQueryParam
        {
            get { return (string)GetValue(YouTubeQueryParamProperty); }
            set { SetValue(YouTubeQueryParamProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryParamProperty = DependencyProperty.Register("YouTubeQueryParam", typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultYouTubeQueryParam));


        public YouTubeQueryType YouTubeQueryTypeSelectedItem
        {
            get { return (YouTubeQueryType)GetValue(YouTubeQueryTypeSelectedItemProperty); }
            set { SetValue(YouTubeQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryTypeSelectedItemProperty = DependencyProperty.Register("YouTubeQueryTypeSelectedItem", typeof(YouTubeQueryType), typeof(YouTubePage), new PropertyMetadata(DefaultQueryType));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register("MaxRecordsParam", typeof(int), typeof(YouTubePage), new PropertyMetadata(DefaultMaxRecordsParam));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(YouTubePage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(YouTubePage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(YouTubePage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register("NoItems", typeof(bool), typeof(YouTubePage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(YouTubePage), new PropertyMetadata(false));

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

        public ICommand MoreDataCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MoreItemsRequest();
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
            AppShell.Current.Shell.ShowRightPane(new YouTubeSettings() { DataContext = this });
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
               
                var config = new YouTubeDataConfig
                {
                    Query = YouTubeQueryParam,
                    QueryType = YouTubeQueryTypeSelectedItem
                };

                var rawParser = new RawParser();
                var rawData = await youTubeDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;

                var items = await youTubeDataProvider.LoadDataAsync(config, MaxRecordsParam);

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

        private async void MoreItemsRequest()
        {
            try
            {
                IsBusy = true;
                HasErrors = false;
                NoItems = false;
                DataProviderRawData = string.Empty;
                Items.Clear();

                var config = new YouTubeDataConfig
                {
                    Query = YouTubeQueryParam,
                    QueryType = YouTubeQueryTypeSelectedItem
                };
            
                DataProviderRawData = string.Empty;

                var items = await youTubeDataProvider.LoadMoreDataAsync();

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
            ApiKey = DefaultApiKey;
            YouTubeQueryParam = DefaultYouTubeQueryParam;
            YouTubeQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }
    }
}
