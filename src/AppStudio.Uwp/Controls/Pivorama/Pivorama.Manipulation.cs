using System;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            double velocity = e.Velocities.Linear.X;
            double offset = Math.Abs(_offset);

            if (Math.Abs(velocity) > 20000000.0)
            {
                if (Math.Sign(velocity) > 0)
                {
                    e.TranslationBehavior.DesiredDisplacement = _slotWidth * _items.Count - offset;
                }
                else
                {
                    e.TranslationBehavior.DesiredDisplacement = _slotWidth * (_items.Count - 1) + offset;
                }
            }
            else
            {
                if (Math.Abs(velocity) > 0.5)
                {
                    if (Math.Sign(velocity) > 0)
                    {
                        e.TranslationBehavior.DesiredDisplacement = _slotWidth * 1 - offset;
                    }
                    else
                    {
                        e.TranslationBehavior.DesiredDisplacement = _slotWidth * 0 + offset;
                    }
                }
                else
                {
                    e.TranslationBehavior.DesiredDeceleration = 2.0;
                }
            }
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MoveOffset(e.Delta.Translation.X);
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_offset > 0.9 && _offset < _slotWidth - 0.9)
            {
                if (_offset < _slotWidth / 2.0)
                {
                    AnimateNext(150);
                }
                else
                {
                    AnimatePrev(150);
                }
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
                    MoveOffsetInternal(delta, this.SelectedIndex, duration);
                }
            }
            else
            {
                MoveOffsetInternal(delta, this.SelectedIndex);
            }
        }

        private void MoveOffsetInternal(double delta, int currentIndex, double duration = 0)
        {
            if (_items.Count > 0)
            {
                double x0 = GetLeftBound();
                double x1 = Math.Round(x0 + _slotWidth * (_items.Count + 2), 2);

                int newIndex = currentIndex;
                var controls = _container.Children.Cast<PivoramaItem>().OrderBy(r => r.X).ToArray();
                for (int n = 0; n < controls.Length; n++)
                {
                    var control = controls[n];
                    var x = Math.Round(control.X + delta, 2);
                    if (x < x0 - 1)
                    {
                        double inc = x - x0;
                        control.MoveX(x1 + inc);
                        control.Header = _items[(currentIndex + (_items.Count + 1)).Mod(_items.Count)];
                        control.Content = _items[(currentIndex + (_items.Count + 1)).Mod(_items.Count)];
                        newIndex = currentIndex.IncMod(_items.Count);
                    }
                    else if (x > x1 - 1)
                    {
                        double inc = x - x1;
                        control.MoveX(x0 + inc);
                        control.Header = _items[(currentIndex - 2).Mod(_items.Count)];
                        control.Content = _items[(currentIndex - 2).Mod(_items.Count)];
                        newIndex = currentIndex.DecMod(_items.Count);
                    }
                    else
                    {
                        control.MoveX(x, duration);
                    }

                    ShowHeader(control, x < this.ActualWidth);
                    if (n == 1 && this.ActualWidth < _slotWidth)
                    {
                        control.TabsVisibility = Visibility.Visible;
                    }
                    else
                    {
                        control.TabsVisibility = Visibility.Collapsed;
                    }

                    control.Index = _items.IndexOf(control.Content).IncMod(_items.Count);
                }
                _offset = Math.Round((_offset + delta).Mod(_slotWidth), 2);

                _disableSelectedIndexCallback = true;
                this.SelectedIndex = newIndex;
                _disableSelectedIndexCallback = false;

                //ApplySelection();
            }
        }
    }
}
