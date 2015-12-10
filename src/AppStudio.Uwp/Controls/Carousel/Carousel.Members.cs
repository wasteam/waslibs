using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class Carousel
    {
        #region ContentTemplate
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Carousel), new PropertyMetadata(null));
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

            var control = d as Carousel;
            control.CreateItems();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Carousel), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region Items
        public IList<object> Items
        {
            get { return _items; }
        }
        #endregion

        #region SelectedIndex
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(-1, SelectedIndexChanged));
        #endregion

        #region MaxItems
        public int MaxItems
        {
            get { return (int)GetValue(MaxItemsProperty); }
            set { SetValue(MaxItemsProperty, value); }
        }

        public static readonly DependencyProperty MaxItemsProperty = DependencyProperty.Register("MaxItems", typeof(int), typeof(Carousel), new PropertyMetadata(3, MaxItemsChanged));
        #endregion

        #region AspectRatio
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(Carousel), new PropertyMetadata(1.6, OnInvalidate));
        #endregion

        #region AlignmentX
        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }

        public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(Carousel), new PropertyMetadata(AlignmentX.Left, OnInvalidate));
        #endregion

        private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            if ((int)e.NewValue > -1)
            {
                control.ArrangeItems();
            }
        }

        private static void OnInvalidate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control.InvalidateMeasure();
        }

        private static void MaxItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control.BuildSlots();
            control.CreateItems();
            control.InvalidateMeasure();
        }

        private void BuildSlots()
        {
            if (_container != null)
            {
                int count = this.MaxItems + 2;

                _container.Children.Clear();
                for (int n = 0; n < count; n++)
                {
                    var control = new CarouselSlot
                    {
                        ContentTemplate = ContentTemplate,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Stretch,
                        UseLayoutRounding = false
                    };
                    _container.Children.Add(control);
                    control.MoveX(n);
                }
            }
        }

        private void CreateItems()
        {
            var items = this.ItemsSource as IEnumerable;
            if (items != null)
            {
                _items = new List<object>();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        _items.Add(item);
                    }
                }

                this.ArrangeItems();
            }
        }

        private void ArrangeItems()
        {
            if (_container != null)
            {
                var controls = _container.Children.Cast<ContentControl>().OrderBy(r => r.GetTranslateX()).ToArray();
                for (int n = 0; n < controls.Length; n++)
                {
                    int index = (this.SelectedIndex + n - 1);
                    if (AlignmentX == AlignmentX.Center)
                    {
                        index -= this.MaxItems / 2;
                    }
                    else if (AlignmentX == AlignmentX.Right)
                    {
                        index -= (this.MaxItems - 1);
                    }
                    index = index.Mod(_items.Count);

                    if (_items.Count > 0)
                    {
                        controls[n].Content = null;
                        var item = _items[index];
                        controls[n].Content = item;
                    }
                    else
                    {
                        controls[n].Content = null;
                    }
                }
            }
        }
    }
}
