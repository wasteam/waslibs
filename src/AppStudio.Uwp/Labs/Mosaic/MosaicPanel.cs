using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Labs
{
    public partial class MosaicPanel : Panel
    {
        #region SeedWidth
        public double SeedWidth
        {
            get { return (double)GetValue(SeedWidthProperty); }
            set { SetValue(SeedWidthProperty, value); }
        }

        private static void SeedWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MosaicPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty SeedWidthProperty = DependencyProperty.Register("SeedWidth", typeof(double), typeof(MosaicPanel), new PropertyMetadata(400.0, SeedWidthChanged));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(MosaicPanel), new PropertyMetadata(null));
        #endregion

        private Rect[] _rects;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (base.Children.Count > 0)
            {
                var engine = new Distribution(base.Children.Count, this.SeedWidth);
                _rects = engine.Distribute(availableSize.Width).ToArray();

                for (int n = 0; n < base.Children.Count; n++)
                {
                    var control = base.Children[n];
                    control.Measure(new Size(_rects[n].Width, _rects[n].Height));
                }

                double w = _rects.Max(r => r.X + r.Width);
                double h = _rects.Max(r => r.Y + r.Height);
                return new Size(w, h);
            }
            return new Size(availableSize.Width, 0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (base.Children.Count > 0)
            {
                for (int n = 0; n < base.Children.Count; n++)
                {
                    var control = base.Children[n];
                    control.Arrange(_rects[n]);
                }

                double w = _rects.Max(r => r.X + r.Width);
                double h = _rects.Max(r => r.Y + r.Height);

                return new Size(w, h);
            }
            return finalSize;
        }
    }
}
