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
        //TODO: REVIEW THEESE FLAGS
        private bool _createTextBlock;
        private bool _createBlock;

        public GridDocumentContainer(Grid ctrl) : base(ctrl)
        {
        }

        public override void Add(DependencyObject ctrl)
        {
            if (ctrl is FrameworkElement)
            {
                var element = ctrl as FrameworkElement;

                Control.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });

                var currentRow = Control.RowDefinitions.Count - 1;


                //TODO: WHAT IF ELEMENTS HAS NOT TEXT CHILDREN??
                Grid.SetRow(element, currentRow);
                Control.Children.Add(element);

                _createTextBlock = true;

            }
            else if (ctrl is Block)
            {
                //TODO: VERIFY CONTAINER IS NOT NULL

                var textBlock = FindOrCreateTextBlock();

                var block = ctrl as Block;
                textBlock.Blocks.Add(block);

                _createBlock = true;
            }
            else if (ctrl is Inline)
            {
                //TODO: VERIFY CONTAINER IS NOT NULL
                var textBlock = FindOrCreateTextBlock();

                var p = textBlock.Blocks
                                    .OfType<Paragraph>()
                                    .LastOrDefault();

                if (_createBlock || p == null)
                {
                    p = new Paragraph();
                    textBlock.Blocks.Add(p);

                    _createBlock = false;

                    //TODO: TRIM START
                }

                var inline = ctrl as Inline;
                p.Inlines.Add(inline);
            }
        }

        private RichTextBlock FindOrCreateTextBlock()
        {
            var textBlock = Control.Children
                                        .OfType<RichTextBlock>()
                                        .LastOrDefault();

            if (_createTextBlock || textBlock == null)
            {
                textBlock = new RichTextBlock();

                Control.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });

                var currentRow = Control.RowDefinitions.Count - 1;


                //TODO: WHAT IF ELEMENTS HAS NOT TEXT CHILDREN??
                Grid.SetRow(textBlock, currentRow);
                Control.Children.Add(textBlock);

                _createTextBlock = false;
            }


            return textBlock;

        }

        public override bool CanAdd(DependencyObject ctrl)
        {
            return true;
        }
    }
}
