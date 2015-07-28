using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Flickr;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseFlickrControl : UserControl, INotifyPropertyChanged
    {
        private string _flickrError;
        private ObservableCollection<FlickrSchema> _flickrItems;
        private string _flickrQueryParam;
        private IList<FlickrQueryType> _flickrQueryTypeComboItems;
        private FlickrQueryType _flickrQueryTypeSelectedItem;
        private string _flickrXmlResult;
        private HttpClient client = new HttpClient();
        private FlickrDataProvider flickrDataProvider;

        public string FlickrError
        {
            get { return _flickrError; }
            set { SetProperty(ref _flickrError, value); }
        }

        public ObservableCollection<FlickrSchema> FlickrItems
        {
            get { return _flickrItems; }
            set { SetProperty(ref _flickrItems, value); }
        }

        public string FlickrQuery
        {
            get { return string.Format("http://api.flickr.com/services/feeds/photos_public.gne?{0}={1}", FlickrQueryTypeSelectedItem, FlickrQueryParam); }
        }

        public string FlickrQueryParam
        {
            get { return _flickrQueryParam; }
            set { SetProperty(ref _flickrQueryParam, value); OnPropertyChanged("FlickrQuery"); }
        }

        public IList<FlickrQueryType> FlickrQueryTypeComboItems
        {
            get
            {
                if (_flickrQueryTypeComboItems == null)
                {
                    _flickrQueryTypeComboItems = new List<FlickrQueryType>() { FlickrQueryType.Tags, FlickrQueryType.Id };
                }
                return _flickrQueryTypeComboItems;
            }
        }

        public FlickrQueryType FlickrQueryTypeSelectedItem
        {
            get { return _flickrQueryTypeSelectedItem; }
            set
            {
                SetProperty(ref _flickrQueryTypeSelectedItem, value);
                OnPropertyChanged("FlickrQuery");
            }
        }

        public string FlickrXmlResult
        {
            get { return _flickrXmlResult; }
            set { SetProperty(ref _flickrXmlResult, value); }
        }

        public BaseFlickrControl()
        {
            FlickrQueryParam = "Seatle";
            FlickrItems = new ObservableCollection<FlickrSchema>();
            FlickrQueryTypeSelectedItem = FlickrQueryTypeComboItems[0];
        }

        protected async void FlickrRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                FlickrError = string.Empty;
                FlickrItems.Clear();
                FlickrXmlResult = await client.GetStringAsync(new Uri(FlickrQuery, UriKind.Absolute));
                flickrDataProvider = new FlickrDataProvider();
                var items = await flickrDataProvider.LoadDataAsync(new FlickrDataConfig() { Query = FlickrQueryParam, QueryType = FlickrQueryTypeSelectedItem });
                foreach (var item in items)
                {
                    FlickrItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                FlickrError += ex.Message;
                FlickrError += ex.StackTrace;
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