using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Cache;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Samples.Extensions;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Tools", Name = "AppCache")]
    public sealed partial class AppCachePage : SamplePage
    {
        private bool _isBusy = false;
        public AppCachePage()
        {
            this.InitializeComponent();

            this.Options = new ObservableCollection<Option>();
            this.Options.Add(new Option() { Text = this.GetResourceString("AppCacheAddItem"), Symbol = Symbol.Add, Command = AddItemCommand });
            this.Options.Add(new Option() { Text = this.GetResourceString("AppCacheCleanMemory"), Symbol = Symbol.Delete, Command = CleanMemoryCommand });
            this.Options.Add(new Option() { Text = this.GetResourceString("AppCacheLoadFromCache"), Symbol = Symbol.Sync, Command = LoadFromCacheCommand });
            this.Options.Add(new Option() { Text = this.GetResourceString("AppCacheSaveToCache"), Symbol = Symbol.Save, Command = SaveMemoryToCacheCommand });
            this.Options.Add(new Option() { Text = this.GetResourceString("AppCacheCleanCache"), Symbol = Symbol.Clear, Command = CleanCacheCommand });

            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "App Cache"; }
        }

        #region Items
        public ObservableCollection<DeviceDataItem> Items
        {
            get { return (ObservableCollection<DeviceDataItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<DeviceDataItem>), typeof(AppCachePage), new PropertyMetadata(null));
        #endregion


        #region Options
        public ObservableCollection<Option> Options
        {
            get { return (ObservableCollection<Option>)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(ObservableCollection<Option>), typeof(AppCachePage), new PropertyMetadata(null));
        #endregion

        #region MemoryItems
        public ObservableCollection<DeviceDataItem> MemoryItems
        {
            get { return (ObservableCollection<DeviceDataItem>)GetValue(MemoryItemsProperty); }
            set { SetValue(MemoryItemsProperty, value); }
        }

        public static readonly DependencyProperty MemoryItemsProperty = DependencyProperty.Register("MemoryItems", typeof(ObservableCollection<DeviceDataItem>), typeof(AppCachePage), new PropertyMetadata(null));
        #endregion

        #region ItemsInMemory
        public int ItemsInMemory
        {
            get { return (int)GetValue(ItemsInMemoryProperty); }
            set { SetValue(ItemsInMemoryProperty, value); }
        }

        public static readonly DependencyProperty ItemsInMemoryProperty = DependencyProperty.Register("ItemsInMemory", typeof(int), typeof(AppCachePage), new PropertyMetadata(0));
        #endregion

        #region ItemsInCache
        public int ItemsInCache
        {
            get { return (int)GetValue(ItemsInCacheProperty); }
            set { SetValue(ItemsInCacheProperty, value); }
        }

        public static readonly DependencyProperty ItemsInCacheProperty = DependencyProperty.Register("ItemsInCache", typeof(int), typeof(AppCachePage), new PropertyMetadata(0));
        #endregion

        #region LastAction
        public string LastAction
        {
            get { return (string)GetValue(LastActionProperty); }
            set { SetValue(LastActionProperty, value); }
        }

        public static readonly DependencyProperty LastActionProperty = DependencyProperty.Register("LastAction", typeof(string), typeof(AppCachePage), new PropertyMetadata(string.Empty));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AppCachePage), new PropertyMetadata(null));
        #endregion


        #region Commands
        private bool CanExecute() => !_isBusy;

        public ICommand AddItemCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var randomITem = Items[new Random().Next(0, Items.Count - 1)];
                    MemoryItems.Add(randomITem);
                    LastAction = "Item added in memory";
                    ItemsInMemory++;
                });
            }
        }

        public ICommand CleanMemoryCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    await MemoryItems.ClearAsync();
                    ItemsInMemory = 0;
                    LastAction = "Memory cleaned";
                    _isBusy = false;
                }, CanExecute);
            }
        }

        public ICommand LoadFromCacheCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var cacheData = await AppCache.GetItemsAsync<DeviceDataItem>("AppCacheSample");
                    if (cacheData != null && cacheData.Items != null)
                    {
                        _isBusy = true;
                        await MemoryItems.ClearAsync();
                        await MemoryItems.CloneAsync(new ObservableCollection<DeviceDataItem>(cacheData.Items));
                        ItemsInMemory = MemoryItems.Count;
                        LastAction = "Memory loaded from cache";
                        _isBusy = false;
                    }
                }, CanExecute);
            }
        }

        public ICommand CleanCacheCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await AppCache.AddItemsAsync<DeviceDataItem>("AppCacheSample", new CachedContent<DeviceDataItem>() { Items = new ObservableCollection<DeviceDataItem>() });
                    ItemsInCache = 0;
                    LastAction = "Cache cleaned";
                });
            }
        }

        public ICommand SaveMemoryToCacheCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await AppCache.AddItemsAsync<DeviceDataItem>("AppCacheSample", new CachedContent<DeviceDataItem>() { Items = MemoryItems });
                    ItemsInCache = MemoryItems.Count;
                    LastAction = "Memory saved to cache";
                });
            }
        }
        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<DeviceDataItem>(new DevicesDataSource().GetItems());
            this.MemoryItems = new ObservableCollection<DeviceDataItem>();
            this.ItemsInMemory = MemoryItems.Count;
            var cacheData = await AppCache.GetItemsAsync<DeviceDataItem>("AppCacheSample");
            if (cacheData == null || cacheData.Items == null)
            {
                this.ItemsInCache = 0;
            }
            else
            {
                this.ItemsInCache = cacheData.Items.Count();
            }
            this.ItemTemplate = Resources["DeviceItemTemplate"] as DataTemplate;
            base.OnNavigatedTo(e);
        }
    }

    public class Option
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public Symbol Symbol { get; set; }
    }

    public static class CollectionExtensions
    {
        public static async Task ClearAsync(this ObservableCollection<DeviceDataItem> items, int millisecondsDelay = 100)
        {
            if (items != null && items.Count > 0)
            {
                while (items.Count > 0)
                {
                    await Task.Delay(millisecondsDelay);
                    items.RemoveAt(items.Count - 1);
                }
            }
        }
        public static async Task CloneAsync(this ObservableCollection<DeviceDataItem> items, ObservableCollection<DeviceDataItem> sourceItems, int millisecondsDelay = 100)
        {
            if (sourceItems != null && sourceItems.Count > 0)
            {
                int index = 0;
                while (index < sourceItems.Count)
                {
                    await Task.Delay(millisecondsDelay);
                    items.Add(sourceItems[index]);
                    index++;
                }
            }
        }
    }
}
