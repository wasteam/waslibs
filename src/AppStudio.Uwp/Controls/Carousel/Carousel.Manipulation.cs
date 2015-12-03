using System;
using System.Linq;

using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    partial class Carousel
    {
        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MoveOffset(e.Delta.Translation.X);
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_offset < _slotWidth / 2.0)
            {
                AnimateNext(50);
            }
            else
            {
                AnimatePrev(50);
            }
        }

        private DateTime _idleTime = DateTime.MinValue;

        private void MoveOffset(double delta, double duration = 0)
        {
            if (duration > 0)
            {
                if (DateTime.Now > _idleTime)
                {
                    _idleTime = DateTime.Now.AddMilliseconds(duration + 50);
                    MoveOffsetInternal(delta, duration);
                }
                else
                {
                    return;
                }
            }
            else
            {
                MoveOffsetInternal(delta);
            }
        }

        private void MoveOffsetInternal(double delta, double duration = 0)
        {
            double x0 = GetLeftBound();
            double x1 = Math.Round(x0 + _slotWidth * (MaxItems + 2), 2);

            var controls = _container.Children.Cast<CarouselSlot>().ToArray();
            for (int n = 0; n < controls.Length; n++)
            {
                var control = controls[n];
                var x = Math.Round(control.X1 + delta, 2);
                if (x < x0 - 1)
                {
                    double inc = x - x0;
                    control.MoveX(x1 + inc);
                    control.Content = _items[(this.Index + (MaxItems + 1)).Mod(_items.Count)];
                    _index = _index.IncMod(_items.Count);
                    SetValue(IndexProperty, _index);
                }
                else if (x > x1 - 1)
                {
                    double inc = x - x1;
                    control.MoveX(x0 + inc);
                    control.Content = _items[(this.Index - 2).Mod(_items.Count)];
                    _index = _index.DecMod(_items.Count);
                    SetValue(IndexProperty, _index);
                }
                else
                {
                    control.MoveX(x, duration);
                }
            }

            _offset = Math.Round((_offset + delta).Mod(_slotWidth), 2);
        }

        private double GetLeftBound()
        {
            double contentWidth = _slotWidth * (MaxItems + 2);
            switch (this.AlignmentX)
            {
                case AlignmentX.Left:
                    return -_slotWidth;
                case AlignmentX.Right:
                    contentWidth -= _slotWidth;
                    return _container.ActualWidth - contentWidth;
                case AlignmentX.Center:
                default:
                    return -Math.Round((contentWidth - _container.ActualWidth) / 2.0, 2);
            }
        }
    }
}
