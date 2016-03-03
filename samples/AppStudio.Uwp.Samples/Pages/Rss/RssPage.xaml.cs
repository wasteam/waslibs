using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Rss;
using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Rss")]
    public sealed partial class RssPage : SamplePage
    {
        private const int DefaultMaxRecordsParam = 20;
        private const string DefaultRssQuery = "http://www.blogger.com/feeds/6781693/posts/default";

        public RssPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
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

        #region RawData
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
                HasErrors = false;
                NoItems = false;
                DataProviderRawData = string.Empty;
                Items.Clear();
                var rssDataProvider = new RssDataProvider();
                var config = new RssDataConfig { Url = new Uri(RssQuery, UriKind.Absolute) };

                var rawParser = new RawParser();
                var rawData = await rssDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw?.ToString();

                var items = await rssDataProvider.LoadDataAsync(config, MaxRecordsParam);
                if (!items.Any())
                {
                    NoItems = true;
                }
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
        }

        private void RestoreConfig()
        {
            RssQuery = DefaultRssQuery;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }
    }
}
