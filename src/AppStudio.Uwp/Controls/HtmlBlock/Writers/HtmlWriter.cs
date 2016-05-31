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
using Windows.UI.Text;

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

        protected static Binding CreateBinding(object source, string path)
        {
            return new Binding
            {
                Path = new PropertyPath(path),
                Source = source
            };
        }

        protected static void ApplyContainerStyles(Grid container, ContainerStyle style)
        {
            if (style != null)
            {
                BindingOperations.SetBinding(container, Grid.MarginProperty, CreateBinding(style, "Margin"));
                BindingOperations.SetBinding(container, Grid.PaddingProperty, CreateBinding(style, "Padding"));
            }
        }

        protected static void ApplyParagraphStyles(Paragraph paragraph, ParagraphStyle style)
        {
            if (style != null)
            {
                BindingOperations.SetBinding(paragraph, Paragraph.MarginProperty, CreateBinding(style, "Margin"));
                ApplyTextStyles(paragraph, style);
            }
        }

        protected static void ApplyTextStyles(TextElement textElement, TextStyle style)
        {
            if (style != null)
            {
                SetBinding(style, TextStyle.FontSizeProperty, () =>
                {
                    var fontSize = style.FontSize * style.FontSizeRatioValue();
                    if (fontSize > 0)
                    {
                        textElement.FontSize = fontSize;
                    }
                });

                SetBinding(style, TextStyle.FontSizeRatioProperty, () =>
                {
                    var fontSize = style.FontSize * style.FontSizeRatioValue();
                    if (fontSize > 0)
                    {
                        textElement.FontSize = fontSize;
                    }
                });

                BindingOperations.SetBinding(textElement, TextElement.FontFamilyProperty, CreateBinding(style, "FontFamily"));
                BindingOperations.SetBinding(textElement, TextElement.FontStyleProperty, CreateBinding(style, "FontStyle"));
                BindingOperations.SetBinding(textElement, TextElement.FontWeightProperty, CreateBinding(style, "FontWeight"));
                BindingOperations.SetBinding(textElement, TextElement.ForegroundProperty, CreateBinding(style, "Foreground"));
            }
        }

        protected static void ApplyImageStyles(FrameworkElement element, ImageStyle style)
        {
            if (style != null)
            {
                BindingOperations.SetBinding(element, FrameworkElement.MarginProperty, CreateBinding(style, "Margin"));
                BindingOperations.SetBinding(element, FrameworkElement.HorizontalAlignmentProperty, CreateBinding(style, "HorizontalAlignment"));
            }
        }

        private static void SetBinding(DependencyObject source, DependencyProperty property, Action applyChange)
        {
            if (source != null)
            {
                applyChange();

                source.RegisterPropertyChangedCallback(property, (sender, dp) =>
                {
                    applyChange();
                });
            }
        }
    }
}
