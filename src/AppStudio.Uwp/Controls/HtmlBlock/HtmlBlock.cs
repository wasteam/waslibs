using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private void WriteFragment(HtmlFragment fragment, InlineCollection inlines)
        {
            foreach (var childFragment in fragment.Fragments)
            {
                //TODO: DON'T DO THIS IF PARENT IS A BLOCK ELEMENT TO
                if (childFragment.Name.ToLower() == "p" || childFragment.Name.ToLower() == "div")
                {
                    var p = new Paragraph();
                    _container.Blocks.Add(p);

                    WriteFragment(childFragment, p.Inlines);
                }
                else if (childFragment.Name.ToLower() == "text")
                {
                    //TODO: CHECK IF INLINES IS NOT NULL
                    //TODO: CHECK IF IS HtmlText
                    var text = ((HtmlText)childFragment).Content;
                    if (!string.IsNullOrEmpty(text))
                    {
                        inlines.Add(new Run
                        {
                            Text = text
                        }); 
                    }
                }
            }
        }

        private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            self.UpdateContent();
        }
    }
}
