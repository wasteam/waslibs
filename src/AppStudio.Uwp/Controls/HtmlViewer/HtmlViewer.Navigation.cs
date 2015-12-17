using System;
using System.Text.RegularExpressions;

using Windows.UI.Xaml;
using Windows.ApplicationModel;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        private bool _isHtmlLoaded = false;

        #region Html
        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            control.NavigateToString(e.NewValue as string);
        }

        public static readonly DependencyProperty HtmlProperty = DependencyProperty.Register("Html", typeof(string), typeof(HtmlViewer), new PropertyMetadata(null, HtmlChanged));
        #endregion

        #region Source
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            control.NavigateToSource(e.NewValue as Uri);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(HtmlViewer), new PropertyMetadata(null, SourceChanged));
        #endregion

        public void NavigateToString(string text)
        {
            if (_webView != null && !DesignMode.DesignModeEnabled)
            {
                if (!String.IsNullOrEmpty(text))
                {
                    this.Source = null;
                    _progress.Visibility = Visibility.Visible;
                    _frame.Opacity = 0.0;
                    _isHtmlLoaded = false;
                    if (!ContainsHTML(text))
                    {
                        text = CovertToHtml(text);
                    }
                    _webView.NavigateToString(text);
                }
            }
        }

        public void NavigateToSource(Uri source)
        {
            if (_webView != null && !DesignMode.DesignModeEnabled)
            {
                if (source != null)
                {
                    this.Html = null;
                    if (source.Scheme.Equals("ms-appx", StringComparison.OrdinalIgnoreCase))
                    {
                        NavigateToContent(source.LocalPath.TrimStart('/'));
                    }
                    else
                    {
                        _progress.Visibility = Visibility.Visible;
                        _frame.Opacity = 0.0;
                        _isHtmlLoaded = false;
                        _webView.Navigate(source);
                    }
                }
            }
        }

        public async void NavigateToContent(string path)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                _progress.Visibility = Visibility.Visible;
                _frame.Opacity = 0.0;
                _isHtmlLoaded = false;
                await _webView.LoadAsync(path);
            }
        }

        private static bool ContainsHTML(string str)
        {
            return !Regex.IsMatch(str, @"^(?!.*<[^>]+>).*");
        }

        private static string CovertToHtml(string plainText)
        {
            return plainText.Replace("\r\n", "\r").Replace('\n', '\r').Replace("\r", "<br />");
        }
    }
}
