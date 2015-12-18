using System;
using System.Threading.Tasks;

using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private Rect _documentSize;

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
            }
        }

        private async Task OnAdornResize()
        {
            await SetHtmlDocumentMargin();
        }

        private async Task OnDocumentResize(Rect rect)
        {
            _footer.TranslateY(_currentHeaderHeight + rect.Height);
            await SetHtmlDocumentMargin();
        }

        private void OnDocumentScroll(Rect rect)
        {
            _container.TranslateY(rect.Y - HeaderHeight);
        }

        private async Task SetHtmlDocumentMargin()
        {
            if (_isHtmlLoaded)
            {
                _header.TranslateX(ASideLeftWidth);
                _footer.TranslateX(ASideLeftWidth);

                double containerHeight = Math.Max(HeaderHeight + _documentSize.Height + FooterHeight, Math.Max(ASideLeftHeight, ASideRightHeight));
                double footerHeight = Math.Max(FooterHeight, containerHeight - (HeaderHeight + _documentSize.Height));

                string margin = $"{HeaderHeight}px {ASideRightWidth + 16}px {footerHeight}px {ASideLeftWidth + 16}px";
                await _webView.InvokeScriptAsync("setHtmlDocumentMargin", margin);

                _currentHeaderHeight = HeaderHeight;
                _currentFooterHeight = FooterHeight;
            }
        }
    }
}
