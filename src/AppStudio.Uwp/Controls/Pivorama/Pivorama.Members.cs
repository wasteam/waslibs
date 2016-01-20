using System;

using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private bool _disableSelectedIndexCallback = false;

        #region SelectedIndex
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set
            {
                // NOTE: Avoid external exceptions when this property is binded
                try
                {
                    SetValue(SelectedIndexProperty, value);
                }
                catch { }
            }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Pivorama), new PropertyMetadata(0, SelectedIndexChanged));
        #endregion

        #region ContentTemplate
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        #region TabTemplate
        public DataTemplate TabTemplate
        {
            get { return (DataTemplate)GetValue(TabTemplateProperty); }
            set { SetValue(TabTemplateProperty, value); }
        }

        public static readonly DependencyProperty TabTemplateProperty = DependencyProperty.Register("TabTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pivorama;

            if (control._disableSelectedIndexCallback)
            {
                return;
            }

            if (control._isInitialized)
            {
                control.AnimateToIndex((int)e.OldValue, (int)e.NewValue);
            }

            //control.ApplySelection();
        }

        private void AnimateToIndex(int oldInx, int newInx)
        {
            newInx = newInx % _items.Count;
            if (oldInx == newInx)
            {
                return;
            }

            oldInx = newInx.DecMod(_items.Count);
            ArrangeItems(oldInx);

            // Animate Next
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffsetInternal(-delta, oldInx, 75);
        }

        // TODO: 
        //private int CompareIndex(int inx1, int inx2)
        //{
        //    if (inx1 == 0 && inx2 == _items.Count - 1)
        //    {
        //        return inx2 - inx1;
        //    }

        //    if (inx1 == _items.Count - 1 && inx2 == 0)
        //    {
        //        return inx2 - inx1;
        //    }

        //    return inx1 - inx2;
        //}
    }
}
