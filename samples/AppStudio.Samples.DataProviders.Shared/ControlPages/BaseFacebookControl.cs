using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Facebook;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseFacebookControl : UserControl, INotifyPropertyChanged
    {
        private string _appId;
        private string _appSecret;
        private string _facebookError;
        private ObservableCollection<FacebookSchema> _facebookItems;
        private string _facebookJsonResult;
        private string _facebookQueryParam;
        private HttpClient client = new HttpClient();
        private FacebookDataProvider FacebookDataProvider;

        public string AppId
        {
            get { return _appId; }
            set { SetProperty(ref _appId, value); }
        }

        public string AppSecret
        {
            get { return _appSecret; }
            set { SetProperty(ref _appSecret, value); }
        }

        public string FacebookError
        {
            get { return _facebookError; }
            set { SetProperty(ref _facebookError, value); }
        }

        public ObservableCollection<FacebookSchema> FacebookItems
        {
            get { return _facebookItems; }
            set { SetProperty(ref _facebookItems, value); }
        }

        public string FacebookJsonResult
        {
            get { return _facebookJsonResult; }
            set { SetProperty(ref _facebookJsonResult, value); }
        }

        public string FacebookQuery
        {
            get
            {
                return string.Format(@"https://graph.facebook.com/v2.2/{0}/posts?&access_token={1}|{2}", FacebookQueryParam, AppId, AppSecret);
            }
        }

        public string FacebookQueryParam
        {
            get { return _facebookQueryParam; }
            set { SetProperty(ref _facebookQueryParam, value); OnPropertyChanged("FacebookQuery"); }
        }

        private int _maxRecordsParam = 20;
        public int MaxRecordsParam
        {
            get { return _maxRecordsParam; }
            set { SetProperty(ref _maxRecordsParam, value); }
        }

        protected async void FacebookRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                FacebookError = string.Empty;
                FacebookItems.Clear();
                FacebookDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = AppId, AppSecret = AppSecret });

                var config = new FacebookDataConfig()
                {
                    UserId = FacebookQueryParam,
                };

                var items = await FacebookDataProvider.LoadDataAsync(config, MaxRecordsParam);
                foreach (var item in items)
                {
                    FacebookItems.Add(item);
                }
                FacebookJsonResult = await client.GetStringAsync(new Uri(FacebookQuery, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                FacebookError += ex.Message;
                FacebookError += ex.StackTrace;
            }
        }
        public void GoBackButtonClick(object sender, RoutedEventArgs e)
        {
            App.GoBack();
        }
        public BaseFacebookControl()
        {
            FacebookQueryParam = "8195378771";
            AppId = "351842111678417";
            AppSecret = "74b187b46cf37a8ef6349b990bc039c2";
            FacebookItems = new ObservableCollection<FacebookSchema>();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion INotifyPropertyChanged

    }
}