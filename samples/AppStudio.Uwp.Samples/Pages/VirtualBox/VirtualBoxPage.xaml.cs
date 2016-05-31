using Windows.UI.Xaml;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "VirtualBox", Order = 20)]
    public sealed partial class VirtualBoxPage : SamplePage
    {
        public VirtualBoxPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
            this.Text = "Etiam ac diam nisi, et mattis diam. Praesent congue porta libero a feugiat. Suspendisse in nisl et nisl gravida faucibus. Quisque sagittis sodales faucibus. Integer rhoncus interdum mi, a gravida orci eleifend id. Curabitur tristique, lacus sed volutpat imperdiet, nisl velit pretium sem, et scelerisque quam nibh at libero. Donec felis enim, ornare at molestie ac, fringilla quis dolor. Integer nec orci vel purus sodales aliquet. Quisque ac erat mi. Maecenas in ligula tellus. Quisque non sapien vel diam fringilla lacinia. In et metus odio, vel rhoncus ipsum.";
        }

        public override string Caption
        {
            get { return "VirtualBox Sample"; }
        }

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(VirtualBoxPage), new PropertyMetadata(null));
        #endregion

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new VirtualBoxSettings() { DataContext = control });
        }
    }
}
