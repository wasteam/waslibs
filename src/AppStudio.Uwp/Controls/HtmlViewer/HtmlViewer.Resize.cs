using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private double _docX;
        private double _docY;
        private double _docWidth;
        private double _docHeight;

        double _currentHeaderHeight = 0;
        double _currentFooterHeight = 0;

        private async Task OnControlResize()
        {
            if (this.ActualWidth > 0)
            {
                _container.Width = this.ActualWidth - MARGIN_RIGHT;
                _header.Width = Math.Max(0, this.ActualWidth - (ASideLeftWidth + ASideRightWidth));
                _footer.Width = Math.Max(0, this.ActualWidth - (ASideLeftWidth + ASideRightWidth));

                await SetHtmlDocumentMargin();

                System.Diagnostics.Debug.WriteLine("OnControlResize");
            }
        }

        private async Task OnDocumentResize(double docX, double docY, double docWidth, double docHeight)
        {
            _footer.TranslateY(HeaderHeight + docHeight);
            await SetHtmlDocumentMargin();
            System.Diagnostics.Debug.WriteLine("OnDocumentResize", _container.Height);
        }

        private void OnDocumentScroll(double y)
        {
            _container.TranslateY(_docY - _currentHeaderHeight);
        }

        private async Task SetHtmlDocumentMargin()
        {
            if (_isHtmlLoaded)
            {
                //await Task.Delay(100);

                //var rect = await _webView.InvokeScriptAsync("getHtmlDocumentRect");
                //string[] parts = rect.Substring(1).Split('|');
                //_docX = parts[0].AsDouble();
                //_docY = parts[1].AsDouble();
                //_docWidth = parts[2].AsDouble();
                //_docHeight = parts[3].AsDouble();

                _header.TranslateX(ASideLeftWidth);
                _footer.TranslateX(ASideLeftWidth);

                double containerHeight = Math.Max(HeaderHeight + _docHeight + FooterHeight, Math.Max(ASideLeftHeight, ASideRightHeight));
                double footerHeight = Math.Max(FooterHeight, containerHeight - (HeaderHeight + _docHeight));
                string margin = $"{HeaderHeight}px {ASideRightWidth + 24}px {footerHeight}px {ASideLeftWidth + 24}px";
                await _webView.InvokeScriptAsync("setHtmlDocumentMargin", margin);

                _currentHeaderHeight = HeaderHeight;
                _currentFooterHeight = FooterHeight;

                System.Diagnostics.Debug.WriteLine("SetMargin " + margin);
            }
        }
    }
}
