using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    public class AccordionItem : ContentControl
    {
        private Control _header = null;

        public AccordionItem()
        {
            this.DefaultStyleKey = typeof(AccordionItem);
        }

        public int Index { get; set; }

        public bool IsUp { get; set; }

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(AccordionItem), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(AccordionItem), new PropertyMetadata(null));
        #endregion

        public double HeaderHeight
        {
            get
            {
                if (_header != null)
                {
                    return _header.DesiredSize.Height;
                }
                return 0.0;
            }
        }

        protected override void OnApplyTemplate()
        {
            _header = base.GetTemplateChild("header") as Control;

            base.OnApplyTemplate();
        }
    }
}
