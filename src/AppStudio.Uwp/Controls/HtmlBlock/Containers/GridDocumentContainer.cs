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
        //TODO: REVIEW THESE FLAGS
        private bool _createTextBlock;
        private bool _createParagraph;

        public GridDocumentContainer(Grid ctrl) : base(ctrl)
        {
        }

        public override bool CanContain(DependencyObject ctrl)
        {
            return true;
        }

        protected override void Add(DependencyObject ctrl)
        {
            if (ctrl is FrameworkElement)
            {
                AddChild(ctrl as FrameworkElement);

                _createTextBlock = true;
            }
            else if (ctrl is Block)
            {
                var textBlock = FindOrCreateTextBlock();

                textBlock.Blocks.Add(ctrl as Block);

                _createParagraph = true;
            }
            else if (ctrl is Inline)
            {
                var textBlock = FindOrCreateTextBlock();
                var p = FindOrCreateParagraph(textBlock);

                p.Inlines.Add(ctrl as Inline);
            }
        }

        private Paragraph FindOrCreateParagraph(RichTextBlock textBlock)
        {
            var p = textBlock.Blocks
                                .OfType<Paragraph>()
                                .LastOrDefault();

            if (_createParagraph || p == null)
            {
                p = new Paragraph();
                textBlock.Blocks.Add(p);

                _createParagraph = false;

                //TODO: TRIM START
            }

            return p;
        }

        private RichTextBlock FindOrCreateTextBlock()
        {
            var textBlock = Control.Children
                                        .OfType<RichTextBlock>()
                                        .LastOrDefault();

            if (_createTextBlock || textBlock == null)
            {
                textBlock = new RichTextBlock();

                AddChild(textBlock);

                _createTextBlock = false;
            }

            return textBlock;
        }

        private void AddChild(FrameworkElement element)
        {
            Control.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            //TODO: WHAT IF ELEMENTS HAS NOT TEXT CHILDREN??
            Grid.SetRow(element, Control.RowDefinitions.Count - 1);
            Control.Children.Add(element);
        }
    }
}
