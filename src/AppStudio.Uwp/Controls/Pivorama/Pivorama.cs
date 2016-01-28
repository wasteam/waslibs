using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class Pivorama : Control
    {
        private Panel _container = null;
        private List<object> _items = null;

        private bool _isInitialized = false;

        public Pivorama()
        {
            _items = new List<object>();
            this.DefaultStyleKey = typeof(Pivorama);
        }

        protected override void OnApplyTemplate()
        {
            _container = base.GetTemplateChild("container") as Panel;

            this.BuildPanes(this.ItemsSource as IEnumerable);
            this.ItemsSourceChanged(this.ItemsSource as IEnumerable);

            _container.ManipulationInertiaStarting += OnManipulationInertiaStarting;
            _container.ManipulationDelta += OnManipulationDelta;
            _container.ManipulationCompleted += OnManipulationCompleted;
            _container.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System | ManipulationModes.TranslateRailsX;

            _isInitialized = true;

            base.OnApplyTemplate();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            if (_container != null && _container.Children.Count > 0)
            {
                double actualWidth = size.Width - 10;
                var tabsVisibility = actualWidth > ItemWidth * 2 ? Visibility.Collapsed : Visibility.Visible;

                var positions = GetPositions(ItemWidth).ToArray();
                var controls = _container.Children.Cast<PivoramaItem>().OrderBy(r => r.X).ToArray();
                for (int n = 0; n < controls.Length; n++)
                {
                    var position = positions[n];
                    var control = controls[n];
                    control.MoveX(position.X + _offset);

                    ShowHeader(control, position.X < actualWidth);
                    if (n == 1 && actualWidth < ItemWidth)
                    {
                        control.TabsVisibility = Visibility.Visible;
                    }
                    else
                    {
                        control.TabsVisibility = Visibility.Collapsed;
                    }
                }
            }

            return size;
        }

        private void ShowHeader(PivoramaItem control, bool visible)
        {
            if (visible)
            {
                control.HeaderOpacity = 1.0;
                control.IsHitTestVisible = true;
            }
            else
            {
                control.HeaderOpacity = 0.0;
                control.IsHitTestVisible = true;
            }
        }
    }
}
