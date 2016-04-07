using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.DataProviders.Rss;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Rss", Order = 60)]
    public sealed partial class RssPage : SamplePage
    {
        private const int DefaultMaxRecordsParam = 10;
        private const string DefaultRssQuery = "http://blogs.windows.com/windows/b/bloggingwindows/rss.aspx";

        RssDataProvider rssDataProvider;

        public RssPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
            rssDataProvider = new RssDataProvider();
        }

        public override string Caption
        {
            get { return "Rss Data Provider"; }
        }

        #region DataProvider Config    
        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register("MaxRecordsParam", typeof(int), typeof(RssPage), new PropertyMetadata(DefaultMaxRecordsParam));


        public string RssQuery
        {
            get { return (string)GetValue(RssQueryProperty); }
            set { SetValue(RssQueryProperty, value); }
        }

        public static readonly DependencyProperty RssQueryProperty = DependencyProperty.Register("RssQuery", typeof(string), typeof(RssPage), new PropertyMetadata(DefaultRssQuery));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(RssPage), new PropertyMetadata(null));
        #endregion      

        #region DataProviderRawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(RssPage), new PropertyMetadata(string.Empty));
        #endregion

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(RssPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register("NoItems", typeof(bool), typeof(RssPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(RssPage), new PropertyMetadata(false));

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
            AppShell.Current.Shell.ShowRightPane(new RssSettings() { DataContext = this });
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
                rssDataProvider = new RssDataProvider();
                var config = new RssDataConfig { Url = new Uri(RssQuery, UriKind.Absolute) };

                var rawParser = new RawParser();
                var rawData = await rssDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw;

                var items = await rssDataProvider.LoadDataAsync(config, MaxRecordsParam);

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

                var items = await rssDataProvider.LoadMoreDataAsync();

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
            RssQuery = DefaultRssQuery;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }
    }
}
