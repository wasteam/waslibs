using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;

namespace AppStudio.Uwp.Labs
{
    public class AutoHide : ContentControl
    {
        private DispatcherTimer _timer = null;

        private bool _isHidden = false;

        public AutoHide()
        {
            this.DefaultStyleKey = typeof(AutoHide);
        }

        #region Container
        public FrameworkElement Container
        {
            get { return (FrameworkElement)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register("Container", typeof(FrameworkElement), typeof(AutoHide), new PropertyMetadata(null));
        #endregion

        #region DelayInterval
        public double DelayInterval
        {
            get { return (double)GetValue(DelayIntervalProperty); }
            set { SetValue(DelayIntervalProperty, value); }
        }

        private static void DelayIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AutoHide;
            control.SetDelayInterval((double)e.NewValue);
        }

        public static readonly DependencyProperty DelayIntervalProperty = DependencyProperty.Register("DelayInterval", typeof(double), typeof(AutoHide), new PropertyMetadata(1500.0, DelayIntervalChanged));
        #endregion

        private void SetDelayInterval(double milliseconds)
        {
            if (_timer != null)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(milliseconds);
            }
        }

        protected override void OnApplyTemplate()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _timer = new DispatcherTimer();
                this.Loaded += OnLoaded;
                this.Unloaded += OnUnloaded;
            }

            this.SetDelayInterval(this.DelayInterval);

            base.OnApplyTemplate();
        }

        private void OnTimerTick(object sender, object e)
        {
            this.FadeOut(500);
            _isHidden = true;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _timer.Tick += OnTimerTick;
            _timer.Start();
            if (Container != null)
            {
                Container.PointerMoved += OnContainerPointerMoved;
            }
            else
            {
                Window.Current.CoreWindow.PointerMoved += OnCorePointerMoved;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= OnTimerTick;
            if (Container != null)
            {
                Container.PointerMoved -= OnContainerPointerMoved;
            }
            else
            {
                Window.Current.CoreWindow.PointerMoved -= OnCorePointerMoved;
            }
        }

        private void OnCorePointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            OnPointerMoved();
        }

        private void OnContainerPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            OnPointerMoved();
        }

        private void OnPointerMoved()
        {
            _timer.Start();
            if (_isHidden)
            {
                this.FadeIn(500);
                _isHidden = false;
            }
        }
    }
}
