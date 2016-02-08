using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private List<object> _items = new List<object>();

        #region Position
        internal double Position
        {
            get { return -_container.GetTranslateX(); }
            set
            {
                double position = value;

                if (position < 0)
                {
                    position = this.PanelWidth + position;
                }
                else
                {
                    position = position % this.PanelWidth;
                }

                if (_isInitialized && position != value)
                {
                    for (int n = 0; n < _container.Children.Count; n++)
                    {
                        double x = TransformX(position, n * ItemWidth);

                        var headerControl = _headerItems.Children[n] as PivoramaItem;
                        headerControl.MoveX(x);

                        var itemControl = _container.Children[n] as PivoramaItem;
                        itemControl.MoveX(x);
                    }
                }

                SetValue(PositionProperty, position);
            }
        }

        private static void PositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pivorama;
            control.MoveTo((double)e.NewValue);
        }

        private void MoveTo(double position)
        {
            _headerItems.TranslateX(-position);
            _container.TranslateX(-position);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(double), typeof(Pivorama), new PropertyMetadata(0.0, PositionChanged));
        #endregion

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
            if (_isInitialized)
            {
                foreach (var item in items)
                {
                    _items.Add(item);
                }

                Position = 0;

                this.BuildPanels(items);
                this.ArrangeTabs();
                this.RefreshLayout();
            }
        }

        private async void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

                this.BuildPanels(_items);
                this.ArrangeTabs();
                this.ArrangeItems();

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.RefreshLayout();
                });
            }
        }

        private void ClearChildren()
        {
            _items.Clear();
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

        private void BuildPanels(IEnumerable items)
        {
            _headerItems.Children.Clear();
            _container.Children.Clear();
            foreach (var item in items)
            {
                var headerControl = CreateHeader();
                headerControl.Width = this.ItemWidth;
                headerControl.Content = item;
                _headerItems.Children.Add(headerControl);

                var itemControl = CreateItem();
                itemControl.Width = this.ItemWidth;
                itemControl.Content = item;
                _container.Children.Add(itemControl);
            }
        }

        private void ArrangeTabs()
        {
            _tabItems.Children.Clear();

            int index = this.Index;
            for (int n = 0; n < _items.Count; n++)
            {
                int inx = (index + n) % _items.Count;
                var tabControl = new PivoramaTab
                {
                    Index = inx,
                    ContentTemplate = n == 0 ? HeaderTemplate : TabTemplate
                };
                tabControl.Content = _items[inx];
                _tabItems.Children.Add(tabControl);
                tabControl.Tapped += OnTabControlTapped;
            }
            _tabItems.TranslateX(0);
        }

        private void OnTabControlTapped(object sender, TappedRoutedEventArgs e)
        {
            var tab = sender as PivoramaTab;
            this.Position = tab.Index * this.ItemWidth;
            this.ArrangeTabs();
            this.ArrangeItems();
        }

        private void AnimateTabsLeft()
        {
            _tabItems.AnimateX((_tabItems.Children[0] as Control).ActualWidth);
        }

        private void AnimateTabsRight()
        {
            _tabItems.AnimateX(-(_tabItems.Children[0] as Control).ActualWidth);
        }

        private void ArrangeItems()
        {
            if (_isInitialized && this.ActualWidth > 0)
            {
                int count = _items.Count;
                for (int n = 0; n < count; n++)
                {
                    double x = TransformX(Position, n * ItemWidth);

                    var headerControl = _headerItems.Children[n] as PivoramaItem;
                    headerControl.MoveX(x);

                    var itemControl = _container.Children[n] as PivoramaItem;
                    itemControl.MoveX(x);

                    if (IsItemInRange(n))
                    {
                        if (itemControl.Content != _items[n])
                        {
                            headerControl.Content = _items[n];
                            itemControl.Content = _items[n];
                        }
                    }
                    else
                    {
                        headerControl.Content = null;
                        itemControl.Content = null;
                    }
                }
            }
        }

        private double TransformX(double position, double x)
        {
            if (position < this.ItemWidth && x >= this.PanelWidth - this.ItemWidth)
            {
                return -this.ItemWidth;
            }

            if ((x + this.ItemWidth * 2) > position)
            {
                return x;
            }
            return x + this.PanelWidth;
        }

        private bool IsItemInRange(int n)
        {
            double pos = Position;
            double x = n * this.ItemWidth;
            x = TransformX(Position, x);

            if (x < 0)
            {
                return true;
            }

            double rangeWidth = this.ActualWidth + this.ItemWidth;
            if ((x + this.ItemWidth * 2) > pos && x - pos < rangeWidth)
            {
                return true;
            }
            return false;
        }

        private PivoramaItem CreateHeader()
        {
            return new PivoramaItem
            {
                ContentTemplate = this.HeaderTemplate
            };
        }

        private PivoramaItem CreateItem()
        {
            return new PivoramaItem
            {
                ContentTemplate = this.ContentTemplate
            };
        }
    }
}
