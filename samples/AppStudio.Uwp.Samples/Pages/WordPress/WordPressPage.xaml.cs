using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.WordPress;
using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "WordPress", Order = 50)]
    public sealed partial class WordPressPage : SamplePage
    {
        private const string DefaultWordPressQuery = "en.blog.wordpress.com";
        private const string DefaultWordPressQueryFilterBy = "";
        private const WordPressQueryType DefaultQueryType = WordPressQueryType.Posts;
        private const int DefaultMaxRecordsParam = 20;

        WordPressDataProvider wordPressDataProvider;
        WordPressDataProvider rawDataProvider;

        public WordPressPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
        }

        public override string Caption
        {
            get { return "WordPress Data Provider"; }
        }

        #region DataProvider Config    
        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register("MaxRecordsParam", typeof(int), typeof(WordPressPage), new PropertyMetadata(DefaultMaxRecordsParam));


        public string WordPressQuery
        {
            get { return (string)GetValue(WordPressQueryProperty); }
            set { SetValue(WordPressQueryProperty, value); }
        }

        public static readonly DependencyProperty WordPressQueryProperty = DependencyProperty.Register("WordPressQuery", typeof(string), typeof(WordPressPage), new PropertyMetadata(DefaultWordPressQuery));

        public string WordPressQueryFilterBy
        {
            get { return (string)GetValue(WordPressQueryFilterByProperty); }
            set { SetValue(WordPressQueryFilterByProperty, value); }
        }

        public static readonly DependencyProperty WordPressQueryFilterByProperty = DependencyProperty.Register("WordPressQueryFilterBy", typeof(string), typeof(WordPressPage), new PropertyMetadata(DefaultWordPressQueryFilterBy));


        public WordPressQueryType WordPressQueryTypeSelectedItem
        {
            get { return (WordPressQueryType)GetValue(WordPressQueryTypeSelectedItemProperty); }
            set { SetValue(WordPressQueryTypeSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty WordPressQueryTypeSelectedItemProperty = DependencyProperty.Register("WordPressQueryTypeSelectedItemProperty", typeof(WordPressQueryType), typeof(WordPressPage), new PropertyMetadata(DefaultQueryType));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(WordPressPage), new PropertyMetadata(null));
        #endregion      

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register("DataProviderRawData", typeof(string), typeof(WordPressPage), new PropertyMetadata(string.Empty));
        #endregion

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(WordPressPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register("NoItems", typeof(bool), typeof(WordPressPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(WordPressPage), new PropertyMetadata(false));

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
            AppShell.Current.Shell.ShowRightPane(new WordPressSettings() { DataContext = this });
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
                var config = new WordPressDataConfig()
                {
                    Query = WordPressQuery,
                    QueryType = WordPressQueryTypeSelectedItem,
                    FilterBy = WordPressQueryFilterBy
                };  
                var items = await wordPressDataProvider.LoadDataAsync(config, MaxRecordsParam);

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
               
                var config = new WordPressDataConfig()
                {
                    Query = WordPressQuery,
                    QueryType = WordPressQueryTypeSelectedItem,
                    FilterBy = WordPressQueryFilterBy
                };             

                var items = await wordPressDataProvider.LoadMoreDataAsync();

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
            WordPressQuery = DefaultWordPressQuery;
            WordPressQueryFilterBy = DefaultWordPressQueryFilterBy;
            WordPressQueryTypeSelectedItem = DefaultQueryType;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }

        private void InitializeDataProvider()
        {
            wordPressDataProvider = new WordPressDataProvider();
            rawDataProvider = new WordPressDataProvider();
        }
    }
}
