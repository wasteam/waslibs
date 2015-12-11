using System;
using System.Collections.Generic;

using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    partial class Carousel
    {
        const double MARGIN_FACTOR = 1.0;

        private double _slotWidth = 1;

        private double _offset = 0;

        private IEnumerable<Point> GetPositions(double slotWidth)
        {
            double x0 = GetLeftBound();
            for (int n = 0; n < (MaxItems + 2); n++)
            {
                yield return new Point(x0, 0);
                x0 += slotWidth;
            }
        }

        public void MoveNext()
        {
            this.SelectedIndex = this.SelectedIndex.IncMod(_items.Count);
        }
        public void MovePrev()
        {
            this.SelectedIndex = this.SelectedIndex.DecMod(_items.Count);
        }

        public void AnimateNext(double duration = 150)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(-delta, duration);
        }
        public void AnimatePrev(double duration = 150)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(delta, duration);
        }

        public async void AnimateNextPage(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < MaxItems * 4; n++)
            {
                MoveOffsetInternal(-delta / 4.0);
                await System.Threading.Tasks.Task.Delay(10);
            }
        }
    }
}
