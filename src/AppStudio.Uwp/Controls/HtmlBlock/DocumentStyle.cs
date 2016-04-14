using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    public class DocumentStyle
    {
        public ParagraphStyle H1 { get; set; }
        public ParagraphStyle H2 { get; set; }
        public ParagraphStyle H3 { get; set; }
        public ParagraphStyle H4 { get; set; }
        public ParagraphStyle H5 { get; set; }
        public ParagraphStyle H6 { get; set; }
        public TextStyle A { get; set; }
        public TextStyle Span { get; set; }
        public ParagraphStyle BlockQuote { get; set; }
        public TextStyle Code { get; set; }
        public ParagraphStyle P { get; set; }
        public ContainerStyle Div { get; set; }
        public ContainerStyle Ul { get; set; }
        public ContainerStyle Ol { get; set; }
        public ListStyle Li { get; set; }
        public TextStyle Strong { get; set; }
        public ImageStyle Img { get; set; }
        public ImageStyle YouTube { get; set; }

        public DocumentStyle()
        {
        }

        public void Merge(DocumentStyle style)
        {
            if (style != null)
            {
                H1 = Merge(H1, style.H1);
                H2 = Merge(H2, style.H2);
                H3 = Merge(H3, style.H3);
                H4 = Merge(H4, style.H4);
                H5 = Merge(H5, style.H5);
                H6 = Merge(H6, style.H6);
                A = Merge(A, style.A);
                Span = Merge(Span, style.Span);
                BlockQuote = Merge(BlockQuote, style.BlockQuote);
                Code = Merge(Code, style.Code);
                P = Merge(P, style.P);
                Div = Merge(Div, style.Div);
                Ul = Merge(Ul, style.Ul);
                Ol = Merge(Ol, style.Ol);
                Li = Merge(Li, style.Li);
                Strong = Merge(Strong, style.Strong);
                Img = Merge(Img, style.Img);
                YouTube = Merge(YouTube, style.YouTube);
            }
        }

        private ListStyle Merge(ListStyle source, ListStyle target)
        {
            if (target != null)
            {
                if (source == null)
                {
                    source = new ListStyle();
                }
                source.Merge(target);
            }
            return source;
        }

        private ContainerStyle Merge(ContainerStyle source, ContainerStyle target)
        {
            if (target != null)
            {
                if (source == null)
                {
                    source = new ContainerStyle();
                }
                source.Merge(target);
            }
            return source;
        }

        private ParagraphStyle Merge(ParagraphStyle source, ParagraphStyle target)
        {
            if (target != null)
            {
                if (source == null)
                {
                    source = new ParagraphStyle();
                }
                source.Merge(target); 
            }
            return source;
        }

        private TextStyle Merge(TextStyle source, TextStyle target)
        {
            if (target != null)
            {
                if (source == null)
                {
                    source = new TextStyle();
                }
                source.Merge(target);
            }
            return source;
        }

        private ImageStyle Merge(ImageStyle source, ImageStyle target)
        {
            if (target != null)
            {
                if (source == null)
                {
                    source = new ImageStyle();
                }
                source.Merge(target);
            }
            return source;
        }
    }

    public class ImageStyle
    {
        public Thickness Margin { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public ImageStyle()
        {
            Margin = new Thickness(double.NaN);
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        public void Merge(ImageStyle style)
        {
            if (double.IsNaN(Margin.Top) && !double.IsNaN(style.Margin.Top))
            {
                Margin = style.Margin;
            }
            if (HorizontalAlignment != HorizontalAlignment.Stretch && style.HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                HorizontalAlignment = style.HorizontalAlignment;
            }
        }
    }

    public class ListStyle : ParagraphStyle
    {
        public string Bullet { get; set; }

        public void Merge(ListStyle style)
        {
            if (string.IsNullOrEmpty(Bullet) && !string.IsNullOrEmpty(style.Bullet))
            {
                Bullet = style.Bullet;
            }
            base.Merge(style);
        }
    }

    public class ParagraphStyle : TextStyle
    {
        public Thickness Margin { get; set; }

        public ParagraphStyle()
        {
            Margin = new Thickness(double.NaN);
        }

        public void Merge(ParagraphStyle style)
        {
            if (double.IsNaN(Margin.Top) && !double.IsNaN(style.Margin.Top))
            {
                Margin = style.Margin;
            }
            base.Merge(style);
        }
    }

    public class ContainerStyle
    {
        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }

        public void Merge(ContainerStyle style)
        {
            if (double.IsNaN(Margin.Top) && !double.IsNaN(style.Margin.Top))
            {
                Margin = style.Margin;
            }
            if (double.IsNaN(Padding.Top) && !double.IsNaN(style.Padding.Top))
            {
                Padding = style.Padding;
            }
        }

        public ContainerStyle()
        {
            Margin = new Thickness(double.NaN);
            Padding = new Thickness(double.NaN);
        }
    }

    public class TextStyle
    {
        public FontFamily FontFamily { get; set; }
        public string FontSizeRatio { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
        public Brush Foreground { get; set; }

        public void Merge(TextStyle style)
        {
            if (FontFamily == null && style.FontFamily != null)
            {
                FontFamily = style.FontFamily;
            }
            if (string.IsNullOrEmpty(FontSizeRatio) && style.FontSizeRatio != null)
            {
                FontSizeRatio = style.FontSizeRatio;
            }
            if (FontStyle == FontStyle.Normal && style.FontStyle != FontStyle.Normal)
            {
                FontStyle = style.FontStyle;
            }
            if (FontWeight.Weight == 0 && style.FontWeight.Weight > 0)
            {
                FontWeight = style.FontWeight;
            }
            if (Foreground == null && style.Foreground != null)
            {
                Foreground = style.Foreground;
            }
        }


        public float GetFontSizeRatio()
        {
            float resultRatio;
            if (float.TryParse(FontSizeRatio, out resultRatio))
            {
                return resultRatio;
            }
            else
            {
                return 1;
            }
        }
    }
}
