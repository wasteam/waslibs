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
    [SamplePage(Category = "DataProviders", Name = "REST API", Order = 90)]
    public sealed partial class RestApiPage : SamplePage
    {

        private const RestApiSampleType DefaultSample = RestApiSampleType.NumericPaginationSample;
        private const int DefaultMaxRecordsParam = 20;

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


        public string RestApiQuery
        {
            get { return (string)GetValue(RestApiUrlProperty); }
            set { SetValue(RestApiUrlProperty, value); }
        }

        public static readonly DependencyProperty RestApiUrlProperty = DependencyProperty.Register(nameof(RestApiQuery), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public string RestApiMainRoot
        {
            get { return (string)GetValue(RestApiMainRootProperty); }
            set { SetValue(RestApiMainRootProperty, value); }
        }

        public static readonly DependencyProperty RestApiMainRootProperty = DependencyProperty.Register(nameof(RestApiMainRoot), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public PaginationParameterType PaginationParameterType
        {
            get { return (PaginationParameterType)GetValue(PaginationParameterTypeProperty); }
            set { SetValue(PaginationParameterTypeProperty, value); }
        }

        public static readonly DependencyProperty PaginationParameterTypeProperty = DependencyProperty.Register(nameof(PaginationParameterType), typeof(PaginationParameterType), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public string PaginationParameterName
        {
            get { return (string)GetValue(PaginationParameterNameProperty); }
            set { SetValue(PaginationParameterNameProperty, value); }
        }

        public static readonly DependencyProperty PaginationParameterNameProperty = DependencyProperty.Register(nameof(PaginationParameterName), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public string ResponseTokenName
        {
            get { return (string)GetValue(ResponseTokenNameProperty); }
            set { SetValue(ResponseTokenNameProperty, value); }
        }

        public static readonly DependencyProperty ResponseTokenNameProperty = DependencyProperty.Register(nameof(ResponseTokenName), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public RestApiTokenType TokenParameterType
        {
            get { return (RestApiTokenType)GetValue(TokenParameterTypeProperty); }
            set { SetValue(TokenParameterTypeProperty, value); }
        }

        public static readonly DependencyProperty TokenParameterTypeProperty = DependencyProperty.Register(nameof(TokenParameterType), typeof(RestApiTokenType), typeof(RestApiPage), new PropertyMetadata(RestApiTokenType.Parameter));


        public int InitialValue
        {
            get { return (int)GetValue(InitialValueProperty); }
            set { SetValue(InitialValueProperty, value); }
        }

        public static readonly DependencyProperty InitialValueProperty = DependencyProperty.Register(nameof(InitialValue), typeof(int), typeof(RestApiPage), new PropertyMetadata(0));


        public int IncrementalValue
        {
            get { return (int)GetValue(IncrementalValueProperty); }
            set { SetValue(IncrementalValueProperty, value); }
        }

        public static readonly DependencyProperty IncrementalValueProperty = DependencyProperty.Register(nameof(IncrementalValue), typeof(int), typeof(RestApiPage), new PropertyMetadata(0));


        public string ItemsPerPageParameterName
        {
            get { return (string)GetValue(ItemsPerPageParameterNameProperty); }
            set { SetValue(ItemsPerPageParameterNameProperty, value); }
        }

        public static readonly DependencyProperty ItemsPerPageParameterNameProperty = DependencyProperty.Register(nameof(ItemsPerPageParameterName), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


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

        public static readonly DependencyProperty Property1Property = DependencyProperty.Register(nameof(TextProperty1), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public string TextProperty2
        {
            get { return (string)GetValue(Property2Property); }
            set { SetValue(Property2Property, value); }
        }

        public static readonly DependencyProperty Property2Property = DependencyProperty.Register(nameof(TextProperty2), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));


        public string ImageProperty
        {
            get { return (string)GetValue(ImagePropertyProperty); }
            set { SetValue(ImagePropertyProperty, value); }
        }

        public static readonly DependencyProperty ImagePropertyProperty = DependencyProperty.Register(nameof(ImageProperty), typeof(string), typeof(RestApiPage), new PropertyMetadata(string.Empty));

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


        public ICommand SampleSelectionChanged
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetSampleDataConfig();
                });
            }
        }
        #endregion



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>();
            SetSample1DataConfig();
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

                var config = new RestApiDataConfig()
                {
                    Url = new Uri(RestApiQuery),
                };

                var paginationConfig = GetPaginationConfig();
                if (paginationConfig != null)
                {
                    config.PaginationConfig = paginationConfig;
                }

                var parser = new RestApiSampleParser();
                parser.InitializeSample(RestApiMainRoot, TextProperty1, TextProperty2, ImageProperty);
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
                    ContinuationTokenIsUrl = TokenParameterType == RestApiTokenType.Url,
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
            Sample = DefaultSample;
        }


        public RestApiSampleType Sample
        {
            get { return (RestApiSampleType)GetValue(SampleProperty); }
            set { SetValue(SampleProperty, value); }
        }

        public static readonly DependencyProperty SampleProperty = DependencyProperty.Register(nameof(Sample), typeof(RestApiSampleType), typeof(RestApiPage), new PropertyMetadata(DefaultSample));

        private void SetSampleDataConfig()
        {
            switch (Sample)
            {
                default:
                case RestApiSampleType.NumericPaginationSample:
                    SetSample1DataConfig();
                    break;
                case RestApiSampleType.TokenAsParameterPaginationSample:
                    SetSample2DataConfig();
                    break;
                case RestApiSampleType.TokenAsUrlPaginationSample:
                    SetSample3DataConfig();
                    break;
                case RestApiSampleType.Custom:
                    SetCustomDataConfig();
                    break;
            }
        }
        private void SetSample1DataConfig()
        {
            RestApiQuery = "https://public-api.wordpress.com/rest/v1.1/sites/3584907/posts/";
            PaginationParameterType = PaginationParameterType.Numeric;
            PaginationParameterName = "page";
            InitialValue = 1;
            IncrementalValue = 1;
            ResponseTokenName = string.Empty;
            TokenParameterType = RestApiTokenType.Parameter;
            ItemsPerPageParameterName = "number";
            MaxRecordsParam = DefaultMaxRecordsParam;

            RestApiMainRoot = "posts";
            TextProperty1 = "title";
            TextProperty2 = "author.name";
            ImageProperty = "post_thumbnail.URL";
        }

        private void SetSample2DataConfig()
        {
            RestApiQuery = "https://public-api.wordpress.com/rest/v1.1/sites/3584907/posts/";
            PaginationParameterType = PaginationParameterType.Token;
            PaginationParameterName = "page_handle";
            InitialValue = 0;
            IncrementalValue = 0;
            ResponseTokenName = "meta.next_page";
            TokenParameterType = RestApiTokenType.Parameter;
            ItemsPerPageParameterName = "number";
            MaxRecordsParam = DefaultMaxRecordsParam;

            RestApiMainRoot = "posts";
            TextProperty1 = "title";
            TextProperty2 = "author.name";
            ImageProperty = "post_thumbnail.URL";
        }

        private void SetSample3DataConfig()
        {
            RestApiQuery = "https://graph.facebook.com/v2.5/8195378771/posts?fields=id,message,from,created_time,link,full_picture&access_token=351842111678417|74b187b46cf37a8ef6349b990bc039c2&";
            PaginationParameterType = PaginationParameterType.Token;
            PaginationParameterName = string.Empty;
            InitialValue = 0;
            IncrementalValue = 0;
            ResponseTokenName = "paging.next";
            TokenParameterType = RestApiTokenType.Url;
            ItemsPerPageParameterName = "limit";
            MaxRecordsParam = DefaultMaxRecordsParam;

            RestApiMainRoot = "data";
            TextProperty1 = "message";
            TextProperty2 = "from.name";
            ImageProperty = "full_picture";
        }

        private void SetCustomDataConfig()
        {
            RestApiQuery = string.Empty;
            PaginationParameterType = PaginationParameterType.None;
            PaginationParameterName = string.Empty;
            InitialValue = 0;
            IncrementalValue = 0;
            ResponseTokenName = string.Empty;
            TokenParameterType = RestApiTokenType.Parameter;
            ItemsPerPageParameterName = string.Empty;
            MaxRecordsParam = DefaultMaxRecordsParam;

            RestApiMainRoot = string.Empty;
            TextProperty1 = string.Empty;
            TextProperty2 = string.Empty;
            ImageProperty = string.Empty;
        }

        private void InitializeDataProvider()
        {
            restApiDataProvider = new RestApiDataProvider();
            rawDataProvider = new RestApiDataProvider();
        }
    }
}
