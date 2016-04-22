using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    public class ContainerStyle : DependencyObject
    {
        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(ContainerStyle), new PropertyMetadata(new Thickness(0)));

        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(ContainerStyle), new PropertyMetadata(new Thickness(0)));

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public void Merge(ContainerStyle style)
        {
            if (style != null)
            {
                if (Margin != style.Margin)
                {
                    Margin = style.Margin;
                }
                if (Padding != style.Padding)
                {
                    Padding = style.Padding;
                } 
            }
        }
    }
}
