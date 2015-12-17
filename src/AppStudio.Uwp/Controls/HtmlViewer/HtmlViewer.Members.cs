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

        #region HeaderVisibility
        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(HtmlViewer), new PropertyMetadata(Visibility.Visible, AdornVisibilityChanged));
        #endregion

        #region HeaderWidth/HeaderHeight
        public double HeaderWidth
        {
            get { return HeaderVisibility == Visibility ? _header.ActualWidth : 0.0; }
        }
        public double HeaderHeight
        {
            get { return HeaderVisibility == Visibility ? _header.ActualHeight : 0.0; }
        }
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

        #region FooterVisibility
        public Visibility FooterVisibility
        {
            get { return (Visibility)GetValue(FooterVisibilityProperty); }
            set { SetValue(FooterVisibilityProperty, value); }
        }

        public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register("FooterVisibility", typeof(Visibility), typeof(HtmlViewer), new PropertyMetadata(Visibility.Visible, AdornVisibilityChanged));
        #endregion

        #region FooterWidth/FooterHeight
        public double FooterWidth
        {
            get { return FooterVisibility == Visibility ? _footer.ActualWidth : 0.0; }
        }
        public double FooterHeight
        {
            get { return FooterVisibility == Visibility ? _footer.ActualHeight : 0.0; }
        }
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

        #region ASideLeftVisibility
        public Visibility ASideLeftVisibility
        {
            get { return (Visibility)GetValue(ASideLeftVisibilityProperty); }
            set { SetValue(ASideLeftVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ASideLeftVisibilityProperty = DependencyProperty.Register("ASideLeftVisibility", typeof(Visibility), typeof(HtmlViewer), new PropertyMetadata(Visibility.Visible, AdornVisibilityChanged));
        #endregion

        #region ASideLeftMaxWidth
        public double ASideLeftMaxWidth
        {
            get { return (double)GetValue(ASideLeftMaxWidthProperty); }
            set { SetValue(ASideLeftMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty ASideLeftMaxWidthProperty = DependencyProperty.Register("ASideLeftMaxWidth", typeof(double), typeof(HtmlViewer), new PropertyMetadata(Double.MaxValue));
        #endregion

        #region ASideLeftWidth/ASideLeftHeight
        public double ASideLeftWidth
        {
            get { return ASideLeftVisibility == Visibility ? _asideLeft.ActualWidth : 0.0; }
        }
        public double ASideLeftHeight
        {
            get { return ASideLeftVisibility == Visibility ? _asideLeft.ActualHeight : 0.0; }
        }
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

        #region ASideRightVisibility
        public Visibility ASideRightVisibility
        {
            get { return (Visibility)GetValue(ASideRightVisibilityProperty); }
            set { SetValue(ASideRightVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ASideRightVisibilityProperty = DependencyProperty.Register("ASideRightVisibility", typeof(Visibility), typeof(HtmlViewer), new PropertyMetadata(Visibility.Visible, AdornVisibilityChanged));
        #endregion

        #region ASideRightMaxWidth
        public double ASideRightMaxWidth
        {
            get { return (double)GetValue(ASideRightMaxWidthProperty); }
            set { SetValue(ASideRightMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty ASideRightMaxWidthProperty = DependencyProperty.Register("ASideRightMaxWidth", typeof(double), typeof(HtmlViewer), new PropertyMetadata(Double.MaxValue));
        #endregion

        #region ASideRightWidth/ASideRightHeight
        public double ASideRightWidth
        {
            get { return ASideRightVisibility == Visibility ? _asideRight.ActualWidth : 0.0; }
        }
        public double ASideRightHeight
        {
            get { return ASideRightVisibility == Visibility ? _asideRight.ActualHeight : 0.0; }
        }
        #endregion


        #region ContentMinWidth
        public double ContentMinWidth
        {
            get { return (double)GetValue(ContentMinWidthProperty); }
            set { SetValue(ContentMinWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentMinWidthProperty = DependencyProperty.Register("ContentMinWidth", typeof(double), typeof(HtmlViewer), new PropertyMetadata(400.0));
        #endregion

        private async Task SetContentAlignment(HorizontalAlignment horizontalAlignment)
        {
            if (_webView != null && !DesignMode.DesignModeEnabled)
            {
                string maxWidth = "4096px";
                string marginLeft = "0px";
                string marginRight = "0px";

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

        private static async void AdornVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HtmlViewer;
            if (control._webView != null)
            {
                await control.OnControlResize();
            }
        }
    }
}
