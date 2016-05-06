using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace AppStudio.Uwp.Controls.Html.Containers
{
    class GridDocumentContainer : DocumentContainer<Grid>
    {
        public GridDocumentContainer(Grid ctrl) : base(ctrl)
        {
        }

        public override bool CanContain(DependencyObject ctrl)
        {
            return !(ctrl is GridColumn);
        }

        protected override void Add(DependencyObject ctrl)
        {
            if (ctrl is FrameworkElement)
            {
                AddChild(ctrl as FrameworkElement);
            }
            else if (ctrl is Block)
            {
                var textBlock = FindOrCreateTextBlock();

                textBlock.Blocks.Add(ctrl as Block);
            }
            else if (ctrl is Inline)
            {
                var textBlock = FindOrCreateTextBlock();
                var p = FindOrCreateParagraph(textBlock);

                p.Inlines.Add(ctrl as Inline);
            }
            else if (ctrl is GridRow)
            {
                Control.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });
                var row = ctrl as GridRow;

                row.Index = Control.RowDefinitions.Count - 1;
                row.Container = Control;
            }
        }

        private static Paragraph FindOrCreateParagraph(RichTextBlock textBlock)
        {
            var p = textBlock.Blocks
                                .OfType<Paragraph>()
                                .LastOrDefault();

            if (p == null)
            {
                p = new Paragraph();
                textBlock.Blocks.Add(p);
            }

            return p;
        }

        private RichTextBlock FindOrCreateTextBlock()
        {
            var textBlock = Control.GetChild<RichTextBlock>(0, Control.RowDefinitions.Count - 1);

            if (textBlock == null)
            {
                textBlock = new RichTextBlock();
                AddChild(textBlock);
            }
            return textBlock;
        }

        private void AddChild(FrameworkElement element)
        {
            Control.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
            Grid.SetRow(element, Control.RowDefinitions.Count - 1);

            Control.Children.Add(element);
        }
    }
}
