using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    partial class AccordionPanel
    {
        #region SelectedIndex
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccordionPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(AccordionPanel), new PropertyMetadata(0, SelectedIndexChanged));
        #endregion

        #region ItemsSource
        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccordionPanel;
            control.SetItemsSource(e.NewValue as IEnumerable<object>);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(AccordionPanel), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccordionPanel;
            control.SetItemTemplate(e.NewValue as DataTemplate);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AccordionPanel), new PropertyMetadata(null, ItemTemplateChanged));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        private static void HeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccordionPanel;
            control.SetHeaderTemplate(e.NewValue as DataTemplate);
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(AccordionPanel), new PropertyMetadata(null, HeaderTemplateChanged));
        #endregion

        static private int MaxTabs
        {
            get { return 4; }
        }

        private void SetItemsSource(IEnumerable<object> items)
        {
            base.Children.Clear();
            if (items != null)
            {
                int n = items.Count() - 1;
                foreach (var item in items.Reverse())
                {
                    var control = new AccordionItem
                    {
                        Index = n--,
                        Content = item,
                        ContentTemplate = this.ItemTemplate,
                        Header = item,
                        HeaderTemplate = this.HeaderTemplate
                    };
                    base.Children.Add(control);
                }
            }
        }

        private void SetItemTemplate(DataTemplate itemTemplate)
        {
            foreach (ContentControl item in base.Children)
            {
                item.ContentTemplate = itemTemplate;
            }
        }

        private void SetHeaderTemplate(DataTemplate headerTemplate)
        {
            foreach (AccordionItem item in base.Children)
            {
                item.HeaderTemplate = headerTemplate;
            }
        }
    }
}
