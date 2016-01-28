using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
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

            var control = d as Pivorama;

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

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Pivorama), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        private void ItemsSourceChanged(IEnumerable items)
        {
            if (_container != null)
            {
                int index = -1;
                ClearChildren();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        AddItem(item);
                        index = 0;
                    }
                }
                this.SelectedIndex = index;
                this.ArrangeItems();
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_container != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        ClearChildren();
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
                this.ArrangeItems();
            }
        }

        private void ClearChildren()
        {
            this.SelectedIndex = -1;
            _items.Clear();
        }

        private void AddItem(object item, int index = -1)
        {
            index = index < 0 ? _items.Count : index;
            _items.Insert(index, item);
            this.SelectedIndex = Math.Max(0, this.SelectedIndex);
        }

        private void RemoveItem(object item)
        {
            _items.Remove(item);
            this.SelectedIndex = Math.Min(_items.Count - 1, this.SelectedIndex);
        }

        private void BuildPanes(IEnumerable items)
        {
            if (items != null)
            {
                int count = 2;
                foreach (var item in items)
                {
                    count++;
                }

                if (_container != null)
                {
                    _container.Children.Clear();
                    for (int n = 0; n < count; n++)
                    {
                        var controlItem = new PivoramaItem
                        {
                            Index = n % (count - 2),
                            Items = _items,
                            TabTemplate = TabTemplate,
                            HeaderTemplate = HeaderTemplate,
                            ContentTemplate = ContentTemplate,
                            Width = ItemWidth,
                            HorizontalContentAlignment = HorizontalAlignment.Stretch,
                            VerticalContentAlignment = VerticalAlignment.Stretch,
                            UseLayoutRounding = false
                        };
                        controlItem.HeaderClick += OnItemHeaderClick;
                        controlItem.TabClick += OnTabClick;
                        _container.Children.Add(controlItem);
                        controlItem.MoveX(n);
                    }
                }
            }
        }

        private void ArrangeItems()
        {
            ArrangeItems(this.SelectedIndex);
        }
        private void ArrangeItems(int currentIndex)
        {
            if (_container != null)
            {
                var panes = _container.Children.Cast<PivoramaItem>().OrderBy(r => r.GetTranslateX()).ToArray();
                for (int n = 0; n < panes.Length; n++)
                {
                    if (_items.Count > 0)
                    {
                        int index = GetItemIndex(currentIndex, n);
                        // TODO: Remove
                        //panes[n].Index = index;

                        var item = _items[index];
                        if (panes[n].Content != item)
                        {
                            panes[n].Header = null;
                            panes[n].Header = item;
                            panes[n].Content = null;
                            panes[n].Content = item;
                        }
                    }
                    else
                    {
                        panes[n].Header = null;
                        panes[n].Content = null;
                    }
                }
            }
        }

        private int GetItemIndex(int currentIndex, int n)
        {
            int index = (currentIndex + n - 1);
            return index.Mod(_items.Count);
        }

        private IEnumerable<Point> GetPositions(double slotWidth)
        {
            double x0 = GetLeftBound();
            for (int n = 0; n < (_items.Count + 2); n++)
            {
                yield return new Point(x0, 0);
                x0 += slotWidth;
            }
        }

        private double GetLeftBound()
        {
            return -Math.Round(ItemWidth, 2);
        }

        private void OnItemHeaderClick(object sender, RoutedEventArgs e)
        {
            var item = sender as PivoramaItem;
            this.SelectedIndex = _items.IndexOf(item.Content);
        }

        private void OnTabClick(object sender, RoutedEventArgs e)
        {
            var item = sender as ContentControl;
            this.SelectedIndex = _items.IndexOf(item.Content);
        }
    }
}
