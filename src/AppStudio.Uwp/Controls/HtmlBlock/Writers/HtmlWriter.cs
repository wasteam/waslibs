using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    abstract class HtmlWriter
    {
        public abstract string[] TargetTags { get; }

        public abstract DependencyObject GetControl(HtmlFragment fragment);

        public virtual void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {

        }

        public virtual bool Match(HtmlFragment fragment)
        {
            return fragment != null && !string.IsNullOrEmpty(fragment.Name) && TargetTags.Any(t => t.Equals(fragment.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        protected static void ApplyContainerStyles(Grid container, ContainerStyle style)
        {
            if (style != null)
            {
                if (!double.IsNaN(style.Margin.Top))
                {
                    container.Margin = style.Margin;
                }
                if (!double.IsNaN(style.Padding.Top))
                {
                    container.Padding = style.Padding;
                }
            }
        }

        protected static void ApplyParagraphStyles(Paragraph paragraph, ParagraphStyle style)
        {
            if (style != null)
            {
                if (!double.IsNaN(style.Margin.Top))
                {
                    paragraph.Margin = style.Margin;
                }
                ApplyTextStyles(paragraph, style);
            }
        }

        protected static void ApplyTextStyles(TextElement textElement, TextStyle style)
        {
            if (style != null)
            {
                textElement.FontSize *= style.GetFontSizeRatio();

                if (style.FontFamily != null)
                {
                    textElement.FontFamily = style.FontFamily;
                }

                textElement.FontStyle = style.FontStyle;

                if (style.FontWeight.Weight > 0)
                {
                    textElement.FontWeight = style.FontWeight;
                }

                if (style.Foreground != null)
                {
                    textElement.Foreground = style.Foreground;
                }
            }
        }

        protected static void ApplyImageStyles(FrameworkElement element, ImageStyle style)
        {
            if (style != null && element != null)
            {
                if (!double.IsNaN(style.Margin.Top))
                {
                    element.Margin = style.Margin;
                }
                if (style.HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    element.HorizontalAlignment = style.HorizontalAlignment;
                }
            }
        }
    }
}
