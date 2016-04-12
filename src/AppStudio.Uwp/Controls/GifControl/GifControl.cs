using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    public partial class GifControl : Control
    {
        private Image _image = null;

        private bool _isInitialized = false;

        public GifControl()
        {
            this.DefaultStyleKey = typeof(GifControl);
            this.InitializeTimer();
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

        protected override void OnApplyTemplate()
        {
            _image = base.GetTemplateChild("image") as Image;

            _isInitialized = true;

            this.SetSource(this.Source);

            base.OnApplyTemplate();
        }

        public void Play()
        {
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public async void Stop()
        {
            _timer.Stop();
            _index = 0;
            await PlayFrameAsync();
        }
    }
}
