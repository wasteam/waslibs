using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;

namespace AppStudio.Uwp.Controls
{
    #region ShapeMode
    public enum ShapeMode
    {
        Rectangle,
        Ellipse
    }
    #endregion

    public sealed class Indicator : ListViewBase
    {
        public Indicator()
        {
            this.DefaultStyleKey = typeof(Indicator);
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.SizeChanged += OnSizeChanged;
        }

        #region ShapeMode
        public ShapeMode ShapeMode
        {
            get { return (ShapeMode)GetValue(ShapeModeProperty); }
            set { SetValue(ShapeModeProperty, value); }
        }

        private static void ShapeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Indicator;
            control.SetShapeMode((ShapeMode)e.NewValue);
        }

        private void SetShapeMode(ShapeMode shapeMode)
        {
            if (shapeMode == ShapeMode.Rectangle)
            {
                this.ItemTemplate = this.RectangleIndicatorTemplate;
            }
            else
            {
                this.ItemTemplate = this.EllipseIndicatorTemplate;
            }
        }

        public static readonly DependencyProperty ShapeModeProperty = DependencyProperty.Register("ShapeMode", typeof(ShapeMode), typeof(Indicator), new PropertyMetadata(ShapeMode.Ellipse, ShapeModeChanged));
        #endregion


        #region RectangleIndicatorTemplate
        public DataTemplate RectangleIndicatorTemplate
        {
            get { return (DataTemplate)GetValue(RectangleIndicatorTemplateProperty); }
            set { SetValue(RectangleIndicatorTemplateProperty, value); }
        }

        public static readonly DependencyProperty RectangleIndicatorTemplateProperty = DependencyProperty.Register("RectangleIndicatorTemplate", typeof(DataTemplate), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        #region EllipseIndicatorTemplate
        public DataTemplate EllipseIndicatorTemplate
        {
            get { return (DataTemplate)GetValue(EllipseIndicatorTemplateProperty); }
            set { SetValue(EllipseIndicatorTemplateProperty, value); }
        }

        public static readonly DependencyProperty EllipseIndicatorTemplateProperty = DependencyProperty.Register("EllipseIndicatorTemplate", typeof(DataTemplate), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        #region RectangleIndicatorItemStyle
        public Style RectangleIndicatorItemStyle
        {
            get { return (Style)GetValue(RectangleIndicatorItemStyleProperty); }
            set { SetValue(RectangleIndicatorItemStyleProperty, value); }
        }

        public static readonly DependencyProperty RectangleIndicatorItemStyleProperty = DependencyProperty.Register("RectangleIndicatorItemStyle", typeof(Style), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        #region EllipseIndicatorItemStyle
        public Style EllipseIndicatorItemStyle
        {
            get { return (Style)GetValue(EllipseIndicatorItemStyleProperty); }
            set { SetValue(EllipseIndicatorItemStyleProperty, value); }
        }

        public static readonly DependencyProperty EllipseIndicatorItemStyleProperty = DependencyProperty.Register("EllipseIndicatorItemStyle", typeof(Style), typeof(Indicator), new PropertyMetadata(null));
        #endregion


        #region PressedBackground
        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        #region SelectedBackground
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        protected override void OnApplyTemplate()
        {
            this.SetShapeMode(this.ShapeMode);

            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new IndicatorItem
            {
                Style = this.ShapeMode == ShapeMode.Rectangle ? this.RectangleIndicatorItemStyle : this.EllipseIndicatorItemStyle
            };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var container = element as IndicatorItem;
            container.SetBinding(IndicatorItem.BackgroundProperty, new Binding { Source = this, Path = new PropertyPath("Background") });
            container.SetBinding(IndicatorItem.PressedBackgroundProperty, new Binding { Source = this, Path = new PropertyPath("PressedBackground") });
            container.SetBinding(IndicatorItem.SelectedBackgroundProperty, new Binding { Source = this, Path = new PropertyPath("SelectedBackground") });
            base.PrepareContainerForItemOverride(element, item);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.SelectedIndex == -1)
            {
                this.SelectedIndex = base.Items.Count > 0 ? 0 : -1;
            }
        }

        // Obsolete
        #region SelectedForeground
        [Deprecated("SelectedForeground property will be removed in future versions.", DeprecationType.Deprecate, 65536)]
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        private static void SelectedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Indicator;
            control.SelectedBackground = e.NewValue as Brush;
        }

        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.Register("SelectedForeground", typeof(Brush), typeof(Indicator), new PropertyMetadata(null, SelectedForegroundChanged));
        #endregion
    }
}
