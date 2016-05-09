using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    public class TextStyle : DependencyObject
    {
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(TextStyle), new PropertyMetadata(null));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(TextStyle), new PropertyMetadata(null));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeRatioProperty = DependencyProperty.Register("FontSizeRatio", typeof(string), typeof(TextStyle), new PropertyMetadata(null));

        public string FontSizeRatio
        {
            get { return (string)GetValue(FontSizeRatioProperty); }
            set { SetValue(FontSizeRatioProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(TextStyle), new PropertyMetadata(null));

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(TextStyle), new PropertyMetadata(FontWeights.Normal));

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public void Merge(TextStyle style)
        {
            if (style != null)
            {
                if (style.FontFamily != null && FontFamily != style.FontFamily)
                {
                    FontFamily = style.FontFamily;
                }
                if (!string.IsNullOrEmpty(style.FontSizeRatio) && FontSizeRatio != style.FontSizeRatio)
                {
                    FontSizeRatio = style.FontSizeRatio;
                }
                if (style.FontStyle != FontStyle.Normal && FontStyle != style.FontStyle)
                {
                    FontStyle = style.FontStyle;
                }
                if (style.FontWeight.Weight != FontWeights.Normal.Weight && FontWeight.Weight != style.FontWeight.Weight)
                {
                    FontWeight = style.FontWeight;
                }
                if (style.Foreground != null && Foreground != style.Foreground)
                {
                    Foreground = style.Foreground;
                } 
            }
        }

        public float FontSizeRatioValue()
        {
            float resultRatio;
            if (float.TryParse(FontSizeRatio, out resultRatio))
            {
                return resultRatio;
            }
            else
            {
                return 0;
            }
        }
    }
}
