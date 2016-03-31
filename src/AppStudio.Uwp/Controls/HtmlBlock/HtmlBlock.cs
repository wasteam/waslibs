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

namespace AppStudio.Uwp.Controls
{
    public sealed class HtmlBlock : Control
    {
        private RichTextBlock _container;

        private RichTextBlock Container
        {
            get
            {
                return base.GetTemplateChild("_container") as RichTextBlock;
            }
        }

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
            _container = base.GetTemplateChild("_container") as RichTextBlock;

            UpdateContent();

            base.OnApplyTemplate();
        }

        private void UpdateContent()
        {
            if (_container != null && !string.IsNullOrEmpty(Source))
            {
                _container.Blocks.Clear();

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
                    default:
                        break;
                }
            }
        }

        private void WriteAnchor(HtmlNode node, InlineCollection inlines)
        {
            if (node != null && node.Attributes.ContainsKey("href"))
            {
                Hyperlink a = new Hyperlink
                {
                    NavigateUri = new Uri(node.Attributes["href"]),
                };

                WriteFragment(node, a.Inlines);

                inlines?.Add(a); 
            }
        }

        private void WriteStrong(HtmlFragment childFragment, InlineCollection inlines)
        {
            var r = new Run();
            r.FontWeight = Windows.UI.Text.FontWeights.SemiBold;

            WriteFragment(childFragment, inlines, r);
        }

        private void WriteSpan(HtmlFragment childFragment, InlineCollection inlines)
        {
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

        private void WriteContainer(HtmlFragment childFragment)
        {
            var p = new Paragraph();
            WriteFragment(childFragment, p.Inlines);
            if (p.Inlines.Count > 0)
            {
                _container.Blocks.Add(p);
            }
        }

        private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            self.UpdateContent();
        }
    }
}
