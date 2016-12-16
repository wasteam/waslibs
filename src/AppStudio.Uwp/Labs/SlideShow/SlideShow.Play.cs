﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace AppStudio.Uwp.Labs
{
    partial class SlideShow
    {
        private Storyboard _delayStoryboard = null;
        private Storyboard _fadeStoryboard = null;

        private bool _isStarted = false;
        private bool _isRunning = false;
        private bool _stopRequest = false;

        #region SelectedIndex
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SlideShow), new PropertyMetadata(0));
        #endregion

        #region Start
        private void Start()
        {
            if (_items.Count == 0)
            {
                return;
            }

            _isStarted = true;

            int inx0 = SelectedIndex;
            object content0;
            object content1;

            lock (_itemsLock)
            {
                int inx1 = (SelectedIndex + 1) % _items.Count;
                content0 = _items[inx0];
                content1 = _items[inx1];
                SelectedIndex = inx1;
            }

            var back = CreateControl(content1);
            var fore = CreateControl(content0);
            _container.Children.Add(back);
            _container.Children.Add(fore);

            _isRunning = true;

            this.InitAnimation();
        }
        #endregion

        public void Stop()
        {
            _stopRequest = true;
        }

        public void Resume()
        {
            if (!_isRunning)
            {
                this.Switch();
                this.InitAnimation();
                _isRunning = true;
            }
        }

        private void InitAnimation()
        {
            var back = _container.Children[0] as Control;
            var fore = _container.Children[1] as Control;

            back.ScaleX(0.9);
            _delayStoryboard = back.AnimateScaleX(1.0, DelayInterval);
            fore.AnimateScaleX(1.1, DelayInterval + FadeInterval);
            fore.AnimateScaleY(1.1, DelayInterval + FadeInterval);

            _delayStoryboard.Completed += OnDelayCompleted;
        }

        private void OnDelayCompleted(object sender, object e)
        {
            var back = _container.Children[0] as Control;
            var fore = _container.Children[1] as Control;

            back.AnimateScaleX(1.1, FadeInterval + DelayInterval + FadeInterval + 250);
            back.AnimateScaleY(1.1, FadeInterval + DelayInterval + FadeInterval + 250);
            _fadeStoryboard = fore.FadeOut(FadeInterval);

            if (_stopRequest)
            {
                _isRunning = false;
                _stopRequest = false;
            }

            if (_isRunning && _fadeStoryboard != null)
            {
                _fadeStoryboard.Completed += OnFadeCompleted;
            }
        }

        private void OnFadeCompleted(object sender, object e)
        {
            this.Switch();

            var back = _container.Children[0] as Control;

            back.ScaleX(0.9);
            _delayStoryboard = back.AnimateScaleX(1.0, DelayInterval);

            if (_stopRequest)
            {
                _isRunning = false;
                _stopRequest = false;
            }

            if (_isRunning && _delayStoryboard != null)
            {
                _delayStoryboard.Completed += OnDelayCompleted;
            }
        }

        #region Switch
        private void Switch()
        {
            SelectedIndex = (SelectedIndex + 1) % _items.Count;

            _container.Children.RemoveAt(1);
            _container.Children.Insert(0, CreateControl(_items[SelectedIndex]));
        }
        #endregion

        #region CreateControl
        private Control CreateControl(object content)
        {
            var control = new ContentControl
            {
                Content = content,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            control.SetBinding(ContentControl.ContentTemplateProperty, new Binding { Source = this, Path = new PropertyPath("ItemTemplate") });
            return control;
        }
        #endregion
    }
}
