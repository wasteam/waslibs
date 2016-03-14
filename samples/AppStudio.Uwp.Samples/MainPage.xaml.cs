using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.TopContentTemplate = this.Resources["WideTopTemplate"] as DataTemplate;
            this.DataContext = this;
            this.SizeChanged += OnSizeChanged;
        }

        public string Caption
        {
            get { return "WasLibs Samples"; }
        }

        public bool HideCommandBar
        {
            get { return true; }
        }

        public DataTemplate HeaderTemplate
        {
            get { return App.Current.Resources["HomeHeaderTemplate"] as DataTemplate; }
        }

        #region PrimaryCommands, SecondaryCommands, CommandBarBackground
        public IEnumerable<ICommandBarElement> PrimaryCommands
        {
            get { return null; }
        }

        public IEnumerable<ICommandBarElement> SecondaryCommands
        {
            get { return null; }
        }

        public Brush CommandBarBackground
        {
            get { return null; }
        }
        #endregion

        #region TopContentTemplate
        public DataTemplate TopContentTemplate
        {
            get { return (DataTemplate)GetValue(TopContentTemplateProperty); }
            set { SetValue(TopContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty TopContentTemplateProperty = DependencyProperty.Register("TopContentTemplate", typeof(DataTemplate), typeof(MainPage), new PropertyMetadata(null));
        #endregion

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(MainPage), new PropertyMetadata(null));
        #endregion
        
        public ICommand ItemClickCommand
        {
            get
            {
                return new RelayCommand<ControlDataItem>((control) =>
                {
                    NavigationService.NavigateToPage(control.DetailPageName);
                });
            }
        }        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>(new FeaturedControlsDataSource().GetItems());
            base.OnNavigatedTo(e);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (AppShell.Current.Shell.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                this.TopContentTemplate = this.Resources["WideTopTemplate"] as DataTemplate;
            }
            else
            {
                this.TopContentTemplate = this.Resources["TinyTopTemplate"] as DataTemplate;
            }
        }
    }
}
