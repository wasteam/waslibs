using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Twitter;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseTwitterControl : UserControl, INotifyPropertyChanged
    {
        private string _accessToken;
        private string _accessTokenSecret;
        private string _consumerKey;
        private string _consumerSecret;
        private string _twitterError;
        private ObservableCollection<TwitterSchema> _twitterItems;
        private string _twitterQueryParam;
        private IList<TwitterQueryType> _twitterQueryTypeComboItems;
        private TwitterQueryType _twitterQueryTypeSelectedItem;
        private HttpClient client = new HttpClient();
        private TwitterDataProvider twitterDataProvider;

        public string AccessToken
        {
            get { return _accessToken; }
            set { SetProperty(ref _accessToken, value); }
        }

        public string AccessTokenSecret
        {
            get { return _accessTokenSecret; }
            set { SetProperty(ref _accessTokenSecret, value); }
        }

        public string ConsumerKey
        {
            get { return _consumerKey; }
            set { SetProperty(ref _consumerKey, value); }
        }

        public string ConsumerSecret
        {
            get { return _consumerSecret; }
            set { SetProperty(ref _consumerSecret, value); }
        }

        public string TwitterError
        {
            get { return _twitterError; }
            set { SetProperty(ref _twitterError, value); }
        }

        public ObservableCollection<TwitterSchema> TwitterItems
        {
            get { return _twitterItems; }
            set { SetProperty(ref _twitterItems, value); }
        }

        public string TwitterQuery
        {
            get
            {
                switch (TwitterQueryTypeSelectedItem)
                {
                    case TwitterQueryType.Home:
                        return "https://api.twitter.com/1.1/statuses/home_timeline.json";

                    case TwitterQueryType.User:
                        return string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}", TwitterQueryParam);

                    case TwitterQueryType.Search:
                        return string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}", TwitterQueryParam);

                    default:
                        return "https://api.twitter.com/1.1/statuses/home_timeline.json";
                }
            }
        }
        public void GoBackButtonClick(object sender, RoutedEventArgs e)
        {
            App.GoBack();
        }

        public string TwitterQueryParam
        {
            get { return _twitterQueryParam; }
            set { SetProperty(ref _twitterQueryParam, value); OnPropertyChanged("TwitterQuery"); }
        }

        public IList<TwitterQueryType> TwitterQueryTypeComboItems
        {
            get
            {
                if (_twitterQueryTypeComboItems == null)
                {
                    _twitterQueryTypeComboItems = new List<TwitterQueryType>() { TwitterQueryType.Search, TwitterQueryType.Home, TwitterQueryType.User };
                }
                return _twitterQueryTypeComboItems;
            }
        }

        public TwitterQueryType TwitterQueryTypeSelectedItem
        {
            get { return _twitterQueryTypeSelectedItem; }
            set
            {
                SetProperty(ref _twitterQueryTypeSelectedItem, value);
                OnPropertyChanged("TwitterQuery");
            }
        }
        public BaseTwitterControl()
        {
            TwitterQueryTypeSelectedItem = TwitterQueryTypeComboItems[0];
            TwitterQueryParam = "WindowsAppStudio";
            ConsumerKey = "29TPMHBW0QcFIWvNrfWxUGmlV";
            ConsumerSecret = "7cp8HDzES42iAFGgE5yxJ3wAxsrDdu5uEHwhoOKPlN6Q2P8k6s";
            AccessToken = "275442106-OdbhPuGr8biRdQsHbtzNSMVvHRrX9acsLbiyYgCF";
            AccessTokenSecret = "GA4Uw2sMgvSayjWTw9qdejB8LzNfNS2cAaQPimVDVhdIP";
            TwitterItems = new ObservableCollection<TwitterSchema>();
        }

        protected async void TwitterRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TwitterError = string.Empty;
                TwitterItems.Clear();
                twitterDataProvider = new TwitterDataProvider(new TwitterOAuthTokens { AccessToken = AccessToken, AccessTokenSecret = AccessTokenSecret, ConsumerKey = ConsumerKey, ConsumerSecret = ConsumerSecret });
                var config = new TwitterDataConfig
                {
                    Query = TwitterQueryParam,
                    QueryType = TwitterQueryTypeSelectedItem
                };

                var items = await twitterDataProvider.LoadDataAsync(config);
                foreach (var item in items)
                {
                    TwitterItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                TwitterError += ex.Message;
                TwitterError += ex.StackTrace;
            }
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