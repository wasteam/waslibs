using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Facebook;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "DataProviders", Name = "Facebook", Order = 10)]
    public sealed partial class FacebookPage : SamplePage
    {       
        private const string DefaultFacebookQueryParam = "8195378771";
        private const int DefaultMaxRecordsParam = 20;

        FacebookDataProvider facebookDataProvider;
        FacebookDataProvider rawDataProvider;

        public FacebookPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;

            InitializeDataProvider();
        }

        public override string Caption
        {
            get { return "Facebook Data Provider"; }
        }

        #region DataProvider Config
        public string AppId
        {
            get { return (string)GetValue(AppIdProperty); }
            set { SetValue(AppIdProperty, value); }
        }

        public static readonly DependencyProperty AppIdProperty = DependencyProperty.Register(nameof(AppId), typeof(string), typeof(FacebookPage), new PropertyMetadata(DefaultAppId));

        public string AppSecret
        {
            get { return (string)GetValue(AppSecretProperty); }
            set { SetValue(AppSecretProperty, value); }
        }

        public static readonly DependencyProperty AppSecretProperty = DependencyProperty.Register(nameof(AppSecret), typeof(string), typeof(FacebookPage), new PropertyMetadata(DefaultAppSecret));


        public string FacebookQueryParam
        {
            get { return (string)GetValue(FacebookQueryParamProperty); }
            set { SetValue(FacebookQueryParamProperty, value); }
        }

        public static readonly DependencyProperty FacebookQueryParamProperty = DependencyProperty.Register(nameof(FacebookQueryParam), typeof(string), typeof(FacebookPage), new PropertyMetadata(DefaultFacebookQueryParam));

        public int MaxRecordsParam
        {
            get { return (int)GetValue(MaxRecordsParamProperty); }
            set { SetValue(MaxRecordsParamProperty, value); }
        }

        public static readonly DependencyProperty MaxRecordsParamProperty = DependencyProperty.Register(nameof(MaxRecordsParam), typeof(int), typeof(FacebookPage), new PropertyMetadata(DefaultMaxRecordsParam));

        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<object>), typeof(FacebookPage), new PropertyMetadata(null));

        #endregion        

        #region RawData
        public string DataProviderRawData
        {
            get { return (string)GetValue(DataProviderRawDataProperty); }
            set { SetValue(DataProviderRawDataProperty, value); }
        }

        public static readonly DependencyProperty DataProviderRawDataProperty = DependencyProperty.Register(nameof(DataProviderRawData), typeof(string), typeof(FacebookPage), new PropertyMetadata(string.Empty));

        #endregion    

        #region HasErrors
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(FacebookPage), new PropertyMetadata(false));
        #endregion

        #region NoItems
        public bool NoItems
        {
            get { return (bool)GetValue(NoItemsProperty); }
            set { SetValue(NoItemsProperty, value); }
        }
        public static readonly DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof(NoItems), typeof(bool), typeof(FacebookPage), new PropertyMetadata(false));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(FacebookPage), new PropertyMetadata(false));

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
            AppShell.Current.Shell.ShowRightPane(new FacebookSettings() { DataContext = this });
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
               
                var config = new FacebookDataConfig
                {
                    UserId = FacebookQueryParam
                };

                var items = await facebookDataProvider.LoadDataAsync(config, MaxRecordsParam);

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

                DataProviderRawData = string.Empty;

                var items = await facebookDataProvider.LoadMoreDataAsync();

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
            AppId = DefaultAppId;
            AppSecret = DefaultAppSecret;
            FacebookQueryParam = DefaultFacebookQueryParam;
            MaxRecordsParam = DefaultMaxRecordsParam;
        }

        private void InitializeDataProvider()
        {
            facebookDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = AppId, AppSecret = AppSecret });
            rawDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = AppId, AppSecret = AppSecret });
        }

        private const string DefaultAppId = "351842111678417";
        private const string DefaultAppSecret = "74b187b46cf37a8ef6349b990bc039c2";
    }
}
