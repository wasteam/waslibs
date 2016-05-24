using System;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.RestApi;
using AppStudio.DataProviders;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class RestApiSample : Page
    {
        private RestApiDataProvider _dataProvider;

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


        public async void GetItemsTokenPagination()
        {
            string endPoint = "http://MyRestApiEndPoint.com";
            var maxRecordsParam = 20;
            var paginationParameterName = "token";
            var pageSizeParemeterName = "limit";
            var responseTokenName = "meta.next_token";


            var paginationConfig = new TokenPagination(paginationParameterName, responseTokenName, pageSizeParemeterName);

            var config = new RestApiDataConfig()
            {
                Url = new Uri(endPoint),
                PaginationConfig = paginationConfig
            };

            var parser = new JsonParser<MySchema>();
            _dataProvider = new RestApiDataProvider();
            var items = await _dataProvider.LoadDataAsync(config, maxRecordsParam, parser);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async void GetItemsPageNumberPagination()
        {
            string endPoint = "http://MyRestApiEndPoint.com";
            var maxRecordsParam = 20;
            var paginationParameterName = "page";
            var pageSizeParemeterName = "limit";

            var paginationConfig = new PageNumberPagination(paginationParameterName, false, pageSizeParemeterName);

            var config = new RestApiDataConfig()
            {
                Url = new Uri(endPoint),
                PaginationConfig = paginationConfig
            };

            var parser = new JsonParser<MySchema>();
            _dataProvider = new RestApiDataProvider();
            var items = await _dataProvider.LoadDataAsync(config, maxRecordsParam, parser);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async void GetItemsItemOffsetPagination()
        {
            string endPoint = "http://MyRestApiEndPoint.com";
            var maxRecordsParam = 20;
            var offsetParemeterName = "offset";
            var pageSizeParemeterName = "limit";


            var paginationConfig = new ItemOffsetPagination(offsetParemeterName, true, pageSizeParemeterName, maxRecordsParam);

            var config = new RestApiDataConfig()
            {
                Url = new Uri(endPoint),
                PaginationConfig = paginationConfig
            };

            var parser = new JsonParser<MySchema>();
            _dataProvider = new RestApiDataProvider();
            var items = await _dataProvider.LoadDataAsync(config, maxRecordsParam, parser);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async void GetItemsNextUrlPagination()
        {
            string endPoint = "http://MyRestApiEndPoint.com";
            var maxRecordsParam = 20;
            var responseTokenName = "meta.next_page";
            var pageSizeParemeterName = "limit";


            var paginationConfig = new NextPageUrlPagination(responseTokenName, pageSizeParemeterName);

            var config = new RestApiDataConfig()
            {
                Url = new Uri(endPoint),
                PaginationConfig = paginationConfig
            };

            var parser = new JsonParser<MySchema>();
            _dataProvider = new RestApiDataProvider();
            var items = await _dataProvider.LoadDataAsync(config, maxRecordsParam, parser);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _dataProvider.LoadMoreDataAsync<MySchema>();

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
