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
        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(RssPage), new PropertyMetadata(null));
        #endregion

        #region DataProviderError
        public string DataProviderError
        {
            get { return (string)GetValue(DataProviderErrorProperty); }
            set { SetValue(DataProviderErrorProperty, value); }
        }

        public static readonly DependencyProperty DataProviderErrorProperty = DependencyProperty.Register("DataProviderError", typeof(string), typeof(RssPage), new PropertyMetadata(string.Empty));
        #endregion

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(RssPage), new PropertyMetadata(string.Empty));
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
                DataProviderError = string.Empty;
                Items.Clear();
                var rssDataProvider = new RssDataProvider();
                var config = new RssDataConfig { Url = new Uri("http://blogs.windows.com/windows/b/bloggingwindows/rss.aspx", UriKind.Absolute) };

                var items = await rssDataProvider.LoadDataAsync(config, MaxRecordsParam);
                foreach (var item in items)
                {
                    Items.Add(item);
                }

                var rawParser = new RawParser();
                var rawData = await rssDataProvider.LoadDataAsync(config, MaxRecordsParam, rawParser);
                DataProviderRawData = rawData.FirstOrDefault()?.Raw?.ToString();
            }
            catch (Exception ex)
            {
                DataProviderError += ex.Message;
                DataProviderError += ex.StackTrace;
            }
        }

        private void RestoreConfig()
        {

        }
    }
}
