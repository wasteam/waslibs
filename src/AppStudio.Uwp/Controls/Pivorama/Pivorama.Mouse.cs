using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private bool _isArrowVisible = false;
        private bool _isArrowOver = false;

        private DispatcherTimer _fadeTimer = null;

        #region Create/Dispose FadeTimer
        private void CreateFadeTimer()
        {
            _fadeTimer = new DispatcherTimer();
            _fadeTimer.Interval = TimeSpan.FromMilliseconds(3000);
            _fadeTimer.Tick += OnFadeTimerTick;
        }

        private void DisposeFadeTimer()
        {
            var fadeTimer = _fadeTimer;
            _fadeTimer = null;
            if (fadeTimer != null)
            {
                fadeTimer.Stop();
            }
        }
        #endregion

        #region ArrowPointerEntered/ArrowPointerExited
        private void OnArrowPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _isArrowOver = true;
        }

        private void OnArrowPointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isArrowOver = false;
        }
        #endregion

        #region LeftClick/RightClick
        private void OnLeftClick(object sender, RoutedEventArgs e)
        {
            if (_isBusy || _panel.ItemsFitContent)
            {
                return;
            }
            _headerContainer.TranslateDeltaX(1);
            _panelContainer.TranslateDeltaX(1);
            AnimatePrev();
        }

        private void OnRightClick(object sender, RoutedEventArgs e)
        {
            if (_isBusy || _panel.ItemsFitContent)
            {
                return;
            }
            _headerContainer.TranslateDeltaX(-1);
            _panelContainer.TranslateDeltaX(-1);
            AnimateNext();
        }
        #endregion

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_panel.ItemsFitContent || _headerContainer.Visibility == Visibility.Collapsed)
            {
                if (_isArrowVisible)
                {
                    _isArrowVisible = false;
                    _arrows.FadeOut();
                }
                return;
            }

            if (!_isArrowVisible)
            {
                _arrows.FadeIn();
                _isArrowVisible = true;
            }
            this._fadeTimer.Start();
        }

        private void OnFadeTimerTick(object sender, object e)
        {
            if (_isArrowVisible && !_isArrowOver)
            {
                _isArrowVisible = false;
                _arrows.FadeOut();
            }
        }
    }
}
