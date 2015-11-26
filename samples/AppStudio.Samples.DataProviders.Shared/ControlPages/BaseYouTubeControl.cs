using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.YouTube;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseYouTubeControl : UserControl, INotifyPropertyChanged
    {
        private string _apiKey;
        private YouTubeDataProvider _youTubeDataProvider;
        private string _youTubeError;
        private ObservableCollection<YouTubeSchema> _youTubeItems;
        private string _youTubeJsonResult;
        private string _youTubeQueryParam;
        private IList<YouTubeQueryType> _youTubeQueryTypeComboItems;
        private YouTubeQueryType _youTubeQueryTypeSelectedItem;
        private HttpClient client = new HttpClient();

        public string ApiKey
        {
            get { return _apiKey; }
            set { SetProperty(ref _apiKey, value); }
        }

        public string YouTubeError
        {
            get { return _youTubeError; }
            set { SetProperty(ref _youTubeError, value); }
        }

        public ObservableCollection<YouTubeSchema> YouTubeItems
        {
            get { return _youTubeItems; }
            set { SetProperty(ref _youTubeItems, value); }
        }

        public string YouTubeJsonResult
        {
            get { return _youTubeJsonResult; }
            set { SetProperty(ref _youTubeJsonResult, value); }
        }

        public string YouTubeQuery
        {
            get
            {
                switch (YouTubeQueryTypeSelectedItem)
                {
                    case YouTubeQueryType.Channels:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/channels?forUsername={0}&part=contentDetails&maxResults=1&key={1}", YouTubeQueryParam, ApiKey);

                    case YouTubeQueryType.Playlist:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/playlistItems?playlistId={0}&part=snippet&maxResults=20&key={1}", YouTubeQueryParam, ApiKey);

                    case YouTubeQueryType.Videos:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/search?q={0}&part=snippet&maxResults=20&key={1}&type=video", YouTubeQueryParam, ApiKey);

                    default:
                        return string.Format(@"https://www.googleapis.com/youtube/v3/search?q={0}&part=snippet&maxResults=20&key={1}&type=video", YouTubeQueryParam, ApiKey);
                }
            }
        }

        public string YouTubeQueryParam
        {
            get { return _youTubeQueryParam; }
            set { SetProperty(ref _youTubeQueryParam, value); OnPropertyChanged("YouTubeQuery"); }
        }

        public IList<YouTubeQueryType> YouTubeQueryTypeComboItems
        {
            get
            {
                if (_youTubeQueryTypeComboItems == null)
                {
                    _youTubeQueryTypeComboItems = new List<YouTubeQueryType>() { YouTubeQueryType.Channels, YouTubeQueryType.Playlist, YouTubeQueryType.Videos };
                }
                return _youTubeQueryTypeComboItems;
            }
        }

        public YouTubeQueryType YouTubeQueryTypeSelectedItem
        {
            get { return _youTubeQueryTypeSelectedItem; }
            set
            {
                SetProperty(ref _youTubeQueryTypeSelectedItem, value);
                OnPropertyChanged("YouTubeQuery");
            }
        }

        private int _maxRecordsParam = 20;
        public int MaxRecordsParam
        {
            get { return _maxRecordsParam; }
            set { SetProperty(ref _maxRecordsParam, value); }
        }

        public BaseYouTubeControl()
        {
            YouTubeQueryTypeSelectedItem = YouTubeQueryTypeComboItems[0];
            YouTubeQueryParam = @"MicrosoftLumia";
            ApiKey = "AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ";
            YouTubeItems = new ObservableCollection<YouTubeSchema>();
        }

        protected async void YouTubeRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                YouTubeError = string.Empty;
                YouTubeItems.Clear();
                _youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = ApiKey });
                var config = new YouTubeDataConfig
                {
                    Query = YouTubeQueryParam,
                    QueryType = YouTubeQueryTypeSelectedItem
                };

                var items = await _youTubeDataProvider.LoadDataAsync(config, MaxRecordsParam);
                foreach (var item in items)
                {
                    YouTubeItems.Add(item);
                }
                YouTubeJsonResult = await client.GetStringAsync(new Uri(YouTubeQuery, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                YouTubeError += ex.Message;
                YouTubeError += ex.StackTrace;
            }
        }
        public void GoBackButtonClick(object sender, RoutedEventArgs e)
        {
            App.GoBack();
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