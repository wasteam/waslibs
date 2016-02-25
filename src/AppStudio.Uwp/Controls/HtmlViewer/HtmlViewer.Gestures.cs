using System;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private bool _cancelManipulation = false;

        private async void OnPointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);

            double offset = -point.Properties.MouseWheelDelta / 20.0;
            double delta = -Math.Sign(offset) * 20;

            for (int n = 0; n < Math.Abs(offset); n++)
            {
                await TranslateDelta(delta);
                await Task.Delay(10);
            }
        }

        private void OnAdornManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            _glass.Background = new SolidColorBrush(Colors.Transparent);
            _cancelManipulation = false;
        }

        private async void OnAdornManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double delta = e.Delta.Translation.Y;
            await TranslateDelta(delta);
            if (_cancelManipulation)
            {
                e.Complete();
            }
        }

        private void OnAdornManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _glass.Background = null;
        }


        private void OnGlassManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            _glass.Background = null;
            _cancelManipulation = true;
        }

        private void OnGlassManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _glass.Background = null;
            _cancelManipulation = true;
        }

        private void OnGlassPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _glass.Background = null;
            _cancelManipulation = true;
        }


        private async Task TranslateDelta(double delta)
        {
            if (_webView != null && _isHtmlLoaded)
            {
                await _webView.InvokeScriptAsync("verticalScrollBy", (-delta).ToInvariantString());
            }
        }
    }
}
