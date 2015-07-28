using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.LocalStorage;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public abstract class BaseLocalStorageControl : UserControl, INotifyPropertyChanged
    {
        private string _filePath;
        private LocalStorageDataProvider<LocalStorageDataSchema> _localStorageDataProvider;
        private string _localStorageError;
        private ObservableCollection<LocalStorageDataSchema> _localStorageItems;
        private string _localStorageJsonResult;

        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        public string LocalStorageError
        {
            get { return _localStorageError; }
            set { SetProperty(ref _localStorageError, value); }
        }

        public ObservableCollection<LocalStorageDataSchema> LocalStorageItems
        {
            get { return _localStorageItems; }
            set { SetProperty(ref _localStorageItems, value); }
        }

        public string LocalStorageJsonResult
        {
            get { return _localStorageJsonResult; }
            set { SetProperty(ref _localStorageJsonResult, value); }
        }
        public BaseLocalStorageControl()
        {
            FilePath = "/Assets/Data/StaticStorage.json";
            LocalStorageItems = new ObservableCollection<LocalStorageDataSchema>();
        }
        protected async void LocalStorageRequestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LocalStorageError = string.Empty;
                LocalStorageJsonResult = string.Empty;
                LocalStorageItems.Clear();

                _localStorageDataProvider = new LocalStorageDataProvider<LocalStorageDataSchema>();
                var items = await _localStorageDataProvider.LoadDataAsync(new LocalStorageDataConfig { FilePath = FilePath });
                foreach (var item in items)
                {
                    LocalStorageItems.Add(item);
                }
                var uri = new Uri(string.Format("ms-appx://{0}", FilePath));
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();
                using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
                {
                    LocalStorageJsonResult = await r.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                LocalStorageError += ex.Message;
                LocalStorageError += ex.StackTrace;
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