using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Input;
using Windows.ApplicationModel;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private double _htmlBottom = 0;

        private async void OnPointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);

            double offset = -point.Properties.MouseWheelDelta / 20.0;
            double delta = -Math.Sign(offset) * 20;

            for (int n = 0; n < Math.Abs(offset); n++)
            {
                double hy = _header.GetTranslateY() + delta;
                if (hy > 0)
                {
                    delta = delta - hy;
                    await TranslateDelta(delta);
                    break;
                }
                else
                {
                    var maxY = this.ActualHeight - Math.Max(_htmlBottom, Math.Max(_asideLeft.ActualHeight, _asideRight.ActualHeight));
                    if (hy < maxY)
                    {
                        delta = delta + maxY - hy;
                        await TranslateDelta(delta);
                        break;
                    }
                }
                if (delta != 0.0)
                {
                    await TranslateDelta(delta);
                    await Task.Delay(10);
                }
            }
        }

        private async void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double delta = e.Delta.Translation.Y;
            double hy = _header.GetTranslateY() + delta;
            if (hy > 0)
            {
                delta = delta - hy;
                e.Complete();
            }
            else
            {
                var maxY = this.ActualHeight - Math.Max(_htmlBottom, Math.Max(_asideLeft.ActualHeight, _asideRight.ActualHeight));
                if (hy < maxY)
                {
                    delta = delta + maxY - hy;
                }
            }
            if (delta != 0.0)
            {
                await TranslateDelta(delta);
            }
        }

        private async Task TranslateDelta(double delta)
        {
            await _webView.InvokeScriptAsync("verticalScrollBy", -delta);
            _header.TranslateDeltaY(delta);
            _footer.TranslateDeltaY(delta);
            _asideLeft.TranslateDeltaY(delta);
            _asideRight.TranslateDeltaY(delta);
        }

        private async Task SetHtmlDocumentMargin()
        {
            if (_isHtmlLoaded & !DesignMode.DesignModeEnabled)
            {
                double height = (await _webView.InvokeScriptAsync("getContentHeight")).AsDouble();

                double headerHeight = IsHeaderVisible ? _header.ActualHeight : 0.0;
                double footerHeight = IsFooterVisible ? _footer.ActualHeight : 0.0;

                double leftWidth = IsASideLeftVisible ? _asideLeft.ActualWidth : 0.0;
                double rightWidth = IsASideRightVisible ? _asideRight.ActualWidth : 0.0;
                double leftHeight = IsASideLeftVisible ? _asideLeft.ActualHeight : 0.0;
                double rightHeight = IsASideRightVisible ? _asideRight.ActualHeight : 0.0;

                double marginBottom = Math.Max(footerHeight, Math.Max(leftHeight - (height + headerHeight), rightHeight - (height + headerHeight)));

                string margin = $"{headerHeight}px {rightWidth}px {marginBottom}px {leftWidth}px";
                await _webView.InvokeScriptAsync("setHtmlDocumentMargin", margin);

                ArrangeParts(await _webView.InvokeScriptAsync("getHtmlDocumentRect"));
            }
        }

        private void ArrangeParts(string value)
        {
            string[] parts = value.Split('|');
            double y = parts[1].AsDouble();
            double height = parts[3].AsDouble();

            double headerHeight = IsHeaderVisible ? _header.ActualHeight : 0.0;
            double footerHeight = IsFooterVisible ? _footer.ActualHeight : 0.0;

            double hy = y - headerHeight;
            double fy = y + height;

            double actualWidth = this.ActualWidth - MARGIN_RIGHT;

            double ml = IsASideLeftVisible ? _asideLeft.ActualWidth : 0.0;
            double mr = IsASideRightVisible ? _asideRight.ActualWidth : 0.0;
            double cw = Math.Max(1.0, actualWidth - (ml + mr));

            _header.Width = cw;
            _footer.Width = cw;

            _header.TranslateX(ml);
            _footer.TranslateX(ml);
            _asideLeft.TranslateX(0.0);
            _asideRight.TranslateX(ml + cw);

            _header.TranslateY(hy);
            _footer.TranslateY(fy);
            _asideLeft.TranslateY(hy);
            _asideRight.TranslateY(hy);

            _header.Opacity = headerHeight > 0 ? (headerHeight + hy) / headerHeight : 0.0;
            _asideLeft.Opacity = _asideLeft.ActualHeight > 0 ? (_asideLeft.ActualHeight + hy) / _asideLeft.ActualHeight : 0.0;
            _asideRight.Opacity = _asideRight.ActualHeight > 0 ? (_asideRight.ActualHeight + hy) / _asideRight.ActualHeight : 0.0;

            _htmlBottom = headerHeight + height + footerHeight;
        }
    }
}
