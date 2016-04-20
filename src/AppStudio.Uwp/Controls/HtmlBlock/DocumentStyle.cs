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
        public TextStyle Label { get; set; }
        public TextStyle Q { get; set; }
        public TextStyle Cite { get; set; }
        public TextStyle I { get; set; }
        public TextStyle Em { get; set; }
        public TextStyle Mark { get; set; }
        public TextStyle Time { get; set; }
        public ParagraphStyle BlockQuote { get; set; }
        public TextStyle Code { get; set; }
        public ParagraphStyle P { get; set; }
        public ParagraphStyle Pre { get; set; }
        public ParagraphStyle FigCaption { get; set; }
        public ContainerStyle Section { get; set; }
        public ContainerStyle Article { get; set; }
        public ContainerStyle Header { get; set; }
        public ContainerStyle Footer { get; set; }
        public ContainerStyle Main { get; set; }
        public ContainerStyle Figure { get; set; }
        public ContainerStyle Details { get; set; }
        public ContainerStyle Summary { get; set; }
        public ContainerStyle Div { get; set; }
        public ContainerStyle Ul { get; set; }
        public ContainerStyle Ol { get; set; }
        public ContainerStyle Dl { get; set; }
        public ParagraphStyle Dt { get; set; }
        public ParagraphStyle Dd { get; set; }
        public ListStyle Li { get; set; }
        public TextStyle Strong { get; set; }
        public ImageStyle Img { get; set; }
        public ImageStyle YouTube { get; set; }
        public TableStyle Table { get; set; }
        public ContainerStyle Td { get; set; }

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
                Label = Merge(Label, style.Label);
                Q = Merge(Q, style.Q);
                Cite = Merge(Cite, style.Cite);
                I = Merge(I, style.I);
                Em = Merge(Em, style.Em);
                Mark = Merge(Mark, style.Mark);
                Time = Merge(Time, style.Time);
                BlockQuote = Merge(BlockQuote, style.BlockQuote);
                Code = Merge(Code, style.Code);
                P = Merge(P, style.P);
                FigCaption = Merge(FigCaption, style.FigCaption);
                Pre = Merge(Pre, style.Pre);
                Section = Merge(Section, style.Section);
                Article = Merge(Article, style.Article);
                Header = Merge(Header, style.Header);
                Footer = Merge(Footer, style.Footer);
                Main = Merge(Main, style.Main);
                Figure = Merge(Figure, style.Figure);
                Details = Merge(Details, style.Details);
                Summary = Merge(Figure, style.Summary);
                Div = Merge(Div, style.Div);
                Ul = Merge(Ul, style.Ul);
                Ol = Merge(Ol, style.Ol);
                Dl = Merge(Dl, style.Dl);
                Dt = Merge(Dt, style.Dt);
                Dd = Merge(Dd, style.Dd);
                Li = Merge(Li, style.Li);
                Strong = Merge(Strong, style.Strong);
                Img = Merge(Img, style.Img);
                YouTube = Merge(YouTube, style.YouTube);
                Table = Merge(Table, style.Table);
                Td = Merge(Td, style.Td);
            }
        }

        private TableStyle Merge(TableStyle source, TableStyle target)
        {
            if (target != null)
            {
                if (source == null)
                {
                    source = new TableStyle();
                }
                source.Merge(target);
            }
            return source;
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

    //TODO: REVIEW XBF DESERIALIZATION ERROR INHERITING FROM CONTAINERSTYLE
    public class TableStyle
    {
        public Thickness Border { get; set; }
        public Brush BorderForeground { get; set; }

        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }

        public GridLength ColumnWidth { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public TableStyle()
        {
            Border = new Thickness(double.NaN);
            Margin = new Thickness(double.NaN);
            Padding = new Thickness(double.NaN);
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        public void Merge(TableStyle style)
        {
            if (double.IsNaN(Border.Top) && !double.IsNaN(style.Border.Top))
            {
                Border = style.Border;
            }
            if (BorderForeground == null && style.BorderForeground != null)
            {
                BorderForeground = style.BorderForeground;
            }
            if (double.IsNaN(Margin.Top) && !double.IsNaN(style.Margin.Top))
            {
                Margin = style.Margin;
            }
            if (double.IsNaN(Padding.Top) && !double.IsNaN(style.Padding.Top))
            {
                Padding = style.Padding;
            }
            if (ColumnWidth == GridLength.Auto && style.ColumnWidth != GridLength.Auto)
            {
                ColumnWidth = style.ColumnWidth;
            }
            if (HorizontalAlignment == HorizontalAlignment.Stretch && style.HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                HorizontalAlignment = style.HorizontalAlignment;
            }
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
            if (HorizontalAlignment == HorizontalAlignment.Stretch && style.HorizontalAlignment != HorizontalAlignment.Stretch)
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

        public ContainerStyle()
        {
            Margin = new Thickness(double.NaN);
            Padding = new Thickness(double.NaN);
        }

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
