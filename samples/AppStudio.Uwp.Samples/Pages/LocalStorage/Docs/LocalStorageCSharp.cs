using AppStudio.DataProviders.LocalStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class LocalStorageSample : Page
    {
        public LocalStorageSample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty
            .Register("Items", typeof(ObservableCollection<object>), typeof(LocalStorageSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {
            string localStorageQuery = "/Assets/LocalStorageSamples.json";
            int maxRecordsParam = 10;

            Items.Clear();
            var localStorageDataProvider = new LocalStorageDataProvider<LocalStorageDataSchema>();         
            var config = new LocalStorageDataConfig { FilePath = localStorageQuery };       

            var items = await localStorageDataProvider.LoadDataAsync(config, maxRecordsParam);           

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
