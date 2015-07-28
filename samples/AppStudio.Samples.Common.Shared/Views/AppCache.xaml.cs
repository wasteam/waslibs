using AppStudio.Common.Cache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppStudio.Samples.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppCache : Page
    {
        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get
            {
                if (_items == null) { _items = new ObservableCollection<string>(); }
                return _items;
            }
        }
        public AppCache()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Items.Clear();
            var cacheData = await AppStudio.Common.Cache.AppCache.GetItemsAsync<string>("AppCacheList");
            if (cacheData != null && cacheData.Items != null)
            {
                foreach (var item in cacheData.Items)
                {
                    Items.Add(item);
                }
            }            
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CachedContent<string> cacheData = new CachedContent<string>() { Items = Items };
            await AppStudio.Common.Cache.AppCache.AddItemsAsync<string>("AppCacheList", cacheData);
        }

        private void CleanListClick(object sender, RoutedEventArgs e)
        {
            Items.Clear();
        }

        private void AddToListItem(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemString.Text))
            {
                Items.Add(ItemString.Text);
                ItemString.Text = string.Empty;
            }
        }
    }
}
