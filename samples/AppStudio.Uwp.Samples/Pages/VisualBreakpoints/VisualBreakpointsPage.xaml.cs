using System;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "VisualBreakpoints", Order = 05)]
    public sealed partial class VisualBreakpointsPage : SamplePage
    {
        public VisualBreakpointsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        public override string Caption
        {
            get { return "Visual Breakpoints"; }
        }

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(VisualBreakpointsPage), new PropertyMetadata(null));
        #endregion

        #region WindowWidth
        public double WindowWidth
        {
            get { return (double)GetValue(WindowWidthProperty); }
            set { SetValue(WindowWidthProperty, value); }
        }

        public static readonly DependencyProperty WindowWidthProperty = DependencyProperty.Register("WindowWidth", typeof(double), typeof(VisualBreakpointsPage), new PropertyMetadata(0));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>(new PhotosDataSource().GetItems());
            this.SizeChanged += SizeChangedEvent;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.SizeChanged -= SizeChangedEvent;
            base.OnNavigatedFrom(e);
        }

        private void SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            this.WindowWidth = Math.Truncate(e.NewSize.Width);
        }
    }
}
