using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    #region ShapeTypes
    public enum ShapeType
    {
        Rectangle,
        Ellipse,
        Border
    }
    #endregion

    public partial class ShapeImage : ContentControl
    {
        private Border _container = null;
        private FrameworkElement _content = null;

        public ShapeImage()
        {
            this.DefaultStyleKey = typeof(ShapeImage);
        }

        #region ShapeType
        public ShapeType ShapeType
        {
            get { return (ShapeType)GetValue(ShapeTypeProperty); }
            set { SetValue(ShapeTypeProperty, value); }
        }

        private static void ShapeTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShapeImage;
            control.SetShapeType((ShapeType)e.NewValue);
        }

        public static readonly DependencyProperty ShapeTypeProperty = DependencyProperty.Register("ShapeType", typeof(ShapeType), typeof(ShapeImage), new PropertyMetadata(null, ShapeTypeChanged));
        #endregion


        #region Stroke
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(ShapeImage), new PropertyMetadata(null));
        #endregion

        #region StrokeThickness
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ShapeImage), new PropertyMetadata(0.0));
        #endregion


        #region CornerRadius
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ShapeImage), new PropertyMetadata(new CornerRadius()));
        #endregion

        #region RadiusX
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(ShapeImage), new PropertyMetadata(0.0));
        #endregion

        #region RadiusY
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(ShapeImage), new PropertyMetadata(0.0));
        #endregion


        #region Fill
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(ShapeImage), new PropertyMetadata(null));
        #endregion

        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ShapeImage), new PropertyMetadata(Stretch.Uniform, FillChanged));
        #endregion

        #region AlignmentX
        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }

        public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(ShapeImage), new PropertyMetadata(AlignmentX.Center, FillChanged));
        #endregion

        #region AlignmentY
        public AlignmentY AlignmentY
        {
            get { return (AlignmentY)GetValue(AlignmentYProperty); }
            set { SetValue(AlignmentYProperty, value); }
        }

        public static readonly DependencyProperty AlignmentYProperty = DependencyProperty.Register("AlignmentY", typeof(AlignmentY), typeof(ShapeImage), new PropertyMetadata(AlignmentY.Center, FillChanged));
        #endregion

        private void SetShapeType(ShapeType shapeType)
        {
            if (_container != null)
            {
                if (shapeType == ShapeType.Border)
                {
                    var border = new Border();

                    border.SetBinding(Border.BackgroundProperty, new Binding { Source = this, Path = new PropertyPath("Fill") });
                    border.SetBinding(Border.BorderBrushProperty, new Binding { Source = this, Path = new PropertyPath("BorderBrush") });
                    border.SetBinding(Border.BorderThicknessProperty, new Binding { Source = this, Path = new PropertyPath("BorderThickness") });
                    border.SetBinding(Border.CornerRadiusProperty, new Binding { Source = this, Path = new PropertyPath("CornerRadius") });

                    _container.Child = border;
                }
                else
                {
                    Shape shape;
                    switch (shapeType)
                    {
                        case ShapeType.Ellipse:
                            shape = new Ellipse();
                            break;
                        case ShapeType.Rectangle:
                        default:
                            shape = new Rectangle();
                            shape.SetBinding(Rectangle.RadiusXProperty, new Binding { Source = this, Path = new PropertyPath("RadiusX") });
                            shape.SetBinding(Rectangle.RadiusYProperty, new Binding { Source = this, Path = new PropertyPath("RadiusY") });
                            break;
                    }

                    shape.SetBinding(Shape.FillProperty, new Binding { Source = this, Path = new PropertyPath("Fill") });
                    shape.SetBinding(Shape.StrokeProperty, new Binding { Source = this, Path = new PropertyPath("Stroke") });
                    shape.SetBinding(Shape.StrokeThicknessProperty, new Binding { Source = this, Path = new PropertyPath("StrokeThickness") });

                    _container.Child = shape;
                }
            }
        }

        private static void FillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShapeImage;
            control.RefreshFill();
        }

        private void RefreshFill()
        {
            var imageBrush = this.Fill as ImageBrush;
            if (imageBrush != null)
            {
                imageBrush.Stretch = this.Stretch;
                imageBrush.AlignmentX = this.AlignmentX;
                imageBrush.AlignmentY = this.AlignmentY;
            }
        }

        protected override void OnApplyTemplate()
        {
            _container = base.GetTemplateChild("container") as Border;
            _content = base.GetTemplateChild("content") as FrameworkElement;

            this.SetShapeType(this.ShapeType);
            this.SetSource(this.Source);

            base.OnApplyTemplate();
        }
    }
}
