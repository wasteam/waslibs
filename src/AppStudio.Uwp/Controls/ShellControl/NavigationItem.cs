using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public class NavigationItem : DependencyObject
    {
        public static readonly NavigationItem Separator = new NavigationItem { IsSeparator = true };

        public NavigationItem()
        {
            this.IsSeparator = false;
        }
        public NavigationItem(string caption)
        {
            this.Caption = caption;
        }
        public NavigationItem(Symbol symbol, string caption)
        {
            this.Icon = new SymbolIcon(symbol);
            this.Caption = caption;
        }
        public NavigationItem(string glyph, string caption)
        {
            this.Icon = CreateIcon(glyph);
            this.Caption = caption;
        }
        public NavigationItem(Uri uriSource, string caption)
        {
            this.Icon = new BitmapIcon { UriSource = uriSource, Width = 20, Height = 20 };
            this.Caption = caption;
        }

        public NavigationItem(string caption, Control control) : this(caption)
        {
            this.Control = control;
        }
        public NavigationItem(string caption, Action<NavigationItem> onClick) : this(caption)
        {
            this.OnClick = onClick;
        }

        public NavigationItem(string glyph, string caption, Control control) : this(glyph, caption)
        {
            this.Control = control;
        }
        public NavigationItem(string glyph, string caption, Action<NavigationItem> onClick, Brush background = null) : this(glyph, caption)
        {
            this.OnClick = onClick;
            if (background != null)
            {
                this.Background = background;
            }
        }
        public NavigationItem(string glyph, string caption, IEnumerable<NavigationItem> subItems, Brush background = null) : this(glyph, caption)
        {
            this.SubItems = subItems;
            if (background != null)
            {
                this.Background = background;
            }
        }

        public NavigationItem(Symbol symbol, string caption, Control control) : this(symbol, caption)
        {
            this.Control = control;
        }
        public NavigationItem(Symbol symbol, string caption, Action<NavigationItem> onClick = null, Brush background = null) : this(symbol, caption)
        {
            this.OnClick = onClick;
            if (background != null)
            {
                this.Background = background;
            }
        }
        public NavigationItem(Symbol symbol, string caption, IEnumerable<NavigationItem> subItems, Brush background = null) : this(symbol, caption)
        {
            this.SubItems = subItems;
            if (background != null)
            {
                this.Background = background;
            }
        }

        public NavigationItem(Uri uriSource, string caption, Control control) : this(uriSource, caption)
        {
            this.Control = control;
        }
        public NavigationItem(Uri uriSource, string caption, Action<NavigationItem> onClick = null, Brush background = null) : this(uriSource, caption)
        {
            this.OnClick = onClick;
            if (background != null)
            {
                this.Background = background;
            }
        }
        public NavigationItem(Uri uriSource, string caption, IEnumerable<NavigationItem> subItems, Brush background = null) : this(uriSource, caption)
        {
            this.SubItems = subItems;
            if (background != null)
            {
                this.Background = background;
            }
        }

        public bool IsSeparator { get; set; }

        #region Icon
        public IconElement Icon
        {
            get { return (IconElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(IconElement), typeof(NavigationItem), new PropertyMetadata(null));
        #endregion

        #region Caption
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(NavigationItem), new PropertyMetadata(null));
        #endregion

        #region Background
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(NavigationItem), new PropertyMetadata(null));
        #endregion

        #region Control
        public Control Control
        {
            get { return (Control)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register("Control", typeof(Control), typeof(NavigationItem), new PropertyMetadata(null));
        #endregion

        #region SubItems
        public IEnumerable<NavigationItem> SubItems
        {
            get { return (IEnumerable<NavigationItem>)GetValue(SubItemsProperty); }
            set { SetValue(SubItemsProperty, value); }
        }

        public static readonly DependencyProperty SubItemsProperty = DependencyProperty.Register("SubItems", typeof(IEnumerable<NavigationItem>), typeof(NavigationItem), new PropertyMetadata(null));
        #endregion

        #region ClearSelection
        public bool ClearSelection
        {
            get { return (bool)GetValue(ClearSelectionProperty); }
            set { SetValue(ClearSelectionProperty, value); }
        }

        public static readonly DependencyProperty ClearSelectionProperty = DependencyProperty.Register("ClearSelection", typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));
        #endregion

        #region OnClick
        public Action<NavigationItem> OnClick
        {
            get { return (Action<NavigationItem>)GetValue(OnClickProperty); }
            set { SetValue(OnClickProperty, value); }
        }

        public static readonly DependencyProperty OnClickProperty = DependencyProperty.Register("OnClick", typeof(Action<NavigationItem>), typeof(NavigationItem), new PropertyMetadata(null));
        #endregion

        private static FontIcon CreateIcon(string glyph)
        {
            string[] parts = glyph.Split(':');
            if (parts.Length == 1)
            {
                return new FontIcon { Glyph = System.Net.WebUtility.HtmlDecode(glyph) };
            }
            return new FontIcon
            {
                Glyph = System.Net.WebUtility.HtmlDecode(parts[1]),
                FontFamily = new FontFamily(parts[0])
            };
        }
    }
}
