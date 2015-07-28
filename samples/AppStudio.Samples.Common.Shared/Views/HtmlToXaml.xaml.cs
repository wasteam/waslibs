using AppStudio.Common;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppStudio.Samples.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HtmlToXaml : Page, INotifyPropertyChanged
    {
        private string _htmlContent;
        public string HtmlContent
        {
            get { return _htmlContent; }
            set { _htmlContent = value; OnPropertyChanged("HtmlContent"); }
        }
        public HtmlToXaml()
        {
            this.InitializeComponent();
            HtmlContent = "<div><p><b>Lorem ipsum dolor sit amet</b>, consectetur adipiscing elit.Donec et tortor augue. Praesent felis lacus, vehicula vehicula convallis vitae, suscipit non augue. Nullam congue nibh a rhoncus accumsan. <i>Aenean mauris risus, tristique vestibulum tellus vel, porttitor scelerisque elit. Vivamus arcu mi, placerat nec mattis sed, posuere quis felis.</i> Quisque volutpat sit amet justo consequat pulvinar.Morbi elementum vulputate magna at venenatis.</p><p><img src=\"https://www.microsoft.com/global/learning/en-us/PublishingImages/ms-logo-site-share.png\"/></p><p>Ut mollis venenatis porttitor.Pellentesque eu arcu at leo ultrices congue.Nullam ac pellentesque lectus. Phasellus maximus, magna at porttitor pretium, eros mauris tempor enim, sed fringilla velit ipsum quis dolor.Suspendisse non nisl rhoncus, mattis justo nec, fermentum sem.Etiam imperdiet consequat quam quis ullamcorper. Ut vitae nunc malesuada, varius nulla at, posuere neque.Cras at arcu lobortis, efficitur libero quis, interdum tellus.Curabitur sagittis sed est vel pharetra. <b>Donec tincidunt leo mi, eu hendrerit nunc viverra a. Pellentesque euismod erat eget condimentum tempus.</b></p><p>Sed tempus pharetra erat, et finibus diam faucibus vel.Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. <b>Aliquam erat volutpat.Aenean urna augue, aliquet ac efficitur eu, sagittis eu ipsum. Duis at eros nec metus mattis accumsan at et nibh. Praesent eget sem eros</b>. Phasellus ullamcorper justo sagittis nunc molestie, nec dapibus dui auctor.</p></div>";
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
