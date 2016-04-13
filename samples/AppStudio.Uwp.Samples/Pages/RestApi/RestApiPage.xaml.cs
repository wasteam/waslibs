using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.DataProviders.RestApi;
using AppStudio.DataProviders.YouTube;
using Newtonsoft.Json.Linq;

namespace AppStudio.Uwp.Samples
{
   [SamplePage(Category = "DataProviders", Name = "RestApi", Order = 60)]
    public sealed partial class RestApiPage : SamplePage
    {
        private const int DefaultMaxRecordsParam = 10;
        private const string DefaultRestApiQuery = "https://www.googleapis.com/youtube/v3/playlistItems?playlistId=UUZPiiUjDlrBv4jiiRqk5dSA&part=snippet&key=AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ";

        RestApiDataProvider<RestApiSchema> restApiDataProvider;
        RestApiDataProvider<RestApiSchema> rawDataProvider;

        public RestApiPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
        }

        public override string Caption
        {
            get { return "RestApiPage Data Provider"; }
        }

        #region DataProvider Config    
        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register("MaxRecordsParam", typeof(int), typeof(RestApiPage), new PropertyMetadata(DefaultMaxRecordsParam));


        public string RestApiQuery
        {
            get { return (string)GetValue(RestApiQueryProperty); }
            set { SetValue(RestApiQueryProperty, value); }
        }

        public static readonly DependencyProperty RestApiQueryProperty = DependencyProperty.Register("RestApiQuery", typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultRestApiQuery));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(RestApiPage), new PropertyMetadata(null));
        #endregion      

        #region DataProviderRawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));
        #endregion

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(RestApiPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register("NoItems", typeof(bool), typeof(RestApiPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(RestApiPage), new PropertyMetadata(false));

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
            AppShell.Current.Shell.ShowRightPane(new RestApiSettings() { DataContext = this });
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

                #region YT  
                var paginatorYT = new TokenPaginator()
                {
                    ContinuationTokenIsUrl = false,
                    ContinuationTokenPath = "nextPageToken",
                    PaginationParameterName = "pageToken"
                };

                Func<string, RestApiSchema> itemYTParser = (x) =>
                   {
                       JObject o = JObject.Parse(x);
                       var result = new RestApiSchema();
                       result._id = (string)o.SelectToken("id.videoId");
                       result.Title = (string)o.SelectToken("snippet.title");
                       result.Description = (string)o.SelectToken("snippet.description");
                       result.ImageUrl= (string)o.SelectToken("snippet.thumbnails.default.url");
                       return result;
                   };

                var configYT = new RestApiDataConfig<RestApiSchema>()
                {
                    Url = new Uri(RestApiQuery, UriKind.Absolute),
                    ElementsRootPath = "items",
                    ItemParser = itemYTParser,
                    PageSizeParameterName = "maxResults",
                    Paginator = paginatorYT
                };

                #endregion

                #region WP  
                var paginatorWP = new TokenPaginator()
                {
                    ContinuationTokenIsUrl = false,
                    ContinuationTokenPath = "meta.next_page",
                    PaginationParameterName = "page_handle"
                };

                Func<string, RestApiSchema> itemWPParser = (x) =>
                  {
                      JObject o = JObject.Parse(x);
                      var result = new RestApiSchema();
                      result._id = (string)o.SelectToken("ID");
                      result.Title = (string)o.SelectToken("title");
                      result.Description = (string)o.SelectToken("content");
                      result.ImageUrl = (string)o.SelectToken("post_thumbnail.URL");
                      return result;
                  };

                var configWP = new RestApiDataConfig<RestApiSchema>()
                {
                    Url = new Uri(RestApiQuery, UriKind.Absolute),
                    ElementsRootPath = "posts",
                    ItemParser = itemWPParser,
                    PageSizeParameterName = "number",
                    Paginator = paginatorWP
                };

                #endregion

                #region Spotify

                var paginatorSF = new TokenPaginator()
                {
                    ContinuationTokenIsUrl = true,
                    ContinuationTokenPath = "artists.next"
                    //PaginationParameterName = "page_handle"
                };

                Func<string, RestApiSchema> itemSFParser = (x) =>
                  {
                      JObject o = JObject.Parse(x);
                      var result = new RestApiSchema();
                      result._id = (string)o.SelectToken("id");
                      result.Title = (string)o.SelectToken("name");
                     // result.Description = (string)o.SelectToken("content");
                      result.ImageUrl = (string)o.SelectToken("images.[0].url");
                      return result;
                  };

                var configSF = new RestApiDataConfig<RestApiSchema>()
                {
                    Url = new Uri(RestApiQuery, UriKind.Absolute),
                    ElementsRootPath = "artists.items",
                    ItemParser = itemSFParser,
                    PageSizeParameterName = "limit",
                    Paginator = paginatorSF
                };

                #endregion


                var items = await restApiDataProvider.LoadDataAsync(configYT, MaxRecordsParam);

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }

                var rawParser = new RawParser();
                var rawData = await rawDataProvider.LoadDataAsync(configYT, MaxRecordsParam, rawParser);
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

                var items = await restApiDataProvider.LoadMoreDataAsync();

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
            RestApiQuery = DefaultRestApiQuery;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }

        private void InitializeDataProvider()
        {
            restApiDataProvider = new RestApiDataProvider<RestApiSchema>();
            rawDataProvider = new RestApiDataProvider<RestApiSchema>();
        }
    }
}
