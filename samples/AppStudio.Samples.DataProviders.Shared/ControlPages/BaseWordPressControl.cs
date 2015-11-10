using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.WordPress;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseWordPressControl : UserControl, INotifyPropertyChanged
    {
        private string _wordPressError;
        private ObservableCollection<WordPressSchema> _wordPressItems;
        private string _wordPressQueryParam;
        private string _wordPressQueryFilterBy;
        private IList<WordPressQueryType> _wordPressQueryTypeComboItems;
        private WordPressQueryType _wordPressQueryTypeSelectedItem;
        private string _wordPressJsonResult;
        private HttpClient client = new HttpClient();
        private WordPressDataProvider wordPressDataProvider;

        public string WordPressError
        {
            get { return _wordPressError; }
            set { SetProperty(ref _wordPressError, value); }
        }

        public ObservableCollection<WordPressSchema> WordPressItems
        {
            get { return _wordPressItems; }
            set { SetProperty(ref _wordPressItems, value); }
        }

        public string WordPressQuery
        {
            get {
                switch (WordPressQueryTypeSelectedItem)
                {
                    case WordPressQueryType.Tag:
                      return $"https://public-api.wordpress.com/rest/v1.1/sites/{WordPressQueryParam}/posts/?tag={WordPressQueryFilterBy}";
                    case WordPressQueryType.Category:
                        return  $"https://public-api.wordpress.com/rest/v1.1/sites/{WordPressQueryParam}/posts/?category={WordPressQueryFilterBy}";
                    default:
                        return $"https://public-api.wordpress.com/rest/v1.1/sites/{WordPressQueryParam}/posts/";
                }              
            }
        }

        public string WordPressQueryParam
        {
            get { return _wordPressQueryParam; }
            set { SetProperty(ref _wordPressQueryParam, value); OnPropertyChanged("WordPressQuery"); }
        }
        public string WordPressQueryFilterBy
        {
            get { return _wordPressQueryFilterBy; }
            set { SetProperty(ref _wordPressQueryFilterBy, value); OnPropertyChanged("WordPressQuery"); }
        }

        public IList<WordPressQueryType> WordPressQueryTypeComboItems
        {
            get
            {
                if (_wordPressQueryTypeComboItems == null)
                {
                    _wordPressQueryTypeComboItems = new List<WordPressQueryType>() { WordPressQueryType.Posts, WordPressQueryType.Tag,WordPressQueryType.Category,  };
                }
                return _wordPressQueryTypeComboItems;
            }
        }

        public WordPressQueryType WordPressQueryTypeSelectedItem
        {
            get { return _wordPressQueryTypeSelectedItem; }
            set
            {
                SetProperty(ref _wordPressQueryTypeSelectedItem, value);
                OnPropertyChanged("WordPressQuery");
            }
        }

        public string WordPressJsonResult
        {
            get { return _wordPressJsonResult; }
            set { SetProperty(ref _wordPressJsonResult, value); }
        }

        public BaseWordPressControl()
        {
            WordPressQueryParam = "en.blog.wordpress.com";
            WordPressItems = new ObservableCollection<WordPressSchema>();
            WordPressQueryTypeSelectedItem = WordPressQueryTypeComboItems[0];
        }

        protected async void WordPressRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                WordPressError = string.Empty;
                WordPressItems.Clear();
                WordPressJsonResult = await client.GetStringAsync(new Uri(WordPressQuery, UriKind.Absolute));
                wordPressDataProvider = new WordPressDataProvider();
                var items = await wordPressDataProvider.LoadDataAsync(new WordPressDataConfig() { Query = WordPressQueryParam, QueryType = WordPressQueryTypeSelectedItem, FilterBy = WordPressQueryFilterBy });
                foreach (var item in items)
                {
                    WordPressItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                WordPressError += ex.Message;
                WordPressError += ex.StackTrace;
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