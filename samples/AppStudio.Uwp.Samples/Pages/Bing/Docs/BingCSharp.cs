using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Bing;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class BingSample : Page
    {
        private BingDataProvider _bingDataProvider;

        public BingSample()
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
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(BingSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {           
            string bingQueryParam = "Windows App Studio";
            BingCountry bingCountrySelectedItem = BingCountry.UnitedStates;
            int maxRecordsParam = 20;          
           
            _bingDataProvider = new BingDataProvider();
            this.Items = new ObservableCollection<object>();

            var config = new BingDataConfig() { Query = bingQueryParam, Country = bingCountrySelectedItem };

            var items = await _bingDataProvider.LoadDataAsync(config, maxRecordsParam);  
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _bingDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
