using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
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

        private async void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double delta = e.Delta.Translation.Y;
            await TranslateDelta(delta);
        }

        private async Task TranslateDelta(double delta)
        {
            if (_webView != null && _isHtmlLoaded)
            {
                await _webView.InvokeScriptAsync("verticalScrollBy", -delta);
            }
        }
    }
}
