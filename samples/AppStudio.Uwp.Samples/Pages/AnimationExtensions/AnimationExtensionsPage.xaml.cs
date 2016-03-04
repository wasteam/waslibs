using System;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using AppStudio.Uwp.Samples.Extensions;
using System.Windows.Input;
using AppStudio.Uwp.Commands;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Tools", Name = "AnimationExtensions")]
    public sealed partial class AnimationExtensionsPage : SamplePage
    {
        public AnimationExtensionsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private bool _isBusy = false;

        #region Commands
        public ICommand FadeInCommand
        {
            get
            {
                return new RelayCommand(async () =>
               {
                   _isBusy = true;
                   await sampleGrid.AnimateDoublePropertyAsync("Opacity", 1.0, 0.0, 500);
                   await sampleGrid.AnimateDoublePropertyAsync("Opacity", 0.0, 1.0, 500);
                   _isBusy = false;
               });
            }
        }
        public ICommand AnimateWidthCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    await sampleGrid.AnimateDoublePropertyAsync("Width", 200.0, 400.0, 500);
                    await sampleGrid.AnimateDoublePropertyAsync("Width", 400.0, 200.0, 500);                    
                    _isBusy = false;
                }, CanAnimate);
            }
        }        
        public ICommand AnimateHeightCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    await sampleGrid.AnimateDoublePropertyAsync("Height", 200.0, 400.0, 500);
                    await sampleGrid.AnimateDoublePropertyAsync("Height", 400.0, 200.0, 500);
                    _isBusy = false;
                }, CanAnimate);
            }
        }
        public ICommand GrowCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    sampleGrid.AnimateDoubleProperty("Width", 200.0, 250.0, 300);
                    sampleGrid.AnimateDoubleProperty("Height", 200.0, 250.0, 300);
                    await Task.Delay(300);
                    sampleGrid.AnimateDoubleProperty("Width", 250.0, 150.0, 300);
                    sampleGrid.AnimateDoubleProperty("Height", 250.0, 150.0, 300);
                    await Task.Delay(300);
                    sampleGrid.AnimateDoubleProperty("Width", 150.0, 200.0, 300);
                    sampleGrid.AnimateDoubleProperty("Height", 150.0, 200.0, 300);
                    _isBusy = false;
                }, CanAnimate);
            }
        }
        public ICommand RotateCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    RotateTransform rotateTransform = new RotateTransform() { CenterX = 100, CenterY = 100 };
                    sampleGrid.RenderTransform = rotateTransform;
                    await rotateTransform.AnimateDoublePropertyAsync("Angle", 0.0, -360.0, 1000);
                    _isBusy = false;
                }, CanAnimate);
            }
        }
        public ICommand TranslateCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    TranslateTransform translateTransform = new TranslateTransform();
                    sampleGrid.RenderTransform = translateTransform;
                    translateTransform.AnimateDoubleProperty("X", 0.0, 100.0, 300);
                    translateTransform.AnimateDoubleProperty("Y", 0.0, 100.0, 300);
                    await Task.Delay(500);
                    translateTransform.AnimateDoubleProperty("X", 100.0, 0.0, 300);
                    translateTransform.AnimateDoubleProperty("Y", 100.0, 0.0, 300);
                    _isBusy = false;
                }, CanAnimate);
            }
        }
        public ICommand SkewCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _isBusy = true;
                    SkewTransform skewTransform = new SkewTransform() { CenterX = 100, CenterY = 100 };
                    sampleGrid.RenderTransform = skewTransform;
                    await skewTransform.AnimateDoublePropertyAsync("AngleX", 0.0, 20.0, 300);                    
                    await skewTransform.AnimateDoublePropertyAsync("AngleX", 20.0, -20.0, 600);
                    await skewTransform.AnimateDoublePropertyAsync("AngleX", -20.0, 0.0, 300);
                    await skewTransform.AnimateDoublePropertyAsync("AngleY", 0.0, 20.0, 300);
                    await skewTransform.AnimateDoublePropertyAsync("AngleY", 20.0, -20.0, 600);
                    await skewTransform.AnimateDoublePropertyAsync("AngleY", -20.0, 0.0, 300);
                    _isBusy = false;
                }, CanAnimate);
            }
        }
        private bool CanAnimate() => !_isBusy;
        #endregion

        public override string Caption
        {
            get { return "Animation Extensions"; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
