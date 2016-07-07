using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private bool _isBusy = false;

        #region Position
        public double Position
        {
            get { return _panelContainer.GetTranslateX(); }
            set
            {
                _headerContainer.TranslateX(value);
                _panelContainer.TranslateX(value);
            }
        }
        #endregion

        #region Offset
        public double Offset
        {
            get
            {
                double position = this.Position % this.ItemWidthEx;
                if (Math.Sign(position) > 0)
                {
                    return this.ItemWidthEx - position;
                }
                return -position;
            }
        }
        #endregion

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (_isBusy)
            {
                return;
            }

            if (_panel.ItemsFitContent)
            {
                return;
            }

            var point = e.GetCurrentPoint(this);
            int sign = Math.Sign(point.Properties.MouseWheelDelta);
            if (sign > 0)
            {
                _headerContainer.TranslateDeltaX(1);
                _panelContainer.TranslateDeltaX(1);
                AnimatePrev();
            }
            else
            {
                _headerContainer.TranslateDeltaX(-1);
                _panelContainer.TranslateDeltaX(-1);
                AnimateNext();
            }
            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_panel.ItemsFitContent)
            {
                return;
            }

            double deltaX = e.Delta.Translation.X;

            if (e.IsInertial)
            {
                e.Complete();
            }
            else
            {
                if (Math.Abs(e.Cumulative.Translation.X) >= this.ItemWidthEx)
                {
                    e.Complete();
                }
                else
                {
                    _headerContainer.TranslateDeltaX(deltaX);
                    _panelContainer.TranslateDeltaX(deltaX);
                    if (Math.Sign(deltaX) > 0)
                    {
                        _tabsContainer.TranslateDeltaX(deltaX * _tabs.PrevTabWidth / this.ItemWidthEx);
                    }
                    else
                    {
                        _tabsContainer.TranslateDeltaX(deltaX * _tabs.SelectedTabWidth / this.ItemWidthEx);
                    }
                }
            }
            e.Handled = true;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_panel.ItemsFitContent)
            {
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
                if (this.Offset > this.ItemWidthEx / 2.0)
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

        private async void AnimateNext(double duration = 500)
        {
            double delta = this.ItemWidthEx - this.Offset;
            double position = Position - delta;
            duration = duration * delta / this.ItemWidthEx;

            _isBusy = true;
            var t1 = _headerContainer.AnimateXAsync(position, duration);
            var t2 = AnimateTabsNext(duration * 1);
            var t3 = _panelContainer.AnimateXAsync(position, duration);
            await Task.WhenAll(t1, t2, t3);
            _isBusy = false;

            this.Index = (int)(-position / this.ItemWidthEx);
            _tabsContainer.TranslateX(0);
        }

        private async void AnimatePrev(double duration = 500)
        {
            double delta = this.Offset;
            double position = Position + delta;
            duration = duration * delta / this.ItemWidthEx;

            _isBusy = true;
            var t1 = _headerContainer.AnimateXAsync(position, duration);
            var t2 = AnimateTabsPrev(duration * 1);
            var t3 = _panelContainer.AnimateXAsync(position, duration);
            await Task.WhenAll(t1, t2, t3);
            _isBusy = false;

            this.Index = (int)(-position / this.ItemWidthEx);
            _tabsContainer.TranslateX(0);
        }

        private async Task AnimateTabsNext(double duration)
        {
            double x = _tabsContainer.GetTranslateX();
            if (x > 0)
            {
                await _tabsContainer.AnimateXAsync(0, duration);
            }
            else
            {
                await _tabsContainer.AnimateXAsync(-_tabs.SelectedTabWidth, duration);
            }
        }

        private async Task AnimateTabsPrev(double duration)
        {
            double x = _tabsContainer.GetTranslateX();
            if (x < 0)
            {
                await _tabsContainer.AnimateXAsync(0, duration);
            }
            else
            {
                await _tabsContainer.AnimateXAsync(_tabs.PrevTabWidth, duration);
            }
        }
    }
}
