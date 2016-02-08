using System;
using System.Linq;
using System.Collections;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class Pivorama : Control
    {
        private Panel _header = null;
        private Panel _headerItems = null;
        private Panel _tabItems = null;
        private Panel _container = null;

        private bool _isInitialized = false;

        public Pivorama()
        {
            this.DefaultStyleKey = typeof(Pivorama);
        }

        protected override void OnApplyTemplate()
        {
            _header = base.GetTemplateChild("header") as Panel;
            _headerItems = base.GetTemplateChild("headerItems") as Panel;
            _headerItems.SizeChanged += OnHeaderItemsSizeChanged;

            _tabItems = base.GetTemplateChild("tabItems") as Panel;

            _container = base.GetTemplateChild("container") as Panel;
            _container.TranslateX(this.Position);

            _container.ManipulationDelta += OnManipulationDelta;
            _container.ManipulationCompleted += OnManipulationCompleted;
            _container.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;

            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _isInitialized = true;
                this.ItemsSourceChanged(this.ItemsSource as IEnumerable);
            }

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private void OnHeaderItemsSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _header.Height = _headerItems.ActualHeight;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.FitToScreen)
            {
                this.ItemWidth = e.NewSize.Width;
                this.SetItemWidth(e.NewSize.Width, Math.Max(1, e.PreviousSize.Width));
            }
            RefreshLayout();
        }

        private void RefreshLayout()
        {
            if (_isInitialized)
            {
                double height = this.ActualHeight;
                if (!double.IsNaN(_header.Height))
                {
                    height = this.ActualHeight - _header.Height;
                }

                if (_container.Children.Count > 0)
                {
                    height = Math.Max(height, _container.Children.Cast<FrameworkElement>().Max(r => r.DesiredSize.Height));
                }
                _container.Height = height;

                if (this.IsTabVisible)
                {
                    _header.Visibility = Visibility.Collapsed;
                    _tabItems.Visibility = Visibility.Visible;
                }
                else
                {
                    _header.Visibility = Visibility.Visible;
                    _tabItems.Visibility = Visibility.Collapsed;
                }
                this.ArrangeItems();
            }
        }
    }
}
