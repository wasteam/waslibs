using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Rss;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseRssControl : UserControl, INotifyPropertyChanged
    {
        private string _rssError;
        private ObservableCollection<RssSchema> _rssItems;
        private string _rssXmlResult;
        private HttpClient client = new HttpClient();
        private RssDataProvider RssDataProvider;

        public string RssError
        {
            get { return _rssError; }
            set { SetProperty(ref _rssError, value); }
        }

        public ObservableCollection<RssSchema> RssItems
        {
            get { return _rssItems; }
            set { SetProperty(ref _rssItems, value); }
        }

        private string _rssQuery;
        public string RssQuery
        {
            get { return _rssQuery; }
            set { SetProperty(ref _rssQuery, value); }
        }

        public string RssXmlResult
        {
            get { return _rssXmlResult; }
            set { SetProperty(ref _rssXmlResult, value); }
        }

        public BaseRssControl()
        {
            RssItems = new ObservableCollection<RssSchema>();
            _rssQuery = string.Format("http://www.blogger.com/feeds/6781693/posts/default?start-index=26&amp;max-results=25&amp;redirect=false");
        }
        protected async void RssRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RssError = string.Empty;
                RssItems.Clear();
                RssDataProvider = new RssDataProvider();

                var items = await RssDataProvider.LoadDataAsync(new RssDataConfig { Url = new Uri(RssQuery, UriKind.Absolute) });
                foreach (var item in items)
                {
                    RssItems.Add(item);
                }
                RssXmlResult = await client.GetStringAsync(new Uri(RssQuery, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                RssError += ex.Message;
                RssError += ex.StackTrace;
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