using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders.YouTube;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class YouTubeSample : Page
    {
        public YouTubeSample()
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(VariableSizedGridSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public void GetItems()
        {
            string ApiKey = "YourApiKey";
            string QueryParam = @"MicrosoftLumia";
            YouTubeQueryType QueryType = YouTubeQueryType.Channels;
            int MaxRecordsParam = 20;

            this.Items = new ObservableCollection<object>();
            var _youTubeDataProvider = new YouTubeDataProvider(new YouTubeOAuthTokens { ApiKey = ApiKey });
            var config = new YouTubeDataConfig
            {
                Query = QueryParam,
                QueryType = QueryType
            };

            var items = await _youTubeDataProvider.LoadDataAsync(config, MaxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

    }
}
