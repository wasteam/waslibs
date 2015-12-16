using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Instagram;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseInstagramControl : UserControl, INotifyPropertyChanged
    {
        private string _clientId;
        private InstagramDataProvider _instagramDataProvider;
        private string _instagramError;
        private ObservableCollection<InstagramSchema> _instagramItems;
        private string _instagramJsonResult;
        private string _instagramQueryParam;
        private IList<InstagramQueryType> _instagramQueryTypeComboItems;
        private InstagramQueryType _instagramQueryTypeSelectedItem;
        private HttpClient client = new HttpClient();

        public string ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

        public string InstagramError
        {
            get { return _instagramError; }
            set { SetProperty(ref _instagramError, value); }
        }

        public ObservableCollection<InstagramSchema> InstagramItems
        {
            get { return _instagramItems; }
            set { SetProperty(ref _instagramItems, value); }
        }

        public string InstagramJsonResult
        {
            get { return _instagramJsonResult; }
            set { SetProperty(ref _instagramJsonResult, value); }
        }

        public string InstagramQuery
        {
            get
            {
                switch (InstagramQueryTypeSelectedItem)
                {
                    case InstagramQueryType.Tag:
                        return string.Format("https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}", InstagramQueryParam, ClientId);

                    case InstagramQueryType.Id:
                        return string.Format("https://api.instagram.com/v1/users/{0}/media/recent?client_id={1}", InstagramQueryParam, ClientId);

                    default:
                        return string.Format("https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}", InstagramQueryParam, ClientId);
                }
            }
        }

        public string InstagramQueryParam
        {
            get { return _instagramQueryParam; }
            set { SetProperty(ref _instagramQueryParam, value); OnPropertyChanged("InstagramQuery"); }
        }

        public IList<InstagramQueryType> InstagramQueryTypeComboItems
        {
            get
            {
                if (_instagramQueryTypeComboItems == null)
                {
                    _instagramQueryTypeComboItems = new List<InstagramQueryType>() { InstagramQueryType.Id, InstagramQueryType.Tag };
                }
                return _instagramQueryTypeComboItems;
            }
        }

        public InstagramQueryType InstagramQueryTypeSelectedItem
        {
            get { return _instagramQueryTypeSelectedItem; }
            set
            {
                SetProperty(ref _instagramQueryTypeSelectedItem, value);
                OnPropertyChanged("InstagramQuery");
            }
        }

        private int _maxRecordsParam = 20;
        public int MaxRecordsParam
        {
            get { return _maxRecordsParam; }
            set { SetProperty(ref _maxRecordsParam, value); }
        }

        public BaseInstagramControl()
        {
            InstagramQueryTypeSelectedItem = InstagramQueryTypeComboItems[0];
            InstagramQueryParam = @"256679330";
            ClientId = "4e9badaafa4e4436977733f01e05fbd0";
            InstagramItems = new ObservableCollection<InstagramSchema>();
        }
        protected async void InstagramRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                InstagramError = string.Empty;
                InstagramItems.Clear();
                _instagramDataProvider = new InstagramDataProvider(new InstagramOAuthTokens { ClientId = ClientId });

                var config = new InstagramDataConfig()
                {
                    Query = InstagramQueryParam,
                    QueryType = InstagramQueryTypeSelectedItem,
                };

                var items = await _instagramDataProvider.LoadDataAsync(config, MaxRecordsParam);
                foreach (var item in items)
                {
                    InstagramItems.Add(item);
                }
                InstagramJsonResult = await client.GetStringAsync(new Uri(InstagramQuery, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                InstagramError += ex.Message;
                InstagramError += ex.StackTrace;
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