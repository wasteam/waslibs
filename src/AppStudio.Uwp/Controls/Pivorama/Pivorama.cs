using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public partial class Pivorama : Control
    {
        private Panel _headerContainer = null;
        private PivoramaPanel _header = null;

        private Panel _tabsContainer = null;
        private PivoramaTabs _tabs = null;

        private Panel _panelContainer = null;
        private PivoramaPanel _panel = null;

        private RectangleGeometry _clip = null;

        private bool _isInitialized = false;

        public Pivorama()
        {
            this.DefaultStyleKey = typeof(Pivorama);
        }

        protected override void OnApplyTemplate()
        {
            _headerContainer = base.GetTemplateChild("headerContainer") as Panel;
            _header = base.GetTemplateChild("header") as PivoramaPanel;
            _header.SelectionChanged += OnSelectionChanged;

            _tabsContainer = base.GetTemplateChild("tabsContainer") as Panel;
            _tabs = base.GetTemplateChild("tabs") as PivoramaTabs;
            _tabs.SelectionChanged += OnSelectionChanged;

            _panelContainer = base.GetTemplateChild("rowPanelContainer") as Panel;
            _panel = base.GetTemplateChild("rowPanel") as PivoramaPanel;

            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            _headerContainer.ManipulationDelta += OnManipulationDelta;
            _headerContainer.ManipulationCompleted += OnManipulationCompleted;
            _headerContainer.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;

            _tabsContainer.ManipulationDelta += OnManipulationDelta;
            _tabsContainer.ManipulationCompleted += OnManipulationCompleted;
            _tabsContainer.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;

            _panelContainer.ManipulationDelta += OnManipulationDelta;
            _panelContainer.ManipulationCompleted += OnManipulationCompleted;
            _panelContainer.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;

            _isInitialized = true;

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Index != _panel.GetIndexOf(e.AddedItems[0]))
            {
                this.Index = _panel.GetIndexOf(e.AddedItems[0]) - 1;
                this.AnimateNext(100);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshLayout();
            _clip.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
        }

        private void RefreshLayout()
        {
            if (this.FitToScreen)
            {
                this.ItemWidthEx = this.ActualWidth;
                _headerContainer.Visibility = Visibility.Collapsed;
                _tabsContainer.Visibility = Visibility.Visible;
            }
            else
            {
                if (this.ItemWidthEx * 2 < this.ActualWidth)
                {
                    _headerContainer.Visibility = Visibility.Visible;
                    _tabsContainer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _headerContainer.Visibility = Visibility.Collapsed;
                    _tabsContainer.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
