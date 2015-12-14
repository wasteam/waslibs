using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.ApplicationModel;

namespace AppStudio.Uwp.Controls
{
    partial class HtmlViewer
    {
        #region ContentAlignment
        public HorizontalAlignment ContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(ContentAlignmentProperty); }
            set { SetValue(ContentAlignmentProperty, value); }
        }

        private static async void ContentAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            await control.SetContentAlignment((HorizontalAlignment)e.NewValue);
        }

        public static readonly DependencyProperty ContentAlignmentProperty = DependencyProperty.Register("ContentAlignment", typeof(HorizontalAlignment), typeof(HtmlViewer), new PropertyMetadata(HorizontalAlignment.Left, ContentAlignmentChanged));
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

        public static readonly DependencyProperty IsHeaderVisibleProperty = DependencyProperty.Register("IsHeaderVisible", typeof(bool), typeof(HtmlViewer), new PropertyMetadata(true, ComplementVisibilityChanged));
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

        public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register("IsFooterVisible", typeof(bool), typeof(HtmlViewer), new PropertyMetadata(true, ComplementVisibilityChanged));
        #endregion


        #region ASideLeft
        public object ASideLeft
        {
            get { return (object)GetValue(ASideLeftProperty); }
            set { SetValue(ASideLeftProperty, value); }
        }

        public static readonly DependencyProperty ASideLeftProperty = DependencyProperty.Register("ASideLeft", typeof(object), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region ASideLeftTemplate
        public DataTemplate ASideLeftTemplate
        {
            get { return (DataTemplate)GetValue(ASideLeftTemplateProperty); }
            set { SetValue(ASideLeftTemplateProperty, value); }
        }

        public static readonly DependencyProperty ASideLeftTemplateProperty = DependencyProperty.Register("ASideLeftTemplate", typeof(DataTemplate), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region IsASideLeftVisible
        public bool IsASideLeftVisible
        {
            get { return (bool)GetValue(IsASideLeftVisibleProperty); }
            set { SetValue(IsASideLeftVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsASideLeftVisibleProperty = DependencyProperty.Register("IsASideLeftVisible", typeof(bool), typeof(HtmlViewer), new PropertyMetadata(true, ComplementVisibilityChanged));
        #endregion

        #region ASideLeftMaxWidth
        public double ASideLeftMaxWidth
        {
            get { return (double)GetValue(ASideLeftMaxWidthProperty); }
            set { SetValue(ASideLeftMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty ASideLeftMaxWidthProperty = DependencyProperty.Register("ASideLeftMaxWidth", typeof(double), typeof(HtmlViewer), new PropertyMetadata(Double.MaxValue, MeasureLayout));
        #endregion


        #region ASideRight
        public object ASideRight
        {
            get { return (object)GetValue(ASideRightProperty); }
            set { SetValue(ASideRightProperty, value); }
        }

        public static readonly DependencyProperty ASideRightProperty = DependencyProperty.Register("ASideRight", typeof(object), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region ASideRightTemplate
        public DataTemplate ASideRightTemplate
        {
            get { return (DataTemplate)GetValue(ASideRightTemplateProperty); }
            set { SetValue(ASideRightTemplateProperty, value); }
        }

        public static readonly DependencyProperty ASideRightTemplateProperty = DependencyProperty.Register("ASideRightTemplate", typeof(DataTemplate), typeof(HtmlViewer), new PropertyMetadata(null));
        #endregion

        #region IsASideRightVisible
        public bool IsASideRightVisible
        {
            get { return (bool)GetValue(IsASideRightVisibleProperty); }
            set { SetValue(IsASideRightVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsASideRightVisibleProperty = DependencyProperty.Register("IsASideRightVisible", typeof(bool), typeof(HtmlViewer), new PropertyMetadata(true, ComplementVisibilityChanged));
        #endregion

        #region ASideRightMaxWidth
        public double ASideRightMaxWidth
        {
            get { return (double)GetValue(ASideRightMaxWidthProperty); }
            set { SetValue(ASideRightMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty ASideRightMaxWidthProperty = DependencyProperty.Register("ASideRightMaxWidth", typeof(double), typeof(HtmlViewer), new PropertyMetadata(Double.MaxValue, MeasureLayout));
        #endregion


        #region ContentMinWidth
        public double ContentMinWidth
        {
            get { return (double)GetValue(ContentMinWidthProperty); }
            set { SetValue(ContentMinWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentMinWidthProperty = DependencyProperty.Register("ContentMinWidth", typeof(double), typeof(HtmlViewer), new PropertyMetadata(400.0, MeasureLayout));
        #endregion

        private static async void MeasureLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            if (control._webView != null)
            {
                control.MeasureLayout();
                await Task.Delay(50);
                await control.SetHtmlDocumentMargin();
            }
        }

        private async Task SetContentAlignment(HorizontalAlignment horizontalAlignment)
        {
            if (_webView != null && !DesignMode.DesignModeEnabled)
            {
                string maxWidth = "4096px";
                string marginLeft = "12px";
                string marginRight = "12px";

                switch (horizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        marginRight = "auto";
                        break;
                    case HorizontalAlignment.Center:
                        marginLeft = "auto";
                        marginRight = "auto";
                        break;
                    case HorizontalAlignment.Right:
                        marginLeft = "auto";
                        break;
                    case HorizontalAlignment.Stretch:
                        maxWidth = "unset";
                        marginLeft = "auto";
                        marginRight = "auto";
                        break;
                    default:
                        break;
                }
                await _webView.InvokeScriptAsync("setHtmlStyle", new string[] { maxWidth, marginLeft, marginRight });
            }
        }

        private static async void ComplementVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            if (control._webView != null)
            {
                control._header.SetVisibility(control.IsHeaderVisible);
                control._footer.SetVisibility(control.IsFooterVisible);
                control._asideLeft.SetVisibility(control.IsASideLeftVisible);
                control._asideRight.SetVisibility(control.IsASideRightVisible);
            }
            await control.SetHtmlDocumentMargin();
        }
    }
}
