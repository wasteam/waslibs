using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    public class ParagraphStyle : TextStyle
    {
        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(ParagraphStyle), new PropertyMetadata(new Thickness(0)));

        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        public void Merge(ParagraphStyle style)
        {
            if (style != null)
            {
                if (Margin != style.Margin)
                {
                    Margin = style.Margin;
                }
                base.Merge(style);
            }
        }
    }
}
