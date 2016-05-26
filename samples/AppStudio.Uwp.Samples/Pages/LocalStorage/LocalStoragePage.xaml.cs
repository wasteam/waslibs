using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "LocalStorage", Order = 80)]
    public sealed partial class LocalStoragePage : SamplePage
    {
        private const int DefaultMaxRecordsParam = 10;
        private const string DefaultLocalStorageQuery = "/Assets/Photos.json";
        private const LocalSampleOrderBy DefaultOrderBy = LocalSampleOrderBy.None;
        private const SortDirection DefaultSortDirection = SortDirection.Ascending;

        LocalStorageDataProvider<LocalStorageDataSchema> localStorageDataProvider;
        LocalStorageDataProvider<RawSchema> rawDataProvider;

        public LocalStoragePage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
        }

        public override string Caption
        {
            get { return "LocalStorage Data Provider"; }
        }

        #region DataProvider Config    
        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register(nameof(MaxRecordsParam), typeof(int), typeof(LocalStoragePage), new PropertyMetadata(DefaultMaxRecordsParam));


        public string LocalStorageQuery
        {
            get { return (string)GetValue(LocalStorageQueryProperty); }
            set { SetValue(LocalStorageQueryProperty, value); }
        }

        public static readonly DependencyProperty LocalStorageQueryProperty = DependencyProperty.Register(nameof(LocalStorageQuery), typeof(string), typeof(LocalStoragePage), new PropertyMetadata(DefaultLocalStorageQuery));

        public LocalSampleOrderBy OrderBy
        {
            get { return (LocalSampleOrderBy)GetValue(OrderByProperty); }
            set { SetValue(OrderByProperty, value); }
        }

        public static readonly DependencyProperty OrderByProperty = DependencyProperty.Register(nameof(OrderBy), typeof(LocalSampleOrderBy), typeof(LocalStoragePage), new PropertyMetadata(DefaultOrderBy));

        public SortDirection SortDirection
        {
            get { return (SortDirection)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(SortDirection), typeof(LocalStoragePage), new PropertyMetadata(DefaultSortDirection));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(LocalStoragePage), new PropertyMetadata(null));
        #endregion      

        #region DataProviderRawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register(nameof(DataProviderRawData), typeof(string), typeof(LocalStoragePage), new PropertyMetadata(string.Empty));
        #endregion

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(LocalStoragePage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof(NoItems), typeof(bool), typeof(LocalStoragePage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(LocalStoragePage), new PropertyMetadata(false));

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
            AppShell.Current.Shell.ShowRightPane(new LocalStorageSettings() { DataContext = this });
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

                var config = new LocalStorageDataConfig
                {
                    FilePath = LocalStorageQuery,
                    OrderBy = OrderBy.ToString(),
                    OrderDirection = SortDirection
                };

                var items = await localStorageDataProvider.LoadDataAsync(config, MaxRecordsParam);

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

                var items = await localStorageDataProvider.LoadMoreDataAsync();

                NoItems = !items.Any();

                foreach (var item in items)
                {
                    Items.Add(item);
                }

                var rawParser = new RawParser();
                var rawData = await rawDataProvider.LoadMoreDataAsync();
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
            LocalStorageQuery = DefaultLocalStorageQuery;
            MaxRecordsParam = DefaultMaxRecordsParam;
            OrderBy = DefaultOrderBy;
            SortDirection = DefaultSortDirection;
        }


        private void InitializeDataProvider()
        {
            localStorageDataProvider = new LocalStorageDataProvider<LocalStorageDataSchema>();
            rawDataProvider = new LocalStorageDataProvider<RawSchema>();
        }
    }
}
