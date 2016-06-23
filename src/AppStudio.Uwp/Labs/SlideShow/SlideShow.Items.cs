using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Windows.UI.Xaml;

namespace AppStudio.Uwp.Labs
{
    partial class SlideShow
    {
        private List<object> _items = new List<object>();
        private object _itemsLock = new Object();

        #region ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is IEnumerable))
            {
                throw new ArgumentException("ItemsSource");
            }

            var control = d as SlideShow;

            control.DetachNotificationEvents(e.OldValue as INotifyCollectionChanged);
            control.AttachNotificationEvents(e.NewValue as INotifyCollectionChanged);

            control.ItemsSourceChanged(e.NewValue as IEnumerable);
        }

        private void AttachNotificationEvents(INotifyCollectionChanged notifyCollection)
        {
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += OnCollectionChanged;
            }
        }

        private void DetachNotificationEvents(INotifyCollectionChanged notifyCollection)
        {
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= OnCollectionChanged;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(SlideShow), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        private void ItemsSourceChanged(IEnumerable items)
        {
            lock (_itemsLock)
            {
                _items.Clear();
                foreach (var item in items)
                {
                    _items.Add(item);
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_itemsLock)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        _items.Clear();
                        break;
                    case NotifyCollectionChangedAction.Add:
                        int index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            AddItem(item, index++);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                        {
                            RemoveItem(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                    default:
                        break;
                }
            }
        }

        private void AddItem(object item, int index = -1)
        {
            index = index < 0 ? _items.Count : index;
            _items.Insert(index, item);
        }

        private void RemoveItem(object item)
        {
            _items.Remove(item);
        }
    }
}
