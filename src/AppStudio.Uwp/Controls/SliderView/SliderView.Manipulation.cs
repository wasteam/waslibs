using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class SliderView
    {
        #region Position
        public double Position
        {
            get { return _panel.GetTranslateX(); }
            set { _panel.TranslateX(value); }
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

        private int _direction = 0;

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _direction = Math.Sign(e.Delta.Translation.X);
            e.Handled = true;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_direction > 0 && this.Position >= 0)
            {
                e.Handled = true;
                return;
            }

            if (_direction < 0 && this.Position <= -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth))
            {
                e.Handled = true;
                return;
            }

            if (_direction > 0)
            {
                _panel.TranslateDeltaX(0.01);
                AnimatePrev();
            }
            else
            {
                _panel.TranslateDeltaX(-0.01);
                AnimateNext();
            }
            e.Handled = true;
        }

        private bool _isBusy = false;

        private async void AnimateNext(double duration = 150)
        {
            if (!_isBusy)
            {
                _isBusy = true;
                double delta = this.ItemWidth - this.Offset;
                double position = Position - delta;
                duration = duration * delta / this.ItemWidth;

                position = Math.Max(position, -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth));
                await _panel.AnimateXAsync(position, duration);

                SetArrowsOpacity(position);

                this.Index = (int)(-position / this.ItemWidth);
                _isBusy = false;
            }
        }

        private async void AnimatePrev(double duration = 150)
        {
            if (!_isBusy)
            {
                _isBusy = true;
                double delta = this.Offset;
                double position = Position + delta;
                duration = duration * delta / this.ItemWidth;

                position = Math.Max(position, -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth));
                await _panel.AnimateXAsync(position, duration);

                SetArrowsOpacity(position);

                this.Index = (int)(-position / this.ItemWidth);
                _isBusy = false;
            }
        }

        private void SetArrowsOpacity(double position)
        {
            if (position >= 0)
            {
                _left.FadeOut();
            }
            else
            {
                _left.FadeIn();
            }

            if (position <= -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth))
            {
                _right.FadeOut();
            }
            else
            {
                _right.FadeIn();
            }
        }
    }
}
