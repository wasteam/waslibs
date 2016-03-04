using AppStudio.DataProviders.WordPress;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class WordPressSample : Page
    {
        public WordPressSample()
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(WordPressSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public async void GetItems()
        {
            string wordPressQuery = "en.blog.wordpress.com";
            WordPressQueryType queryType = WordPressQueryType.Posts;
            string queryFilterBy = string.Empty;
            int maxRecordsParam = 20;

            Items.Clear();
            var wordPressDataProvider = new WordPressDataProvider();
            var config = new WordPressDataConfig()
            {
                Query = wordPressQuery,
                QueryType = queryType,
                FilterBy = queryFilterBy
            };           

            var items = await wordPressDataProvider.LoadDataAsync(config, maxRecordsParam);           

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
