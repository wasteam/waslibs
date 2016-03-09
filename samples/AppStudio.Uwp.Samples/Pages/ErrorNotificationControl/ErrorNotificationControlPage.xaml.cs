using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Misc", Name = "ErrorNotificationControl")]
    public sealed partial class ErrorNotificationControlPage : SamplePage
    {
        public ErrorNotificationControlPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "Error Notification Control"; }
        }

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(ErrorNotificationControlPage), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ErrorNotificationControlPage), new PropertyMetadata(null));
        #endregion

        #region ErrorText
        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public static readonly DependencyProperty ErrorTextProperty = DependencyProperty.Register("ErrorText", typeof(string), typeof(ErrorNotificationControlPage), new PropertyMetadata("Ups! Something went wrong. Try again later."));
        #endregion 

        #region ErrorVisibility
        public Visibility ErrorVisibility
        {
            get { return (Visibility)GetValue(ErrorVisibilityProperty); }
            set { SetValue(ErrorVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ErrorVisibilityProperty = DependencyProperty.Register("ErrorVisibility", typeof(Visibility), typeof(ErrorNotificationControlPage), new PropertyMetadata(Visibility.Collapsed));
        #endregion

        #region ForceCrash
        public bool ForceCrash
        {
            get { return (bool)GetValue(ForceCrashProperty); }
            set { SetValue(ForceCrashProperty, value); }
        }

        public static readonly DependencyProperty ForceCrashProperty = DependencyProperty.Register("ForceCrash", typeof(bool), typeof(ErrorNotificationControlPage), new PropertyMetadata(true));
        #endregion        

        #region Commands
        public ICommand RefreshData
        {
            get { return new RelayCommand(() => LoadData()); }
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            this.ItemTemplate = Resources["DeviceItemTemplate"] as DataTemplate;
            LoadData();
            base.OnNavigatedTo(e);
        }

        private void LoadData()
        {
            try
            {
                ErrorVisibility = Visibility.Collapsed;
                this.Items = new ObservableCollection<object>(new DevicesDataSource().GetItems());
                if (ForceCrash)
                {
                    throw new Exception("Simulate exception");
                }
            }
            catch (System.Exception)
            {
                ErrorVisibility = Visibility.Visible;
            }
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new ErrorNotificationControlSettings() { DataContext = this });
        }
    }
}
