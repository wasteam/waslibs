using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class ResponsiveGridView : BaseControlPage
    {
        private double _desiredWidth;
        public double DesiredWidth
        {
            get { return _desiredWidth; }
            set { SetProperty(ref _desiredWidth, value); }
        }
        private ObservableCollection<SolidColorBrush> _items;
        public ObservableCollection<SolidColorBrush> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public ResponsiveGridView()
        {
            this.InitializeComponent();
            DesiredWidth = 250;
            LoadData();
        }

        private void LoadData()
        {
            this.Items = new ObservableCollection<SolidColorBrush>();
            this.Items.Add(new SolidColorBrush(Colors.AliceBlue));
            this.Items.Add(new SolidColorBrush(Colors.Aqua));
            this.Items.Add(new SolidColorBrush(Colors.Aquamarine));
            this.Items.Add(new SolidColorBrush(Colors.Azure));
            this.Items.Add(new SolidColorBrush(Colors.Beige));
            this.Items.Add(new SolidColorBrush(Colors.Bisque));
            this.Items.Add(new SolidColorBrush(Colors.Black));
            this.Items.Add(new SolidColorBrush(Colors.BlanchedAlmond));
            this.Items.Add(new SolidColorBrush(Colors.Blue));
            this.Items.Add(new SolidColorBrush(Colors.BlueViolet));
            this.Items.Add(new SolidColorBrush(Colors.Brown));
            this.Items.Add(new SolidColorBrush(Colors.BurlyWood));
            this.Items.Add(new SolidColorBrush(Colors.CadetBlue));
            this.Items.Add(new SolidColorBrush(Colors.Chartreuse));
            this.Items.Add(new SolidColorBrush(Colors.Chocolate));
            this.Items.Add(new SolidColorBrush(Colors.Coral));
            this.Items.Add(new SolidColorBrush(Colors.CornflowerBlue));
            this.Items.Add(new SolidColorBrush(Colors.Cornsilk));
            this.Items.Add(new SolidColorBrush(Colors.Crimson));
            this.Items.Add(new SolidColorBrush(Colors.Cyan));
            this.Items.Add(new SolidColorBrush(Colors.DarkBlue));
            this.Items.Add(new SolidColorBrush(Colors.DarkCyan));
            this.Items.Add(new SolidColorBrush(Colors.DarkGoldenrod));
            this.Items.Add(new SolidColorBrush(Colors.DarkGray));
            this.Items.Add(new SolidColorBrush(Colors.DarkGreen));
            this.Items.Add(new SolidColorBrush(Colors.DarkKhaki));
            this.Items.Add(new SolidColorBrush(Colors.DarkMagenta));
            this.Items.Add(new SolidColorBrush(Colors.DarkOliveGreen));
            this.Items.Add(new SolidColorBrush(Colors.DarkOrange));
            this.Items.Add(new SolidColorBrush(Colors.DarkOrchid));
            this.Items.Add(new SolidColorBrush(Colors.DarkRed));
            this.Items.Add(new SolidColorBrush(Colors.DarkSalmon));
            this.Items.Add(new SolidColorBrush(Colors.DarkSeaGreen));
            this.Items.Add(new SolidColorBrush(Colors.DarkSlateBlue));
            this.Items.Add(new SolidColorBrush(Colors.DarkSlateGray));
            this.Items.Add(new SolidColorBrush(Colors.DarkTurquoise));
            this.Items.Add(new SolidColorBrush(Colors.DarkViolet));
            this.Items.Add(new SolidColorBrush(Colors.DeepPink));
            this.Items.Add(new SolidColorBrush(Colors.DeepSkyBlue));
            this.Items.Add(new SolidColorBrush(Colors.DimGray));
            this.Items.Add(new SolidColorBrush(Colors.DodgerBlue));
            this.Items.Add(new SolidColorBrush(Colors.Firebrick));
            this.Items.Add(new SolidColorBrush(Colors.FloralWhite));
            this.Items.Add(new SolidColorBrush(Colors.ForestGreen));
            this.Items.Add(new SolidColorBrush(Colors.Fuchsia));
            this.Items.Add(new SolidColorBrush(Colors.Gainsboro));
            this.Items.Add(new SolidColorBrush(Colors.GhostWhite));
            this.Items.Add(new SolidColorBrush(Colors.Gold));
            this.Items.Add(new SolidColorBrush(Colors.Goldenrod));
            this.Items.Add(new SolidColorBrush(Colors.Gray));
            this.Items.Add(new SolidColorBrush(Colors.Green));
            this.Items.Add(new SolidColorBrush(Colors.GreenYellow));
            this.Items.Add(new SolidColorBrush(Colors.Honeydew));
            this.Items.Add(new SolidColorBrush(Colors.HotPink));
            this.Items.Add(new SolidColorBrush(Colors.IndianRed));
            this.Items.Add(new SolidColorBrush(Colors.Indigo));
            this.Items.Add(new SolidColorBrush(Colors.Ivory));
            this.Items.Add(new SolidColorBrush(Colors.Khaki));
            this.Items.Add(new SolidColorBrush(Colors.Lavender));
            this.Items.Add(new SolidColorBrush(Colors.LavenderBlush));
            this.Items.Add(new SolidColorBrush(Colors.LawnGreen));
            this.Items.Add(new SolidColorBrush(Colors.LemonChiffon));
            this.Items.Add(new SolidColorBrush(Colors.LightBlue));
            this.Items.Add(new SolidColorBrush(Colors.LightCoral));
            this.Items.Add(new SolidColorBrush(Colors.LightCyan));
            this.Items.Add(new SolidColorBrush(Colors.LightGoldenrodYellow));
            this.Items.Add(new SolidColorBrush(Colors.LightGray));
            this.Items.Add(new SolidColorBrush(Colors.LightGreen));
            this.Items.Add(new SolidColorBrush(Colors.LightPink));
            this.Items.Add(new SolidColorBrush(Colors.LightSeaGreen));
            this.Items.Add(new SolidColorBrush(Colors.LightSkyBlue));
            this.Items.Add(new SolidColorBrush(Colors.LightSlateGray));
            this.Items.Add(new SolidColorBrush(Colors.LightSteelBlue));
            this.Items.Add(new SolidColorBrush(Colors.LightYellow));
            this.Items.Add(new SolidColorBrush(Colors.Lime));
            this.Items.Add(new SolidColorBrush(Colors.LimeGreen));
            this.Items.Add(new SolidColorBrush(Colors.Linen));
            this.Items.Add(new SolidColorBrush(Colors.Magenta));
            this.Items.Add(new SolidColorBrush(Colors.Maroon));
            this.Items.Add(new SolidColorBrush(Colors.MediumAquamarine));
            this.Items.Add(new SolidColorBrush(Colors.MediumBlue));
            this.Items.Add(new SolidColorBrush(Colors.MediumOrchid));
            this.Items.Add(new SolidColorBrush(Colors.MediumPurple));
            this.Items.Add(new SolidColorBrush(Colors.MediumSeaGreen));
            this.Items.Add(new SolidColorBrush(Colors.MediumSlateBlue));
            this.Items.Add(new SolidColorBrush(Colors.MediumSpringGreen));
            this.Items.Add(new SolidColorBrush(Colors.MediumTurquoise));
            this.Items.Add(new SolidColorBrush(Colors.MediumVioletRed));
            this.Items.Add(new SolidColorBrush(Colors.MidnightBlue));
            this.Items.Add(new SolidColorBrush(Colors.MintCream));
            this.Items.Add(new SolidColorBrush(Colors.MistyRose));
            this.Items.Add(new SolidColorBrush(Colors.Moccasin));
            this.Items.Add(new SolidColorBrush(Colors.NavajoWhite));
            this.Items.Add(new SolidColorBrush(Colors.Navy));
            this.Items.Add(new SolidColorBrush(Colors.OldLace));
            this.Items.Add(new SolidColorBrush(Colors.Olive));
            this.Items.Add(new SolidColorBrush(Colors.OliveDrab));
            this.Items.Add(new SolidColorBrush(Colors.Orange));
            this.Items.Add(new SolidColorBrush(Colors.OrangeRed));
            this.Items.Add(new SolidColorBrush(Colors.Orchid));
            this.Items.Add(new SolidColorBrush(Colors.PaleGoldenrod));
            this.Items.Add(new SolidColorBrush(Colors.PaleGreen));
            this.Items.Add(new SolidColorBrush(Colors.PaleTurquoise));
            this.Items.Add(new SolidColorBrush(Colors.PaleVioletRed));
            this.Items.Add(new SolidColorBrush(Colors.PapayaWhip));
            this.Items.Add(new SolidColorBrush(Colors.PeachPuff));
            this.Items.Add(new SolidColorBrush(Colors.Peru));
            this.Items.Add(new SolidColorBrush(Colors.Pink));
            this.Items.Add(new SolidColorBrush(Colors.Plum));
            this.Items.Add(new SolidColorBrush(Colors.PowderBlue));
            this.Items.Add(new SolidColorBrush(Colors.Purple));
            this.Items.Add(new SolidColorBrush(Colors.Red));
            this.Items.Add(new SolidColorBrush(Colors.RosyBrown));
            this.Items.Add(new SolidColorBrush(Colors.RoyalBlue));
            this.Items.Add(new SolidColorBrush(Colors.SaddleBrown));
            this.Items.Add(new SolidColorBrush(Colors.Salmon));
            this.Items.Add(new SolidColorBrush(Colors.SandyBrown));
            this.Items.Add(new SolidColorBrush(Colors.SeaGreen));
            this.Items.Add(new SolidColorBrush(Colors.SeaShell));
            this.Items.Add(new SolidColorBrush(Colors.Sienna));
            this.Items.Add(new SolidColorBrush(Colors.Silver));
            this.Items.Add(new SolidColorBrush(Colors.SkyBlue));
            this.Items.Add(new SolidColorBrush(Colors.SlateBlue));
            this.Items.Add(new SolidColorBrush(Colors.SlateGray));
            this.Items.Add(new SolidColorBrush(Colors.Snow));
            this.Items.Add(new SolidColorBrush(Colors.SpringGreen));
            this.Items.Add(new SolidColorBrush(Colors.SteelBlue));
            this.Items.Add(new SolidColorBrush(Colors.Tan));
            this.Items.Add(new SolidColorBrush(Colors.Teal));
            this.Items.Add(new SolidColorBrush(Colors.Thistle));
            this.Items.Add(new SolidColorBrush(Colors.Tomato));
            this.Items.Add(new SolidColorBrush(Colors.Turquoise));
            this.Items.Add(new SolidColorBrush(Colors.Violet));
            this.Items.Add(new SolidColorBrush(Colors.Wheat));
            this.Items.Add(new SolidColorBrush(Colors.WhiteSmoke));
            this.Items.Add(new SolidColorBrush(Colors.Yellow));
            this.Items.Add(new SolidColorBrush(Colors.YellowGreen));
        }
    }
}
