using System;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private bool _cancelManipulation = false;

        private async void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            var offset = -point.Properties.MouseWheelDelta / 20.0;
            int sign = Math.Sign(offset) * 20;
            for (int n = 0; n < Math.Abs(offset); n++)
            {
                await _webView.InvokeScriptAsync("verticalScrollBy", sign);
                await Task.Delay(10);
            }
        }


        private void OnFooterManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            _glass.Background = new SolidColorBrush(Colors.Transparent);
            _cancelManipulation = false;
        }

        private async void OnFooterManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            await _webView.InvokeScriptAsync("verticalScrollBy", -e.Delta.Translation.Y);
            if (_cancelManipulation)
            {
                e.Complete();
            }
        }


        private void OnGlassManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            _glass.Background = null;
            _cancelManipulation = true;
        }
    }
}
