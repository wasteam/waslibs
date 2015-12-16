using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    partial class Virtualbox
    {
        #region VirtualWidth
        public double VirtualWidth
        {
            get { return (double)GetValue(VirtualWidthProperty); }
            set { SetValue(VirtualWidthProperty, value); }
        }

        public static readonly DependencyProperty VirtualWidthProperty = DependencyProperty.Register("VirtualWidth", typeof(double), typeof(Virtualbox), new PropertyMetadata(0.0, ArrangeContent));
        #endregion

        #region VirtualHeight
        public double VirtualHeight
        {
            get { return (double)GetValue(VirtualHeightProperty); }
            set { SetValue(VirtualHeightProperty, value); }
        }

        public static readonly DependencyProperty VirtualHeightProperty = DependencyProperty.Register("VirtualHeight", typeof(double), typeof(Virtualbox), new PropertyMetadata(0.0, ArrangeContent));
        #endregion


        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Virtualbox), new PropertyMetadata(Stretch.Uniform, ArrangeContent));
        #endregion

        #region AlignmentX
        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }

        public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(Virtualbox), new PropertyMetadata(AlignmentX.Center, ArrangeContent));
        #endregion

        #region AlignmentY
        public AlignmentY AlignmentY
        {
            get { return (AlignmentY)GetValue(AlignmentYProperty); }
            set { SetValue(AlignmentYProperty, value); }
        }

        public static readonly DependencyProperty AlignmentYProperty = DependencyProperty.Register("AlignmentY", typeof(AlignmentY), typeof(Virtualbox), new PropertyMetadata(AlignmentY.Center, ArrangeContent));
        #endregion

        private static void ArrangeContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Virtualbox;
            control.ArrangeContent();
        }
    }
}
