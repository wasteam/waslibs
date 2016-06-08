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
        private const string DefaultYouTubeQueryParam = "PLZCHH_4VqpRjpQP36-XM1jb1E_JIxJZFJ";
        private const YouTubeQueryType DefaultQueryType = YouTubeQueryType.Playlist;
        private const int DefaultMaxRecordsParam = 20;
        private const YouTubeSearchOrderBy DefaultOrderBy = YouTubeSearchOrderBy.None;

        YouTubeDataProvider youTubeDataProvider;
        YouTubeDataProvider rawDataProvider;

        public YouTubePage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
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

        public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register(nameof(ApiKey), typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultApiKey));


        public string YouTubeQueryParam
        {
            get { return (string)GetValue(YouTubeQueryParamProperty); }
            set { SetValue(YouTubeQueryParamProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryParamProperty = DependencyProperty.Register(nameof(YouTubeQueryParam), typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultYouTubeQueryParam));


        public YouTubeQueryType YouTubeQueryTypeSelectedItem
        {
            get { return (YouTubeQueryType)GetValue(YouTubeQueryTypeSelectedItemProperty); }
            set { SetValue(YouTubeQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryTypeSelectedItemProperty = DependencyProperty.Register(nameof(YouTubeQueryTypeSelectedItem), typeof(YouTubeQueryType), typeof(YouTubePage), new PropertyMetadata(DefaultQueryType));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register(nameof(MaxRecordsParam), typeof(int), typeof(YouTubePage), new PropertyMetadata(DefaultMaxRecordsParam));


        public YouTubeSearchOrderBy OrderBy
        {
            get { return (YouTubeSearchOrderBy)GetValue(OrderByProperty); }
            set { SetValue(OrderByProperty, value); }
        }

        public static readonly DependencyProperty OrderByProperty = DependencyProperty.Register(nameof(OrderBy), typeof(YouTubeSearchOrderBy), typeof(YouTubePage), new PropertyMetadata(DefaultOrderBy));


        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(YouTubePage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register(nameof(DataProviderRawData), typeof(string), typeof(YouTubePage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(YouTubePage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof(NoItems), typeof(bool), typeof(YouTubePage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(YouTubePage), new PropertyMetadata(false));

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
                    Request();
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
                    QueryType = YouTubeQueryTypeSelectedItem,
                    SearchVideosOrderBy = OrderBy
                };

                var items = await youTubeDataProvider.LoadDataAsync(config, MaxRecordsParam);

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }

                var rawParser = new RawParser();
                var rawData = await rawDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;
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

                var items = await youTubeDataProvider.LoadMoreDataAsync();

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }

                var rawData = await rawDataProvider.LoadMoreDataAsync<RawSchema>();
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;
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
            OrderBy = DefaultOrderBy;
        }

        private void InitializeDataProvider()
        {
            youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = ApiKey });
            rawDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = ApiKey });
        }

        private const string DefaultApiKey = "AIzaSyBTho5YSdBMdiM78irZNljWItF25VJngkk";
    }
}
