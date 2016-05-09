using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Media.Casting;
using AppStudio.Uwp.EventArguments;

namespace AppStudio.Uwp.Controls
{
    public partial class GifControl : Control
    {
        public event RoutedEventHandler ImageOpened;
        public event EventHandler<ExceptionEventArgs> ImageFailed;

        private Image _image = null;

        private bool _isInitialized = false;
        private bool _isPlaying = false;

        public GifControl()
        {
            this.DefaultStyleKey = typeof(GifControl);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
            this.InitializeTimer();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _timer.Start();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _timer.Stop();
            }
        }

        #region Source
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GifControl;
            control.SetSource(e.NewValue as Uri);
        }

        public async void SetSource(Uri uri)
        {
            if (_isInitialized)
            {
                if (uri != null)
                {
                    await ProcessSourceAsync(uri);
                    if (this.AutoPlay)
                    {
                        Play();
                    }
                    else
                    {
                        await PlayFrameAsync();
                    }
                }
                else
                {
                    _timer.Stop();
                    _isPlaying = false;
                }
            }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(GifControl), new PropertyMetadata(null, SourceChanged));
        #endregion

        #region AutoPlay
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        private static void AutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GifControl;
            control.SetAutoPlay((bool)e.NewValue);
        }

        private void SetAutoPlay(bool autoPlay)
        {
            if (_isInitialized)
            {
                if (autoPlay && !_timer.IsEnabled)
                {
                    this.Play();
                }
            }
        }

        public static readonly DependencyProperty AutoPlayProperty = DependencyProperty.Register("AutoPlay", typeof(bool), typeof(GifControl), new PropertyMetadata(true, AutoPlayChanged));
        #endregion

        #region IsLooping
        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }

        public static readonly DependencyProperty IsLoopingProperty = DependencyProperty.Register("IsLooping", typeof(bool), typeof(GifControl), new PropertyMetadata(true));
        #endregion

        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(GifControl), new PropertyMetadata(Stretch.Uniform));
        #endregion

        #region NineGrid
        public Thickness NineGrid
        {
            get { return (Thickness)GetValue(NineGridProperty); }
            set { SetValue(NineGridProperty, value); }
        }

        private static void NineGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GifControl;
            control.SetNineGrid((Thickness)e.NewValue);
        }

        private void SetNineGrid(Thickness newValue)
        {
            if (_isInitialized)
            {
                _image.NineGrid = newValue;
            }
        }

        public static readonly DependencyProperty NineGridProperty = DependencyProperty.Register("NineGrid", typeof(Thickness), typeof(GifControl), new PropertyMetadata(new Thickness(), NineGridChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _image = base.GetTemplateChild("image") as Image;

            _isInitialized = true;

            this.SetSource(this.Source);
            this.SetNineGrid(this.NineGrid);

            base.OnApplyTemplate();
        }

        public void Play()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Start();
            _isPlaying = true;
        }

        public void Pause()
        {
            _timer.Stop();
            _isPlaying = false;
        }

        public async void Stop()
        {
            _timer.Stop();
            _index = 0;
            await PlayFrameAsync();
            _isPlaying = false;
        }

        public CastingSource GetAsCastingSource()
        {
            if (_isInitialized)
            {
                return _image.GetAsCastingSource();
            }
            return null;
        }
    }
}
