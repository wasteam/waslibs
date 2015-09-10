using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class ErrorNotificationControl : BaseControlPage
    {
        private Visibility _errorVisibility;
        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set {  SetProperty(ref _errorVisibility, value); }
        }
        private bool _forceCrash;
        public bool ForceCrash
        {
            get { return _forceCrash; }
            set { SetProperty(ref _forceCrash, value); }
        }
        private Visibility _isBusy;
        public Visibility IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public ErrorNotificationControl()
        {
            this.InitializeComponent();
            this.Items = new ObservableCollection<string>();
            this.IsBusy = Visibility.Collapsed;
            this.ForceCrash = false;
            this.ErrorVisibility = Visibility.Collapsed;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Items.Clear();
            this.ErrorVisibility = Visibility.Collapsed;
            IsBusy = Visibility.Visible;
            try
            {
                await Task.Delay(1500);
                if (ForceCrash)
                {
                    throw new Exception();
                }
                else
                {
                    IsBusy = Visibility.Collapsed;
                    for (int i = 0; i < 50; i++)
                    {
                        this.Items.Add(Guid.NewGuid().ToString());
                    }
                }
            }
            catch (Exception)
            {
                this.ErrorVisibility = Visibility.Visible;
                IsBusy = Visibility.Collapsed;
            }
        }
    }
}
