using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;

using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    partial class Indicator
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

            var control = d as Indicator;

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

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Indicator), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        private void ItemsSourceChanged(IEnumerable items)
        {
            if (_isInitialized)
            {
                ClearChildren();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        AddItem(item);
                    }
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isInitialized)
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
                        SelectedIndexChanged(this.SelectedIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                        {
                            RemoveItem(item);
                        }
                        SelectedIndexChanged(this.SelectedIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                    default:
                        break;
                }
            }
        }

        private void ClearChildren()
        {
            this.SelectedIndex = -1;
            foreach (var item in _stack.Children.Cast<IndicatorItem>())
            {
                UnplugControl(item);
            }
            _stack.Children.Clear();
        }

        private void AddItem(object item, int index = -1)
        {
            index = index < 0 ? _stack.Children.Count : index;
            var control = new IndicatorItem
            {
                Style = this.ItemContainerStyle,
                ContentTemplate = this.ItemTemplate,
                Content = item
            };
            _stack.Children.Insert(index, control);
            PlugControl(control);
        }

        private void RemoveItem(object item)
        {
            var control = _stack.Children.Cast<IndicatorItem>().Where(r => r.Content.Equals(item)).FirstOrDefault();
            if (control != null)
            {
                UnplugControl(control);
                _stack.Children.Remove(control);
            }
        }

        #region Plug/Unplug Control
        private void PlugControl(IndicatorItem control)
        {
            control.IsSelectedChanged += OnSelectedItemChanged;
        }

        private void UnplugControl(IndicatorItem control)
        {
            control.IsSelectedChanged -= OnSelectedItemChanged;
        }
        #endregion
    }
}
