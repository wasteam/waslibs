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

        protected static void ApplyContainerStyles(Grid container, ContainerStyle style)
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
                SetBinding(Host, HtmlBlock.FontSizeProperty, () =>
                {
                    var fontSize = Host.FontSize * style.FontSizeRatioValue();
                    if (fontSize > 0)
                    {
                        textElement.FontSize = fontSize;
                    }
                });

                SetBinding(style, TextStyle.FontSizeRatioProperty, () =>
                {
                    var fontSize = Host.FontSize * style.FontSizeRatioValue();
                    if (fontSize > 0)
                    {
                        textElement.FontSize = fontSize;
                    }
                });

                SetBinding(style, TextStyle.FontFamilyProperty, () =>
                {
                    if (style.FontFamily != null)
                    {
                        textElement.FontFamily = style.FontFamily;
                    }
                });

                SetBinding(style, TextStyle.FontStyleProperty, () =>
                {
                    if (style.FontStyle != FontStyle.Normal)
                    {
                        textElement.FontStyle = style.FontStyle;
                    }
                });

                SetBinding(style, TextStyle.FontWeightProperty, () =>
                {
                    if (style.FontWeight.Weight != FontWeights.Normal.Weight)
                    {
                        textElement.FontWeight = style.FontWeight;
                    }
                });

                SetBinding(style, TextStyle.ForegroundProperty, () =>
                {
                    if (style.Foreground != null)
                    {
                        textElement.Foreground = style.Foreground;
                    }
                });
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
