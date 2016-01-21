using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AppStudio.Uwp.Controls
{
    public sealed class PivoramaItem : ContentControl
    {
        public event RoutedEventHandler HeaderClick;
        public event RoutedEventHandler TabClick;

        private ContentPresenter _content = null;
        private ContentControl _header = null;
        private Panel _tabs = null;

        private Storyboard _storyboard = null;

        public PivoramaItem()
        {
            this.DefaultStyleKey = typeof(PivoramaItem);
        }

        internal double X { get; set; }

        #region Index
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        private static void IndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PivoramaItem;
            control.SetItems(control.Items);
        }

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(PivoramaItem), new PropertyMetadata(null, IndexChanged));
        #endregion

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(PivoramaItem), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(PivoramaItem), new PropertyMetadata(null));
        #endregion

        #region HeaderOpacity
        public double HeaderOpacity
        {
            get { return (double)GetValue(HeaderOpacityProperty); }
            set { SetValue(HeaderOpacityProperty, value); }
        }

        public static readonly DependencyProperty HeaderOpacityProperty = DependencyProperty.Register("HeaderOpacity", typeof(double), typeof(PivoramaItem), new PropertyMetadata(0.0));
        #endregion

        #region Items
        public IEnumerable<object> Items
        {
            get { return (IEnumerable<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PivoramaItem;
            control.SetItems(e.NewValue as IEnumerable<object>);
        }

        private void SetItems(IEnumerable<object> items)
        {
            if (_tabs != null && items != null)
            {
                _tabs.Children.Clear();
                var arrItems = items.ToArray();
                for (int n = 0; n < arrItems.Length; n++)
                {
                    var control = new ContentControl { Content = arrItems[(this.Index + n) % arrItems.Length], ContentTemplate = TabTemplate };
                    control.Tapped += OnTabTapped;
                    _tabs.Children.Add(control);
                }
            }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<object>), typeof(PivoramaItem), new PropertyMetadata(null, ItemsChanged));
        #endregion

        #region TabTemplate
        public DataTemplate TabTemplate
        {
            get { return (DataTemplate)GetValue(TabTemplateProperty); }
            set { SetValue(TabTemplateProperty, value); }
        }

        public static readonly DependencyProperty TabTemplateProperty = DependencyProperty.Register("TabTemplate", typeof(DataTemplate), typeof(PivoramaItem), new PropertyMetadata(null));
        #endregion

        #region TabsVisibility
        public Visibility TabsVisibility
        {
            get { return (Visibility)GetValue(TabsVisibilityProperty); }
            set { SetValue(TabsVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TabsVisibilityProperty = DependencyProperty.Register("TabsVisibility", typeof(Visibility), typeof(PivoramaItem), new PropertyMetadata(Visibility.Collapsed));
        #endregion

        protected override void OnApplyTemplate()
        {
            _content = base.GetTemplateChild("content") as ContentPresenter;
            _header = base.GetTemplateChild("header") as ContentControl;
            _tabs = base.GetTemplateChild("tabs") as Panel;

            _header.Tapped += OnHeaderTapped;

            this.SetItems(this.Items);

            base.OnApplyTemplate();
        }

        internal void MoveX(double x, double duration = 0)
        {
            if (_storyboard != null)
            {
                _storyboard.Pause();
                _storyboard = null;
            }
            if (duration > 0)
            {
                _storyboard = this.AnimateX(x, duration);
            }
            else
            {
                this.TranslateX(x);
            }
            this.X = x;
        }

        private void OnHeaderTapped(object sender, TappedRoutedEventArgs e)
        {
            if (HeaderClick != null)
            {
                HeaderClick(this, new RoutedEventArgs());
            }
        }

        private void OnTabTapped(object sender, TappedRoutedEventArgs e)
        {
            if (TabClick != null)
            {
                TabClick(sender, new RoutedEventArgs());
            }
        }
    }
}
