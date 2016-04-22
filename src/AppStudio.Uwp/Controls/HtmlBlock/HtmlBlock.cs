using AppStudio.Uwp.Controls.Html.Containers;
using AppStudio.Uwp.Controls.Html.Writers;
using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private DocumentStyle _docStyles;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(HtmlBlock), new PropertyMetadata(null, SourcePropertyChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private async static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            await self.UpdateContentAsync();
        }

        public static readonly DependencyProperty DocumentStyleProperty = DependencyProperty.Register("DocumentStyle", typeof(DocumentStyle), typeof(HtmlBlock), new PropertyMetadata(new DocumentStyle(), DocumentStylesChanged));

        public DocumentStyle DocumentStyle
        {
            get { return (DocumentStyle)GetValue(DocumentStyleProperty); }
            set { SetValue(DocumentStyleProperty, value); }
        }

        internal static readonly DependencyProperty DefaultDocumentStyleProperty = DependencyProperty.Register("DefaultDocumentStyle", typeof(DocumentStyle), typeof(HtmlBlock), new PropertyMetadata(new DocumentStyle(), DocumentStylesChanged));

        internal DocumentStyle DefaultDocumentStyle
        {
            get { return (DocumentStyle)GetValue(DefaultDocumentStyleProperty); }
            set { SetValue(DefaultDocumentStyleProperty, value); }
        }

        private static void DocumentStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            self._docStyles.Merge(self.DefaultDocumentStyle, self.DocumentStyle);
        }

        public HtmlBlock()
        {
            this.DefaultStyleKey = typeof(HtmlBlock);

            HtmlWriterFactory.Host = this;
            _docStyles = new DocumentStyle();
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = base.GetTemplateChild("_container") as Grid;

            await UpdateContentAsync();
        }

        private async Task UpdateContentAsync()
        {
            if (_container != null && !string.IsNullOrEmpty(Source))
            {
                _container.RowDefinitions.Clear();
                _container.ColumnDefinitions.Clear();
                _container.Children.Clear();

                try
                {
                    var doc = await HtmlDocument.LoadAsync(Source);

                    HtmlFragment body = doc?.Body;
                    if (body == null)
                    {
                        body = doc;
                    }

                    WriteFragments(body, new GridDocumentContainer(_container));
                }
                catch (Exception ex)
                {
                    //TODO: RENDER ERROR?
                    Debug.WriteLine($"HtmlBlock: Error rendering document. Ex: {ex.ToString()}");
                }
            }
        }

        private void WriteFragments(HtmlFragment fragment, DocumentContainer parentContainer)
        {
            if (parentContainer != null)
            {
                foreach (var childFragment in fragment.Fragments)
                {
                    var writer = HtmlWriterFactory.Find(childFragment);

                    var ctrl = writer?.GetControl(childFragment);

                    if (ctrl != null)
                    {
                        if (!parentContainer.CanContain(ctrl))
                        {
                            var antecesorContainer = parentContainer.Find(ctrl);

                            if (antecesorContainer == null)
                            {
                                continue;
                            }
                            else
                            {
                                parentContainer = antecesorContainer;
                            }
                        }

                        var currentContainer = parentContainer.Append(ctrl);

                        WriteFragments(childFragment, currentContainer);

                        if (DocumentStyle != null)
                        {
                            writer?.ApplyStyles(_docStyles, ctrl, childFragment);
                        }
                    }
                }
            }
        }
    }
}
