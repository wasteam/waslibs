using System;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.RestApi;
using AppStudio.DataProviders;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class RestApiSample : Page
    {
        public RestApiSample()
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(RestApiSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetItemsTokenPagination();
            GetItemsNumericPagination();
        }

        public async void GetItemsTokenPagination()
        {
            string endPoint = "http://MyRestApiEndPoint.com";
            var maxRecordsParam = 20;
            var paginationParameterName = "token";
            var itemsPerPageParameterName = "number";
            var responseTokenName = "next_page";


            var paginationConfig = new TokenPagination()
            {
                PageSizeParameterName = paginationParameterName,
                PaginationParameterName = itemsPerPageParameterName,
                ContinuationTokenIsUrl = false,
                ContinuationTokenPath = responseTokenName
            };

            var config = new RestApiDataConfig()
            {
                Url = new Uri(endPoint),
                PaginationConfig = paginationConfig
            };

            var parser = new JsonParser<MySchema>();
            var dataProvider = new RestApiDataProvider();
            var items = await dataProvider.LoadDataAsync(config, maxRecordsParam, parser);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async void GetItemsNumericPagination()
        {
            string endPoint = "http://MyRestApiEndPoint.com";
            var maxRecordsParam = 20;
            var paginationParameterName = "page";
            var itemsPerPageParameterName = "number";


            var paginationConfig = new NumericPagination()
            {
                PageSizeParameterName = itemsPerPageParameterName,
                PaginationParameterName = paginationParameterName,
                ContinuationTokenInitialValue = "1",
                IncrementalValue = 1
            };

            var config = new RestApiDataConfig()
            {
                Url = new Uri(endPoint),
                PaginationConfig = paginationConfig
            };

            var parser = new JsonParser<MySchema>();
            var dataProvider = new RestApiDataProvider();
            var items = await dataProvider.LoadDataAsync(config, maxRecordsParam, parser);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }

    public class MySchema : SchemaBase
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
