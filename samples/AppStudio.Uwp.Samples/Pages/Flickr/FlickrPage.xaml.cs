using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Flickr;
using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Flickr", Order = 30)]
    public sealed partial class FlickrPage : SamplePage
    {
        private const string DefaultFlickrQueryParam = "Abstract";
        private const FlickrQueryType DefaultQueryType = FlickrQueryType.Tags;
        private const int DefaultMaxRecordsParam = 12;
        FlickrDataProvider flickrDataProvider;

        public FlickrPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            flickrDataProvider = new FlickrDataProvider();
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

        public static readonly DependencyProperty FlickrQueryParamProperty = DependencyProperty.Register("FlickrQueryParam", typeof(string), typeof(FlickrPage), new PropertyMetadata(DefaultFlickrQueryParam));


        public FlickrQueryType FlickrQueryTypeSelectedItem
        {
            get { return (FlickrQueryType)GetValue(FlickrQueryTypeSelectedItemProperty); }
            set { SetValue(FlickrQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty FlickrQueryTypeSelectedItemProperty = DependencyProperty.Register("FlickrQueryTypeSelectedItem", typeof(FlickrQueryType), typeof(FlickrPage), new PropertyMetadata(DefaultQueryType));


        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register("MaxRecordsParam", typeof(int), typeof(FlickrPage), new PropertyMetadata(DefaultMaxRecordsParam));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(FlickrPage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(FlickrPage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(FlickrPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register("NoItems", typeof(bool), typeof(FlickrPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(FlickrPage), new PropertyMetadata(false));

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

                flickrDataProvider = new FlickrDataProvider();
                var config = new FlickrDataConfig
                {
                    Query = FlickrQueryParam,
                    QueryType = FlickrQueryTypeSelectedItem
                };

                //var rawParser = new RawParser();
                //var rawData = await flickrDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                //DataProviderRawData = rawData.FirstOrDefault()?.Raw;

                var items = await flickrDataProvider.LoadDataAsync(config, MaxRecordsParam);

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

                var config = new FlickrDataConfig
                {
                    Query = FlickrQueryParam,
                    QueryType = FlickrQueryTypeSelectedItem
                };

                //var rawParser = new RawParser();
                //var rawData = await flickrDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                //DataProviderRawData = rawData.FirstOrDefault()?.Raw;

                var items = await flickrDataProvider.LoadMoreDataAsync();

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
            FlickrQueryParam = DefaultFlickrQueryParam;
            FlickrQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }
    }
}
