using System;
using System.Collections;
using System.Collections.Specialized;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Labs
{
    partial class MosaicPanel
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

            var control = d as MosaicPanel;

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

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(MosaicPanel), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        private void ItemsSourceChanged(IEnumerable items)
        {
            base.Children.Clear();

            if (items != null)
            {
                foreach (var item in items)
                {
                    var control = CreateControl(item);
                    base.Children.Add(control);
                }
            }

            this.InvalidateMeasure();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            this.InvalidateMeasure();
        }

        private void ClearChildren()
        {
            base.Children.Clear();
        }

        private void AddItem(object item, int index = -1)
        {
            index = index < 0 ? base.Children.Count : index;
            base.Children.Insert(index, CreateControl(item));
        }

        private void RemoveItem(object item)
        {
            foreach (ContentControl control in base.Children)
            {
                if (control.Content == item)
                {
                    base.Children.Remove(control);
                    break;
                }
            }
        }

        private ContentControl CreateControl(object content)
        {
            var control = new MosaicItem
            {
                Content = content,
                CommandParameter = content,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
            control.SetBinding(ContentControl.MarginProperty, new Binding { Source = this, Path = new PropertyPath("ItemMargin") });
            control.SetBinding(ContentControl.ContentTemplateProperty, new Binding { Source = this, Path = new PropertyPath("ItemTemplate") });
            control.SetBinding(Button.CommandProperty, new Binding { Source = this, Path = new PropertyPath("ItemClickCommand") });
            return control;
        }
    }
}
