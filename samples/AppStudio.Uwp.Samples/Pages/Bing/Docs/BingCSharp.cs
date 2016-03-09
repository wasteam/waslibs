using AppStudio.DataProviders.Bing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class BingSample : Page
    {
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(BingSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {           
            string bingQueryParam = "Windows App Studio";
            BingCountry bingCountrySelectedItem = BingCountry.UnitedStates;
            int maxRecordsParam = 20;
          
            Items.Clear();
            var bingDataProvider = new BingDataProvider();
            var config = new BingDataConfig() { Query = bingQueryParam, Country = bingCountrySelectedItem };

            var items = await bingDataProvider.LoadDataAsync(config, maxRecordsParam);          

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
