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

        protected static Binding CreateBinding(object source, string path)
        {
            return new Binding
            {
                Path = new PropertyPath(path),
                Source = source
            };
        }

        protected void ApplyContainerStyles(Grid container, ContainerStyle style)
        {
            if (style != null)
            {
                BindingOperations.SetBinding(container, Grid.MarginProperty, CreateBinding(style, "Margin"));
                BindingOperations.SetBinding(container, Grid.PaddingProperty, CreateBinding(style, "Padding"));
            }
        }

        protected void ApplyParagraphStyles(Paragraph paragraph, ParagraphStyle style)
        {
            if (style != null)
            {
                BindingOperations.SetBinding(paragraph, Paragraph.MarginProperty, CreateBinding(style, "Margin"));
                ApplyTextStyles(paragraph, style);
            }
        }

        protected void ApplyTextStyles(TextElement textElement, TextStyle style)
        {
            if (style != null)
            {
                SetBindingFontSize(textElement, style, size => { return size * style.GetFontSizeRatio(); });

                BindingOperations.SetBinding(textElement, TextElement.FontFamilyProperty, CreateBinding(style, "FontFamily"));
                BindingOperations.SetBinding(textElement, TextElement.FontStyleProperty, CreateBinding(style, "FontStyle"));
                BindingOperations.SetBinding(textElement, TextElement.FontWeightProperty, CreateBinding(style, "FontWeight"));
                BindingOperations.SetBinding(textElement, TextElement.ForegroundProperty, CreateBinding(style, "Foreground"));
            }
        }

        protected void ApplyImageStyles(FrameworkElement element, ImageStyle style)
        {
            if (style != null)
            {
                BindingOperations.SetBinding(element, FrameworkElement.MarginProperty, CreateBinding(style, "Margin"));
                BindingOperations.SetBinding(element, FrameworkElement.HorizontalAlignmentProperty, CreateBinding(style, "HorizontalAlignment"));
            }
        }

        private void SetBindingFontSize(TextElement element, TextStyle style, Func<double, double> calculateFontSize)
        {
            if (Host != null)
            {
                element.FontSize = calculateFontSize(Host.FontSize);

                Host.RegisterPropertyChangedCallback(HtmlBlock.FontSizeProperty, (sender, dp) =>
                {
                    element.FontSize = calculateFontSize(Host.FontSize);
                });
                style.RegisterPropertyChangedCallback(TextStyle.FontSizeRatioProperty, (sender, dp) =>
                {
                    element.FontSize = calculateFontSize(Host.FontSize);
                });
            }
        }
    }
}
