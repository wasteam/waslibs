using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.RestApi;
using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "REST API", Order = 50)]
    public sealed partial class RestApiPage : SamplePage
    {
        private const string DefaultRestApiUrl = "https://api.twitch.tv/kraken/games/top";       

        private const PaginationParameterType DefaultParameterType = PaginationParameterType.Token;
        private const string DefaultPaginationParameterName = "";

        private const int DefaultInitialValue = 1;
        private const int DefaultIncrementalValue = 1;


        private const string DefaultTokenName = "_links.next";
        private const bool DefaultTokenIsUrl = true;

        private const string DefaultItemsPerPageParameterName = "limit";
        private const int DefaultMaxRecordsParam = 20;

        private const string DefaultRestApiMainRoot = "top";
        private const string DefaultProperty1 = "game.name";
        private const string DefaultProperty2 = "viewers";
        private const string DefaultImageProperty = "game.box.medium";

        RestApiDataProvider restApiDataProvider;
        RestApiDataProvider rawDataProvider;

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
            get { return "REST API Data Provider"; }
        }

        #region DataProvider Config          


        public string RestApiUrl
        {
            get { return (string)GetValue(RestApiUrlProperty); }
            set { SetValue(RestApiUrlProperty, value); }
        }

        public static readonly DependencyProperty RestApiUrlProperty = DependencyProperty.Register(nameof(RestApiUrl), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultRestApiUrl));


        public string RestApiMainRoot
        {
            get { return (string)GetValue(RestApiMainRootProperty); }
            set { SetValue(RestApiMainRootProperty, value); }
        }

        public static readonly DependencyProperty RestApiMainRootProperty = DependencyProperty.Register(nameof(RestApiMainRoot), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultRestApiMainRoot));


        public PaginationParameterType PaginationParameterType
        {
            get { return (PaginationParameterType)GetValue(PaginationParameterTypeProperty); }
            set { SetValue(PaginationParameterTypeProperty, value); }
        }

        public static readonly DependencyProperty PaginationParameterTypeProperty = DependencyProperty.Register(nameof(PaginationParameterType), typeof(PaginationParameterType), typeof(RestApiPage), new PropertyMetadata(DefaultParameterType));


        public string PaginationParameterName
        {
            get { return (string)GetValue(PaginationParameterNameProperty); }
            set { SetValue(PaginationParameterNameProperty, value); }
        }

        public static readonly DependencyProperty PaginationParameterNameProperty = DependencyProperty.Register(nameof(PaginationParameterName), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultPaginationParameterName));


        public string ResponseTokenName
        {
            get { return (string)GetValue(ResponseTokenNameProperty); }
            set { SetValue(ResponseTokenNameProperty, value); }
        }

        public static readonly DependencyProperty ResponseTokenNameProperty = DependencyProperty.Register(nameof(ResponseTokenName), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultTokenName));


        public bool TokenIsUrl
        {
            get { return (bool)GetValue(TokenIsUrlProperty); }
            set { SetValue(TokenIsUrlProperty, value); }
        }

        public static readonly DependencyProperty TokenIsUrlProperty = DependencyProperty.Register(nameof(TokenIsUrl), typeof(bool), typeof(RestApiPage), new PropertyMetadata(DefaultTokenIsUrl));


        public int InitialValue
        {
            get { return (int)GetValue(InitialValueProperty); }
            set { SetValue(InitialValueProperty, value); }
        }

        public static readonly DependencyProperty InitialValueProperty = DependencyProperty.Register(nameof(InitialValue), typeof(int), typeof(RestApiPage), new PropertyMetadata(DefaultInitialValue));


        public int IncrementalValue
        {
            get { return (int)GetValue(IncrementalValueProperty); }
            set { SetValue(IncrementalValueProperty, value); }
        }

        public static readonly DependencyProperty IncrementalValueProperty = DependencyProperty.Register(nameof(IncrementalValue), typeof(int), typeof(RestApiPage), new PropertyMetadata(DefaultIncrementalValue));


        public string ItemsPerPageParameterName
        {
            get { return (string)GetValue(ItemsPerPageParameterNameProperty); }
            set { SetValue(ItemsPerPageParameterNameProperty, value); }
        }

        public static readonly DependencyProperty ItemsPerPageParameterNameProperty = DependencyProperty.Register(nameof(ItemsPerPageParameterName), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultItemsPerPageParameterName));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register(nameof(MaxRecordsParam), typeof(int), typeof(RestApiPage), new PropertyMetadata(DefaultMaxRecordsParam));


        public string TextProperty1
        {
            get { return (string)GetValue(Property1Property); }
            set { SetValue(Property1Property, value); }
        }

        public static readonly DependencyProperty Property1Property = DependencyProperty.Register(nameof(TextProperty1), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultProperty1));


        public string TextProperty2
        {
            get { return (string)GetValue(Property2Property); }
            set { SetValue(Property2Property, value); }
        }

        public static readonly DependencyProperty Property2Property = DependencyProperty.Register(nameof(TextProperty2), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultProperty2));


        public string ImageProperty
        {
            get { return (string)GetValue(ImagePropertyProperty); }
            set { SetValue(ImagePropertyProperty, value); }
        }

        public static readonly DependencyProperty ImagePropertyProperty = DependencyProperty.Register(nameof(ImageProperty), typeof(string), typeof(RestApiPage), new PropertyMetadata(DefaultImageProperty));



        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(RestApiPage), new PropertyMetadata(null));
        #endregion      

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register(nameof(DataProviderRawData), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));
        #endregion

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(RestApiPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof(NoItems), typeof(bool), typeof(RestApiPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(RestApiPage), new PropertyMetadata(false));

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
                var parser = new RestApiSampleParser();
                parser.InitializeSample(RestApiMainRoot, TextProperty1, TextProperty2, ImageProperty);

                var config = new RestApiDataConfig()
                {
                    Url = new Uri(RestApiUrl),                    
                };

                var paginationConfig = GetPaginationConfig();
                if (paginationConfig != null)
                {
                    config.PaginationConfig = paginationConfig;
                }
                
                var items = await restApiDataProvider.LoadDataAsync(config, MaxRecordsParam, parser);

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

        private IPaginationConfig GetPaginationConfig()
        {
            if (PaginationParameterType == PaginationParameterType.Numeric)
            {
                return new NumericPagination()
                {
                    PageSizeParameterName = ItemsPerPageParameterName,
                    PaginationParameterName = PaginationParameterName,
                    ContinuationTokenInitialValue = InitialValue.ToString(),
                    IncrementalValue = IncrementalValue                
                };
            }
            else if (PaginationParameterType == PaginationParameterType.Token)
            {
                return new TokenPagination()
                {
                    PageSizeParameterName = ItemsPerPageParameterName,
                    PaginationParameterName = PaginationParameterName,
                    ContinuationTokenIsUrl = TokenIsUrl,
                    ContinuationTokenPath = ResponseTokenName                    
                };
            }
            return null;
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

                var items = await restApiDataProvider.LoadMoreDataAsync<RestApiSampleSchema>();

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
            RestApiUrl = DefaultRestApiUrl;
            PaginationParameterType = DefaultParameterType;
            PaginationParameterName = DefaultPaginationParameterName;
            InitialValue = DefaultInitialValue;
            IncrementalValue = DefaultIncrementalValue;
            ResponseTokenName = DefaultTokenName;
            TokenIsUrl = DefaultTokenIsUrl;
            ItemsPerPageParameterName = DefaultItemsPerPageParameterName;          
            MaxRecordsParam = DefaultMaxRecordsParam;

            RestApiMainRoot = DefaultRestApiMainRoot;
            TextProperty1 = DefaultProperty1;
            TextProperty2 = DefaultProperty2;
            ImageProperty = DefaultImageProperty;
        }

        private void InitializeDataProvider()
        {
            restApiDataProvider = new RestApiDataProvider();
            rawDataProvider = new RestApiDataProvider();
        }
    }
}
