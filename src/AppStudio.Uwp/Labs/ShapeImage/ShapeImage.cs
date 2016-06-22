using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    #region ShapeMode
    public enum ShapeMode
    {
        Rectangle,
        Ellipse
    }
    #endregion

    public partial class ShapeImage : Control
    {
        private Border _container = null;
        public ShapeImage()
        {
            this.DefaultStyleKey = typeof(ShapeImage);
        }

        #region ShapeMode
        public ShapeMode ShapeMode
        {
            get { return (ShapeMode)GetValue(ShapeModeProperty); }
            set { SetValue(ShapeModeProperty, value); }
        }

        private static void ShapeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShapeImage;
            control.SetShapeMode((ShapeMode)e.NewValue);
        }

        public static readonly DependencyProperty ShapeModeProperty = DependencyProperty.Register("ShapeMode", typeof(ShapeMode), typeof(ShapeImage), new PropertyMetadata(ShapeMode.Rectangle, ShapeModeChanged));
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

        private void SetShapeMode(ShapeMode shapeMode)
        {
            if (_container != null)
            {
                Shape shape;
                switch (shapeMode)
                {
                    case ShapeMode.Ellipse:
                        shape = new Ellipse();
                        break;
                    case ShapeMode.Rectangle:
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

            this.SetShapeMode(this.ShapeMode);

            base.OnApplyTemplate();
        }
    }
}
