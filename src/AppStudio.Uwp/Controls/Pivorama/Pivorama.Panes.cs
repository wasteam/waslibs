using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private double _offset = 0;
        private double _slotWidth = 1;

        public void MoveNext()
        {
            this.SelectedIndex = this.SelectedIndex.IncMod(_items.Count);
        }
        public void MovePrev()
        {
            this.SelectedIndex = this.SelectedIndex.DecMod(_items.Count);
        }

        public void AnimateNext(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(-delta, duration);
        }
        public void AnimatePrev(double duration = 50)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(delta, duration);
        }

        public async void AnimateNextPage(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < _items.Count * 4; n++)
            {
                MoveOffsetInternal(-delta / 4.0, this.SelectedIndex);
                await System.Threading.Tasks.Task.Delay(10);
            }
        }
        public async void AnimatePrevPage(double duration = 50)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < _items.Count * 4; n++)
            {
                MoveOffsetInternal(delta / 4.0, this.SelectedIndex);
                await System.Threading.Tasks.Task.Delay(10);
            }
        }
    }
}
