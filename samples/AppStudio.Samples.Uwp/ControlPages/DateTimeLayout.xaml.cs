// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class DateTimeLayout : BaseControlPage
    {
        private System.DateTime _dateTimeValue;
        public System.DateTime DateTimeValue
        {
            get { return _dateTimeValue; }
            set { SetProperty(ref _dateTimeValue, value); }
        }

        public DateTimeLayout()
        {
            DateTimeValue = System.DateTime.Now;
            this.InitializeComponent();
        }
    }
}
