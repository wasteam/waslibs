using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.DataProviders;
using AppStudio.DataProviders.WordPress;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class WordPressSample : Page
    {
        private WordPressDataProvider _wordPressDataProvider;

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
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(WordPressSample), new PropertyMetadata(null));

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
            WordPressOrderBy orderBy = WordPressOrderBy.None;
            SortDirection sortDirection = SortDirection.Ascending;

            _wordPressDataProvider = new WordPressDataProvider();
            this.Items = new ObservableCollection<object>();
            
            var config = new WordPressDataConfig()
            {
                Query = wordPressQuery,
                QueryType = queryType,
                FilterBy = queryFilterBy,
                OrderBy = orderBy,
                SortDirection = sortDirection
            };           

            var items = await _wordPressDataProvider.LoadDataAsync(config, maxRecordsParam);   
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _wordPressDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }      
    }
}
