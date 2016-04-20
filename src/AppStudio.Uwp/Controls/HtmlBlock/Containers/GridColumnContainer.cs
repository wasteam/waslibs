using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls.Html.Containers
{
    class GridColumnContainer : DocumentContainer<GridColumn>
    {
        public GridColumnContainer(GridColumn ctrl) : base(ctrl)
        {
        }

        public override bool CanContain(DependencyObject ctrl)
        {
            return !(ctrl is GridRow || ctrl is GridColumn);
        }

        protected override void Add(DependencyObject ctrl)
        {
            if (Control.Row != null && Control.Row.Container != null)
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
            }
        }

        private Paragraph FindOrCreateParagraph(RichTextBlock textBlock)
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
            var border = Control.Row.Container.GetChild<Border>(Control.Index, Control.Row.Index);
            var grid = border?.Child as Grid;
            var textBlock = grid?.GetChild<RichTextBlock>(0, grid.RowDefinitions.Count - 1);
            if (textBlock == null)
            {
                textBlock = new RichTextBlock();
                AddChild(textBlock);
            }
            return textBlock;
        }

        private void AddChild(FrameworkElement element)
        {
            var border = Control.Row.Container.GetChild<Border>(Control.Index, Control.Row.Index);
            if (border == null)
            {
                border = new Border
                {
                    Child = new Grid()
                };

                Grid.SetColumn(border, Control.Index);
                Grid.SetRow(border, Control.Row.Index);
                if (Control.ColSpan > 0)
                {
                    Grid.SetColumnSpan(border, Control.ColSpan);
                }
                if (Control.RowSpan > 0)
                {
                    Grid.SetRowSpan(border, Control.RowSpan);
                }

                Control.Row.Container.Children.Add(border);
            }

            var grid = border.Child as Grid;

            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
            Grid.SetRow(element, grid.RowDefinitions.Count - 1);

            grid.Children.Add(element);
        }
    }
}
