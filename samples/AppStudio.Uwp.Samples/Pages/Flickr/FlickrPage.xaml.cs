using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Flickr;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Flickr", Order = 30)]
    public sealed partial class FlickrPage : SamplePage
    {
        private const string DefaultFlickrQueryParam = "Abstract";
        private const FlickrQueryType DefaultQueryType = FlickrQueryType.Tags;
        private const int DefaultMaxRecordsParam = 12;
        private const FlickrSampleOrderBy DefaultOrderBy = FlickrSampleOrderBy.None;
        private const SortDirection DefaultSortDirection = SortDirection.Ascending;

        FlickrDataProvider flickrDataProvider;
        FlickrDataProvider rawDataProvider;

        public FlickrPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
        }

        public override string Caption
        {
            get { return "Flickr Data Provider"; }
        }

        #region DataProvider Config

        public string FlickrQueryParam
        {
            get { return (string)GetValue(FlickrQueryParamProperty); }
            set { SetValue(FlickrQueryParamProperty, value); }
        }

        public static readonly DependencyProperty FlickrQueryParamProperty = DependencyProperty.Register(nameof(FlickrQueryParam), typeof(string), typeof(FlickrPage), new PropertyMetadata(DefaultFlickrQueryParam));


        public FlickrQueryType FlickrQueryTypeSelectedItem
        {
            get { return (FlickrQueryType)GetValue(FlickrQueryTypeSelectedItemProperty); }
            set { SetValue(FlickrQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty FlickrQueryTypeSelectedItemProperty = DependencyProperty.Register(nameof(FlickrQueryTypeSelectedItem), typeof(FlickrQueryType), typeof(FlickrPage), new PropertyMetadata(DefaultQueryType));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register(nameof(MaxRecordsParam), typeof(int), typeof(FlickrPage), new PropertyMetadata(DefaultMaxRecordsParam));

        public FlickrSampleOrderBy OrderBy
        {
            get { return (FlickrSampleOrderBy)GetValue(OrderByProperty); }
            set { SetValue(OrderByProperty, value); }
        }

        public static readonly DependencyProperty OrderByProperty = DependencyProperty.Register(nameof(OrderBy), typeof(FlickrSampleOrderBy), typeof(FlickrPage), new PropertyMetadata(DefaultOrderBy));

        public SortDirection SortDirection
        {
            get { return (SortDirection)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(SortDirection), typeof(FlickrPage), new PropertyMetadata(DefaultSortDirection));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(FlickrPage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register(nameof(DataProviderRawData), typeof(string), typeof(FlickrPage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(FlickrPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof(NoItems), typeof(bool), typeof(FlickrPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(FlickrPage), new PropertyMetadata(false));

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
            AppShell.Current.Shell.ShowRightPane(new FlickrSettings() { DataContext = this });
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

                var config = new FlickrDataConfig
                {
                    Query = FlickrQueryParam,
                    QueryType = FlickrQueryTypeSelectedItem,
                    OrderBy = OrderBy != FlickrSampleOrderBy.None? OrderBy.ToString():string.Empty,
                    OrderDirection = SortDirection
                };

                var items = await flickrDataProvider.LoadDataAsync(config, MaxRecordsParam);

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

                var items = await flickrDataProvider.LoadMoreDataAsync();

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
            FlickrQueryParam = DefaultFlickrQueryParam;
            FlickrQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;
            OrderBy = DefaultOrderBy;
            SortDirection = DefaultSortDirection;
        }

        private void InitializeDataProvider()
        {
            flickrDataProvider = new FlickrDataProvider();
            rawDataProvider = new FlickrDataProvider();
        }
    }
}
