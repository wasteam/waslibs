using System;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.ApplicationModel;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
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
            control.Navigate(e.NewValue as Uri);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(HtmlViewer), new PropertyMetadata(null, SourceChanged));
        #endregion


        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region IsHeaderVisible
        public bool IsHeaderVisible
        {
            get { return (bool)GetValue(IsHeaderVisibleProperty); }
            set { SetValue(IsHeaderVisibleProperty, value); }
        }

        private static async void IsHeaderVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            control._header.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            await control.SetHtmlDocumentMargin();
        }

        public static readonly DependencyProperty IsHeaderVisibleProperty = DependencyProperty.Register("IsHeaderVisible", typeof(bool), typeof(HtmlViewer), new PropertyMetadata(true, IsHeaderVisibleChanged));
        #endregion


        #region Footer
        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region FooterTemplate
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region IsFooterVisible
        public bool IsFooterVisible
        {
            get { return (bool)GetValue(IsFooterVisibleProperty); }
            set { SetValue(IsFooterVisibleProperty, value); }
        }

        private static async void IsFooterVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            if (control._footer != null)
            {
                control._footer.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }            
            await control.SetHtmlDocumentMargin();
        }

        public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register("IsFooterVisible", typeof(bool), typeof(HtmlViewer), new PropertyMetadata(true, IsFooterVisibleChanged));
        #endregion



        public async void NavigateToContent(string path)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                _isHtmlLoaded = false;
                await _webView.LoadAsync(path);
            }
        }

        public void NavigateToString(string text)
        {
            if (_webView != null && !DesignMode.DesignModeEnabled)
            {
                if (!String.IsNullOrEmpty(text))
                {
                    this.Source = null;
                    _isHtmlLoaded = false;
                    _webView.NavigateToString(text);
                }
            }
        }

        public void Navigate(Uri source)
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
                        _isHtmlLoaded = false;
                        _webView.Navigate(source);
                    }
                }
            }
        }
    }
}
