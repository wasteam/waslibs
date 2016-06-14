using System;
using System.Linq;

using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Labs
{
    public partial class AccordionPanel : Panel
    {
        public AccordionPanel()
        {
            this.PointerWheelChanged += OnPointerWheelChanged;
            this.ManipulationMode = ManipulationModes.TranslateY;
            this.ManipulationStarted += OnManipulationStarted;
            this.ManipulationDelta += OnManipulationDelta;
            this.ManipulationCompleted += OnManipulationCompleted;
        }

        private double TabHeight { get; set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            var sample = base.Children.LastOrDefault() as AccordionItem;
            if (sample != null)
            {
                this.TabHeight = sample.HeaderHeight;
            }

            double contentHeight = GetContentHeight(availableSize.Height);
            foreach (var item in base.Children)
            {
                var size = new Size(availableSize.Width, contentHeight);
                item.Measure(size);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double contentHeight = GetContentHeight(finalSize.Height);
            double y = 0;

            foreach (var item in base.Children.Reverse())
            {
                var rect = new Rect(0, y, finalSize.Width, contentHeight);
                item.Arrange(rect);
                y += TabHeight;
            }

            int n = 0;
            double ty = -(finalSize.Height - MaxTabs * TabHeight);
            foreach (AccordionItem item in base.Children.Reverse())
            {
                if (n < this.SelectedIndex)
                {
                    item.IsUp = true;
                    item.TranslateY(ty);
                }
                else
                {
                    item.IsUp = false;
                    item.TranslateY(0);
                }
                n++;
            }

            int index = Math.Max(0, this.SelectedIndex - 2);
            index = Math.Min(index, base.Children.Count - 4);
            double cy = index * this.TabHeight;
            this.AnimateY(-cy);

            return finalSize;
        }

        private double GetContentHeight(double availableHeight)
        {
            return availableHeight - MaxTabs * TabHeight + TabHeight;
        }

        private double GetTopBound()
        {
            return -(this.ActualHeight - MaxTabs * TabHeight);
        }
    }
}
