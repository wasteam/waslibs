using AppStudio.DataProviders.YouTube;
using AppStudio.Uwp.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "YouTube")]
    public sealed partial class YouTubePage : SamplePage
    {
        private const string DefaultApiKey = "AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ";
        private const string DefaultYouTubeQueryParam = @"MicrosoftLumia";
        private const YouTubeQueryType DefaultQueryType = YouTubeQueryType.Channels;
        private const int DefaultMaxRecordsParam = 20;
        public YouTubePage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "YouTube Data Provider"; }
        }

        #region ApiKey
        public string ApiKey
        {
            get { return (string)GetValue(ApiKeyProperty); }
            set { SetValue(ApiKeyProperty, value); }
        }

        public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register("ApiKey", typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultApiKey));

        #endregion

        #region Query
        public string YouTubeQueryParam
        {
            get { return (string)GetValue(YouTubeQueryParamProperty); }
            set { SetValue(YouTubeQueryParamProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryParamProperty = DependencyProperty.Register("YouTubeQueryParam", typeof(string), typeof(YouTubePage), new PropertyMetadata(DefaultYouTubeQueryParam));

        #endregion

        #region QueryType
      
        public YouTubeQueryType YouTubeQueryTypeSelectedItem
        {
            get { return (YouTubeQueryType)GetValue(YouTubeQueryTypeSelectedItemProperty); }
            set { SetValue(YouTubeQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty YouTubeQueryTypeSelectedItemProperty = DependencyProperty.Register("YouTubeQueryTypeSelectedItem", typeof(YouTubeQueryType), typeof(YouTubePage), new PropertyMetadata(DefaultQueryType));

        #endregion

        #region MaxRecords
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

        #region DataProviderError
        public string DataProviderError
        {
            get { return (string)GetValue(DataProviderErrorProperty); }
            set { SetValue(DataProviderErrorProperty, value); }
        }

        public static readonly DependencyProperty DataProviderErrorProperty = DependencyProperty.Register("DataProviderError", typeof(string), typeof(YouTubePage), new PropertyMetadata(string.Empty));

        #endregion

        #region JsonResult
        public string DataProviderJsonResult
        {
            get { return (string)GetValue(DataProviderJsonResultProperty); }
            set { SetValue(DataProviderJsonResultProperty, value); }
        }

        public static readonly DependencyProperty DataProviderJsonResultProperty = DependencyProperty.Register("DataProviderJsonResult", typeof(string), typeof(YouTubePage), new PropertyMetadata(string.Empty));

        #endregion    

        #region ICommands
        public ICommand RefreshData
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Request();
                });
            }
        }

        public ICommand SetDefault
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetDefaultData();
                });
            }
        }

        public ICommand OpenUrl
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(YouTubeQuery));
                });
            }
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            this.Items = new ObservableCollection<object>();
            SetDefaultData();
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
                DataProviderError = string.Empty;
                DataProviderJsonResult = string.Empty;
                Items.Clear();

                var _youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = ApiKey });
                var config = new YouTubeDataConfig
                {
                    Query = YouTubeQueryParam,
                    QueryType = YouTubeQueryTypeSelectedItem
                };

                var items = await _youTubeDataProvider.LoadDataAsync(config, MaxRecordsParam);
                foreach (var item in items)
                {
                    Items.Add(item);
                }

                HttpClient client = new HttpClient();
                DataProviderJsonResult = await client.GetStringAsync(new Uri(YouTubeQuery, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                DataProviderError += ex.Message;
                DataProviderError += ex.StackTrace;
            }
        }

        private void SetDefaultData()
        {
            ApiKey = DefaultApiKey;
            YouTubeQueryParam = DefaultYouTubeQueryParam;
            YouTubeQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;           
        } 

        private string YouTubeQuery
        {
            get
            {
                switch (YouTubeQueryTypeSelectedItem)
                {
                    case YouTubeQueryType.Channels:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/channels?forUsername={0}&part=contentDetails&maxResults={1}&key={2}", YouTubeQueryParam, MaxRecordsParam, ApiKey);

                    case YouTubeQueryType.Playlist:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/playlistItems?playlistId={0}&part=snippet&maxResults={1}&key={2}", YouTubeQueryParam, MaxRecordsParam, ApiKey);

                    case YouTubeQueryType.Videos:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/search?q={0}&part=snippet&maxResults={1}&key={2}&type=video", YouTubeQueryParam, MaxRecordsParam, ApiKey);

                    default:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/search?q={0}&part=snippet&maxResults={1}&key={2}&type=video", YouTubeQueryParam, MaxRecordsParam, ApiKey);
                }
            }
        }

    }
}
