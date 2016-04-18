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


        public HtmlBlock Host { get; set; }



        public virtual void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {

        }

        public virtual bool Match(HtmlFragment fragment)
        {
            return fragment != null && !string.IsNullOrEmpty(fragment.Name) && TargetTags.Any(t => t.Equals(fragment.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        protected void ApplyContainerStyles(Grid container, ContainerStyle style)
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

        protected void ApplyParagraphStyles(Paragraph paragraph, ParagraphStyle style)
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

        protected void ApplyTextStyles(TextElement textElement, TextStyle style)
        {
            if (style != null)
            {
                SetBindingFontSize(textElement, size => { return size * style.GetFontSizeRatio(); });

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

        protected void ApplyImageStyles(FrameworkElement element, ImageStyle style)
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

        private void SetBindingFontSize(TextElement element, Func<double, double> calculateFontSize)
        {
            if (Host != null)
            {
                element.FontSize = calculateFontSize(Host.FontSize);

                Host.RegisterPropertyChangedCallback(HtmlBlock.FontSizeProperty, (sender, dp) =>
                {
                    element.FontSize = calculateFontSize(Host.FontSize);
                });
            }
        }
    }
}
