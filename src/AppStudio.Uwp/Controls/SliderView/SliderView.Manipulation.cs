using System;

using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class SliderView
    {
        #region Position
        public double Position
        {
            get { return _panel.GetTranslateX(); }
            set
            {
                _panel.TranslateX(value);
            }
        }
        #endregion

        #region Offset
        public double Offset
        {
            get
            {
                double position = this.Position % this.ItemWidth;
                if (Math.Sign(position) > 0)
                {
                    return this.ItemWidth - position;
                }
                return -position;
            }
        }
        #endregion

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double deltaX = e.Delta.Translation.X;

            if (this.ActualWidth > this.ItemWidth * _panel.ItemsCount)
            {
                e.Complete();
                e.Handled = true;
                return;
            }

            if (this.Position + deltaX > this.ItemWidth / 2.5)
            {
                e.Complete();
                e.Handled = true;
                return;
            }

            if (this.Position + deltaX < -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth) - this.ItemWidth / 2.5)
            {
                e.Complete();
                e.Handled = true;
                return;
            }

            int itemsPerThird = (int)Math.Ceiling(this.ActualWidth / this.ItemWidth);
            if (Math.Abs(e.Cumulative.Translation.X) >= this.ItemWidth * itemsPerThird)
            {
                e.Complete();
            }
            else
            {
                _panel.TranslateDeltaX(deltaX);
            }

            e.Handled = true;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (this.ActualWidth > this.ItemWidth * _panel.ItemsCount)
            {
                e.Handled = true;
                return;
            }

            if (this.Position > 0)
            {
                AnimateNext();
                e.Handled = true;
                return;
            }

            if (this.Position < -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth))
            {
                AnimatePrev();
                e.Handled = true;
                return;
            }

            if (e.IsInertial)
            {
                if (Math.Sign(e.Cumulative.Translation.X) < 0)
                {
                    AnimateNext();
                }
                else
                {
                    AnimatePrev();
                }
            }
            else
            {
                if (this.Offset > this.ItemWidth / 2.0)
                {
                    AnimateNext();
                }
                else
                {
                    AnimatePrev();
                }
            }
            e.Handled = true;
        }

        private bool _isBusy = false;

        private async void AnimateNext(double duration = 500)
        {
            if (!_isBusy)
            {
                _isBusy = true;
                double delta = this.ItemWidth - this.Offset;
                double position = Position - delta;
                duration = duration * delta / this.ItemWidth;

                position = Math.Max(position, -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth));
                await _panel.AnimateXAsync(position, duration);

                this.Index = (int)(-position / this.ItemWidth);
                _isBusy = false;
            }
        }

        private async void AnimatePrev(double duration = 500)
        {
            if (!_isBusy)
            {
                _isBusy = true;
                double delta = this.Offset;
                double position = Position + delta;
                duration = duration * delta / this.ItemWidth;

                position = Math.Max(position, -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth));
                await _panel.AnimateXAsync(position, duration);

                this.Index = (int)(-position / this.ItemWidth);
                _isBusy = false;
            }
        }
    }
}
