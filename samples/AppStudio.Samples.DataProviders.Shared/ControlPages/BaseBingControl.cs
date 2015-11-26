using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Bing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using AppStudio.DataProviders.Core;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseBingControl : UserControl, INotifyPropertyChanged
    {
        private IList<BingCountry> _bingCountryComboItems;
        private BingCountry _bingCountrySelectedItem;
        private string _bingError;
        private ObservableCollection<BingSchema> _bingItems;
        private string _bingQueryParam;
        private string _bingXmlResult;
        private BingDataProvider bingDataProvider;
        private HttpClient client = new HttpClient();

        public IList<BingCountry> BingCountryComboItems
        {
            get
            {
                if (_bingCountryComboItems == null)
                {
                    _bingCountryComboItems = new List<BingCountry>(Enum.GetValues(typeof(BingCountry)).Cast<BingCountry>());
                }
                return _bingCountryComboItems;
            }
        }

        public BingCountry BingCountrySelectedItem
        {
            get { return _bingCountrySelectedItem; }
            set { SetProperty(ref _bingCountrySelectedItem, value); OnPropertyChanged("BingQuery"); }
        }

        public string BingError
        {
            get { return _bingError; }
            set { SetProperty(ref _bingError, value); }
        }

        public ObservableCollection<BingSchema> BingItems
        {
            get { return _bingItems; }
            set { SetProperty(ref _bingItems, value); }
        }

        public string BingQuery
        {
            get { return string.Format("http://www.bing.com/search?q={0}&loc:{1}&format=rss", BingQueryParam, BingCountrySelectedItem.GetStringValue() ); }
        }

        public string BingQueryParam
        {
            get { return _bingQueryParam; }
            set { SetProperty(ref _bingQueryParam, value); OnPropertyChanged("BingQuery"); }
        }

        public string BingXmlResult
        {
            get { return _bingXmlResult; }
            set { SetProperty(ref _bingXmlResult, value); }
        }

        private int _maxRecordsParam = 20;
        public int MaxRecordsParam
        {
            get { return _maxRecordsParam; }
            set { SetProperty(ref _maxRecordsParam, value); }
        }

        protected async void BingRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                BingError = string.Empty;
                BingItems.Clear();
                BingXmlResult = await client.GetStringAsync(new Uri(BingQuery, UriKind.Absolute));
                bingDataProvider = new BingDataProvider();
                var items = await bingDataProvider.LoadDataAsync(new BingDataConfig() { Query = BingQueryParam, Country = BingCountrySelectedItem }, MaxRecordsParam);
                foreach (var item in items)
                {
                    BingItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                BingError += ex.Message;
                BingError += ex.StackTrace;
            }
        }
        public void GoBackButtonClick(object sender, RoutedEventArgs e)
        {
            App.GoBack();
        }

        public BaseBingControl()
        {
            BingCountrySelectedItem = BingCountry.UnitedStates;
            BingQueryParam = "Windows App Studio";
            BingItems = new ObservableCollection<BingSchema>();
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