using AppStudio.Uwp.Controls.Html.Containers;
using AppStudio.Uwp.Controls.Html.Writers;
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

        private async static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlock;
            await self.UpdateContentAsync();
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

                WriteFragments(doc, new GridDocumentContainer(_container));
            }
        }

        private void WriteFragments(HtmlFragment fragment, DocumentContainer container)
        {
            foreach (var childFragment in fragment.Fragments)
            {
                var writer = HtmlWriter
                                .GetWriters()
                                .FirstOrDefault(w => w.Match(childFragment));

                var ctrl = writer?.GetControl(childFragment);

                if (ctrl == null)
                {
                    continue;
                }

                if (!container.CanAdd(ctrl))
                {
                    container = container.Find(ctrl);
                }

                container.Add(ctrl);

                var currentContainer = container.Create(ctrl);

                WriteFragments(childFragment, currentContainer);

                ////TODO: VERIFY IF IS EMPTY AND THE NOT ADD
                //container.Add(ctrl);
            }
        }
    }
}
