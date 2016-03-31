using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
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

        protected override void OnApplyTemplate()
        {
            _container = base.GetTemplateChild("_container") as Grid;

            UpdateContent();

            base.OnApplyTemplate();
        }

        private void UpdateContent()
        {
            if (_container != null && !string.IsNullOrEmpty(Source))
            {
                _container.RowDefinitions.Clear();
                _container.Children.Clear();

                var doc = HtmlDocument.Load(Source);

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
                    default:
                        break;
                }
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

        private void WriteStrong(HtmlFragment childFragment, InlineCollection inlines)
        {
            var b = new Bold();
            inlines.Add(b);

            var r = new Run();

            WriteFragment(childFragment, b.Inlines, r);
        }

        private void WriteSpan(HtmlFragment childFragment, InlineCollection inlines)
        {
            //TODO: WHAT SPAN CONTROL DOES?
            var r = new Run();
            r.Foreground = new SolidColorBrush(Colors.Lime);

            WriteFragment(childFragment, inlines, r);
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
                _container.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });

                var currentRow = _container.RowDefinitions.Count - 1;

                var image = new ImageEx
                {
                    Source = new BitmapImage(new Uri(node.Attributes["src"], UriKind.Absolute)),
                    Stretch = Stretch.Uniform
                };

                Grid.SetRow(image, currentRow);
                _container.Children.Add(image);
            }
        }

        private void WriteContainer(HtmlFragment childFragment)
        {
            _container.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            var currentRow = _container.RowDefinitions.Count - 1;

            var textBlock = new RichTextBlock();

            var p = new Paragraph();

            WriteFragment(childFragment, p.Inlines);

            if (p.Inlines.Count > 0)
            {
                textBlock.Blocks.Add(p);

                Grid.SetRow(textBlock, currentRow);
                _container.Children.Add(textBlock);
            }
        }

        private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            self.UpdateContent();
        }
    }
}
