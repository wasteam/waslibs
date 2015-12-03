using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class Carousel : Control
    {
        private Panel _container = null;
        private RectangleGeometry _clip = null;

        private IList<object> _items = null;

        public Carousel()
        {
            this.DefaultStyleKey = typeof(Carousel);
            this.SizeChanged += OnSizeChanged;
        }

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
                throw new ArgumentException();
            }

            var control = d as Carousel;
            control.ArrangeItems(e.NewValue as IEnumerable);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Carousel), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region Items
        public IList<object> Items
        {
            get { return _items; }
        }
        #endregion

        #region Index
        public int Index
        {
            get { return _index; }
            set { SetValue(IndexProperty, value); _index = value; }
        }

        private static void IndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            if (control._index != (int)e.NewValue)
            {
                control.ArrangeItems();
            }
        }

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(Carousel), new PropertyMetadata(0, IndexChanged));
        #endregion

        #region MaxItems
        public int MaxItems
        {
            get { return (int)GetValue(MaxItemsProperty); }
            set { SetValue(MaxItemsProperty, value); }
        }

        public static readonly DependencyProperty MaxItemsProperty = DependencyProperty.Register("MaxItems", typeof(int), typeof(Carousel), new PropertyMetadata(3));
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

        private static void OnInvalidate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control.InvalidateArrange();
        }

        protected override void OnApplyTemplate()
        {
            _container = base.GetTemplateChild("container") as Panel;
            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            CreateSlots(MaxItems + 2);

            ArrangeItems(this.ItemsSource as IEnumerable);

            _container.ManipulationStarted += OnManipulationStarted;
            _container.ManipulationDelta += OnManipulationDelta;
            _container.ManipulationCompleted += OnManipulationCompleted;
            _container.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;

            base.OnApplyTemplate();
        }

        private void CreateSlots(int count)
        {
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

        private void ArrangeItems(IEnumerable items)
        {
            _items = new List<object>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    _items.Add(item);
                }
            }
            ArrangeItems();
        }
        private void ArrangeItems()
        {
            if (_container != null)
            {
                var controls = _container.Children.Cast<ContentControl>().OrderBy(r => r.GetTranslateX()).ToArray();
                for (int n = 0; n < controls.Length; n++)
                {
                    if (_items.Count > 0)
                    {
                        controls[n].Content = null;
                        var item = _items[(this.Index + n - 1).Mod(_items.Count)];
                        controls[n].Content = item;
                    }
                    else
                    {
                        controls[n].Content = null;
                    }
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double contentWidth = availableSize.Width;
            double width = contentWidth / MaxItems;
            double height = width / AspectRatio;
            if (height < MinHeight)
            {
                height = MinHeight;
                width = height * AspectRatio;
            }
            if (height > MaxHeight)
            {
                height = MaxHeight;
                width = height * AspectRatio;
            }
            var size = new Size(width, height);
            base.MeasureOverride(size);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            double slotWidth = Math.Round(Math.Min(size.Width / MARGIN_FACTOR, Math.Max(_container.ActualWidth / (double)MaxItems, size.Height * AspectRatio)), 2);
            double factor = Math.Round(_slotWidth / slotWidth, 2);
            factor = factor == 0 ? 1 : factor;
            _slotWidth = Math.Round(slotWidth, 2);
            _offset = Math.Round((_offset / factor).Mod(_slotWidth), 2);

            var positions = GetPositions(_slotWidth).ToArray();
            var controls = _container.Children.Cast<CarouselSlot>().OrderBy(r => r.X1).ToArray();
            for (int n = 0; n < controls.Length; n++)
            {
                var position = positions[n];
                var control = controls[n];
                control.MoveX(position.X + _offset);
                control.Width = _slotWidth;
                control.Height = _container.ActualHeight;
            }

            return size;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _clip.Rect = new Rect(new Point(), _container.GetSize());
        }
    }
}
