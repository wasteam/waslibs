using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    public class DocumentStyle : DependencyObject
    {
        public ContainerStyle Section { get; set; } = new ContainerStyle();
        public ContainerStyle Article { get; set; } = new ContainerStyle();
        public ContainerStyle Header { get; set; } = new ContainerStyle();
        public ContainerStyle Footer { get; set; } = new ContainerStyle();
        public ContainerStyle Main { get; set; } = new ContainerStyle();
        public ContainerStyle Figure { get; set; } = new ContainerStyle();
        public ContainerStyle Details { get; set; } = new ContainerStyle();
        public ContainerStyle Summary { get; set; } = new ContainerStyle();
        public ContainerStyle Div { get; set; } = new ContainerStyle();
        public ContainerStyle Ul { get; set; } = new ContainerStyle();
        public ContainerStyle Ol { get; set; } = new ContainerStyle();
        public ContainerStyle Dl { get; set; } = new ContainerStyle();
        public ContainerStyle Td { get; set; } = new ContainerStyle();

        public TableStyle Table { get; set; } = new TableStyle();

        public ImageStyle Img { get; set; } = new ImageStyle();
        public ImageStyle YouTube { get; set; } = new ImageStyle();
        public ImageStyle Channel9 { get; set; } = new ImageStyle();

        public ParagraphStyle H1 { get; set; } = new ParagraphStyle();
        public ParagraphStyle H2 { get; set; } = new ParagraphStyle();
        public ParagraphStyle H3 { get; set; } = new ParagraphStyle();
        public ParagraphStyle H4 { get; set; } = new ParagraphStyle();
        public ParagraphStyle H5 { get; set; } = new ParagraphStyle();
        public ParagraphStyle H6 { get; set; } = new ParagraphStyle();
        public ParagraphStyle BlockQuote { get; set; } = new ParagraphStyle();
        public ParagraphStyle P { get; set; } = new ParagraphStyle();
        public ParagraphStyle Pre { get; set; } = new ParagraphStyle();
        public ParagraphStyle FigCaption { get; set; } = new ParagraphStyle();
        public ParagraphStyle Dt { get; set; } = new ParagraphStyle();
        public ParagraphStyle Dd { get; set; } = new ParagraphStyle();

        public ListStyle Li { get; set; } = new ListStyle();

        public TextStyle A { get; set; } = new TextStyle();
        public TextStyle Span { get; set; } = new TextStyle();
        public TextStyle Label { get; set; } = new TextStyle();
        public TextStyle Q { get; set; } = new TextStyle();
        public TextStyle Cite { get; set; } = new TextStyle();
        public TextStyle I { get; set; } = new TextStyle();
        public TextStyle Em { get; set; } = new TextStyle();
        public TextStyle Mark { get; set; } = new TextStyle();
        public TextStyle Time { get; set; } = new TextStyle();
        public TextStyle Code { get; set; } = new TextStyle();
        public TextStyle Strong { get; set; } = new TextStyle();

        public void Merge(params DocumentStyle[] styles)
        {
            if (styles != null)
            {
                foreach (var style in styles)
                {
                    Section.Merge(style.Section);
                    Article.Merge(style.Article);
                    Header.Merge(style.Header);
                    Footer.Merge(style.Footer);
                    Main.Merge(style.Main);
                    Figure.Merge(style.Figure);
                    Details.Merge(style.Details);
                    Summary.Merge(style.Summary);
                    Div.Merge(style.Div);
                    Ul.Merge(style.Ul);
                    Ol.Merge(style.Ol);
                    Dl.Merge(style.Dl);
                    Td.Merge(style.Td);

                    Table.Merge(style.Table);

                    Img.Merge(style.Img);
                    YouTube.Merge(style.YouTube);
                    Channel9.Merge(style.Channel9);

                    H1.Merge(style.H1);
                    H2.Merge(style.H2);
                    H3.Merge(style.H3);
                    H4.Merge(style.H4);
                    H5.Merge(style.H5);
                    H6.Merge(style.H6);
                    BlockQuote.Merge(style.BlockQuote);
                    P.Merge(style.P);
                    FigCaption.Merge(style.FigCaption);
                    Pre.Merge(style.Pre);
                    Dt.Merge(style.Dt);
                    Dd.Merge(style.Dd);

                    Li.Merge(style.Li);

                    A.Merge(style.A);
                    Span.Merge(style.Span);
                    Label.Merge(style.Label);
                    Q.Merge(style.Q);
                    Cite.Merge(style.Cite);
                    I.Merge(style.I);
                    Em.Merge(style.Em);
                    Mark.Merge(style.Mark);
                    Time.Merge(style.Time);
                    Code.Merge(style.Code);
                    Strong.Merge(style.Strong);
                }
            }
        }
    }
}
