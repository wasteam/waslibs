using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace AppStudio.Uwp.Controls
{
    public sealed class HtmlBlock : Control
    {
        private Grid _container;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(HtmlBlock), new PropertyMetadata(null, SourcePropertyChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public HtmlBlock()
        {
            this.DefaultStyleKey = typeof(HtmlBlock);
        }

        protected override async void OnApplyTemplate()
        {
            _container = base.GetTemplateChild("_container") as Grid;

            await UpdateContentAsync();

            base.OnApplyTemplate();
        }

        private async Task UpdateContentAsync()
        {
            if (_container != null && !string.IsNullOrEmpty(Source))
            {
                _container.RowDefinitions.Clear();
                _container.Children.Clear();

                var doc = await HtmlDocument.LoadAsync(Source);

                WriteFragment(doc, null);
            }
        }

        private void WriteFragment(HtmlFragment fragment, InlineCollection inlines, Run run = null)
        {
            foreach (var childFragment in fragment.Fragments)
            {
                switch (childFragment.Name.ToLower())
                {
                    case "p":
                    case "div":
                    case "ul":
                    case "ol":
                        WriteContainer(childFragment);
                        break;
                    case "text":
                        WriteText(childFragment.AsText(), inlines, run);
                        break;
                    case "span":
                        WriteSpan(childFragment, inlines);
                        break;
                    case "strong":
                    case "b":
                        WriteStrong(childFragment, inlines);
                        break;
                    case "a":
                        WriteAnchor(childFragment.AsNode(), inlines);
                        break;
                    case "img":
                        WriteImage(childFragment.AsNode());
                        break;
                    case "h1":
                        WriteH1(childFragment);
                        break;
                    case "h2":
                        WriteH2(childFragment);
                        break;
                    case "h3":
                        WriteH3(childFragment);
                        break;
                    case "h4":
                        WriteH4(childFragment);
                        break;
                    case "h5":
                        WriteH5(childFragment);
                        break;
                    case "h6":
                        WriteH6(childFragment);
                        break;
                    case "code":
                        WriteCode(childFragment, inlines);
                        break;
                    case "blockquote":
                        WriteBlockQuote(childFragment);
                        break;
                    case "li":
                        WriteListItem(childFragment);
                        break;
                    default:
                        //TODO: DEFAULT WRITTER
                        break;
                }
            }
        }

        private void WriteH1(HtmlFragment fragment)
        {
            WriteHeader(fragment, 2);
        }

        private void WriteH2(HtmlFragment fragment)
        {
            WriteHeader(fragment, 1.5f);
        }

        private void WriteH3(HtmlFragment fragment)
        {
            WriteHeader(fragment, 1.17f);
        }

        private void WriteH4(HtmlFragment fragment)
        {
            WriteHeader(fragment, 1);
        }

        private void WriteH5(HtmlFragment fragment)
        {
            WriteHeader(fragment, 0.83f);
        }

        private void WriteH6(HtmlFragment fragment)
        {
            WriteHeader(fragment, 0.67f);
        }

        private void WriteHeader(HtmlFragment fragment, float fontSizeWeight)
        {
            _container.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            var currentRow = _container.RowDefinitions.Count - 1;

            var textBlock = new RichTextBlock();

            var p = new Paragraph();
            p.FontSize *= fontSizeWeight;
            p.FontWeight = FontWeights.SemiBold;

            WriteFragment(fragment, p.Inlines);

            if (p.Inlines.Count > 0)
            {
                textBlock.Blocks.Add(p);

                Grid.SetRow(textBlock, currentRow);
                _container.Children.Add(textBlock);
            }
        }

        //TODO: REVIEW IF THIS HAS CONTAINER NESTED, LOST THE FORMAT
        private void WriteBlockQuote(HtmlFragment fragment)
        {
            _container.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            var currentRow = _container.RowDefinitions.Count - 1;

            var textBlock = new RichTextBlock();

            textBlock.Margin = new Thickness(20, 0, 0, 0);

            var p = new Paragraph();
            //p.FontStyle = FontStyle.Italic;

            WriteFragment(fragment, p.Inlines);

            if (p.Inlines.Count > 0)
            {
                textBlock.Blocks.Add(p);

                Grid.SetRow(textBlock, currentRow);
                _container.Children.Add(textBlock);
            }
        }

        private void WriteAnchor(HtmlNode node, InlineCollection inlines)
        {
            if (node != null && node.Attributes.ContainsKey("href"))
            {
                Hyperlink a = new Hyperlink();

                Uri uri;

                if (Uri.TryCreate(node.Attributes["href"], UriKind.Absolute, out uri))
                {
                    a.NavigateUri = uri;
                }

                WriteFragment(node, a.Inlines);

                inlines?.Add(a);
            }
        }

        private void WriteCode(HtmlFragment fragment, InlineCollection inlines)
        {
            var r = new Run();
            r.FontFamily = new FontFamily("Courier New");

            WriteFragment(fragment, inlines, r);
        }

        private void WriteStrong(HtmlFragment fragment, InlineCollection inlines)
        {
            var b = new Bold();
            inlines.Add(b);

            var r = new Run();

            WriteFragment(fragment, b.Inlines, r);
        }

        private void WriteSpan(HtmlFragment fragment, InlineCollection inlines)
        {
            //TODO: WHAT SPAN CONTROL DOES?
            var r = new Run();
            r.Foreground = new SolidColorBrush(Colors.Lime);

            WriteFragment(fragment, inlines, r);
        }

        private static void WriteText(HtmlText text, InlineCollection inlines, Run run)
        {
            if (text != null && !string.IsNullOrEmpty(text.Content))
            {
                //TODO: REVIEW THIS
                if (run == null)
                {
                    var r = new Run();
                    r.Text = text.Content;

                    inlines?.Add(r);
                }

                else
                {
                    run.Text = text.Content;

                    inlines?.Add(run);
                }
            }
        }

        private void WriteImage(HtmlNode node)
        {
            if (node != null && node.Attributes.ContainsKey("src"))
            {
                Uri uri;

                if (Uri.TryCreate(node.Attributes["src"], UriKind.Absolute, out uri))
                {
                    _container.RowDefinitions.Add(new RowDefinition
                    {
                        Height = GridLength.Auto
                    });

                    var currentRow = _container.RowDefinitions.Count - 1;

                    var image = new ImageEx
                    {
                        Source = new BitmapImage(uri),
                        Stretch = Stretch.Uniform
                    };

                    Grid.SetRow(image, currentRow);
                    _container.Children.Add(image);
                }
            }
        }

        private void WriteListItem(HtmlFragment fragment)
        {
            _container.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            var currentRow = _container.RowDefinitions.Count - 1;

            var textBlock = new RichTextBlock();

            var p = new Paragraph();
            p.Inlines.Add(new Run
            {
                Text = "\u25CF  ",
            });

            WriteFragment(fragment, p.Inlines);

            if (p.Inlines.Count > 0)
            {
                textBlock.Blocks.Add(p);

                Grid.SetRow(textBlock, currentRow);
                _container.Children.Add(textBlock);
            }
        }

        private void WriteContainer(HtmlFragment fragment)
        {
            _container.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            var currentRow = _container.RowDefinitions.Count - 1;

            var textBlock = new RichTextBlock();

            var p = new Paragraph();

            WriteFragment(fragment, p.Inlines);

            if (p.Inlines.Count > 0)
            {
                textBlock.Blocks.Add(p);

                Grid.SetRow(textBlock, currentRow);
                _container.Children.Add(textBlock);
            }
        }

        private async static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            await self.UpdateContentAsync();
        }
    }
}
