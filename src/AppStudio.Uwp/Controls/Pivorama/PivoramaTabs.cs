using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public class PivoramaTabs : PivoramaPanel
    {
        public double PrevTabWidth { get; private set; }
        public double SelectedTabWidth { get; private set; }

        protected override int MaxItems
        {
            get { return 24; }
        }

        #region SelectedItemTemplate
        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(PivoramaTabs), new PropertyMetadata(null));
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            this.EnsurePanes();

            int index = this.Index;
            int count = this.Items.Count;

            double x = 0;
            double maxHeight = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(index + n).Mod(MaxItems)] as ContentControl;
                    if (n <= count)
                    {
                        int inx = (index + n - 1).Mod(count);
                        pane.ContentTemplate = n == 1 ? ItemTemplate : SelectedItemTemplate;
                        pane.Content = this.Items[inx];
                        pane.Tag = inx;

                        pane.Measure(availableSize);
                        maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);
                        x += pane.DesiredSize.Width;
                    }
                    else
                    {
                        pane.ContentTemplate = null;
                        pane.Content = null;
                        pane.Tag = null;
                        pane.Measure(availableSize);
                    }

                    if (n == 0)
                    {
                        PrevTabWidth = pane.DesiredSize.Width;
                    }
                    if (n == 1)
                    {
                        SelectedTabWidth = pane.DesiredSize.Width;
                    }
                }
            }

            return new Size(x, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int inx = this.Index;
            int count = this.Items.Count;

            double x = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(inx + n).Mod(MaxItems)] as ContentControl;
                    if (n == 0)
                    {
                        x = -pane.DesiredSize.Width;
                    }

                    pane.Arrange(new Rect(x, 0, pane.DesiredSize.Width, finalSize.Height));
                    x += pane.DesiredSize.Width;
                }
            }

            return new Size(0, finalSize.Height);
        }
    }
}
