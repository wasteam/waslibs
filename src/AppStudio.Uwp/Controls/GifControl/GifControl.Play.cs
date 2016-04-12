using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Web.Http;

namespace AppStudio.Uwp.Controls
{
    partial class GifControl
    {
        private DispatcherTimer _timer = null;

        private int _index = 0;
        List<BitmapFrame> _frames = null;

        private int _width;
        private int _height;
        private byte[] _pixels = null;

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;
        }

        private async void OnTimerTick(object sender, object e)
        {
            await PlayFrameAsync();

            _index = (_index + 1) % _frames.Count;

            if (_index == 0 && !this.IsLooping)
            {
                this.Stop();
            }
        }

        private async Task PlayFrameAsync()
        {
            var frame = _frames[_index];
            var props = await GetFramePropertiesAsync(frame);
            _timer.Interval = TimeSpan.FromMilliseconds(props.DelayMilliseconds);

            if (_index == 0 || props.ShouldDispose)
            {
                _width = (int)props.Rect.Width;
                _height = (int)props.Rect.Height;
                _pixels = await GetPixelsAsync(frame);
            }
            else
            {
                var pixels = await GetPixelsAsync(frame);
                MergePixels(_pixels, _width, _height, pixels, props.Rect);
            }

            _image.Source = LoadImage(_pixels, _width, _height);
        }

        private async Task ProcessSourceAsync(Uri uri)
        {
            if (uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https"))
            {
                using (var httpClient = new HttpClient())
                {
                    using (var httpMessage = await httpClient.GetAsync(uri))
                    {
                        using (var stream = new InMemoryRandomAccessStream())
                        {
                            await httpMessage.Content.WriteToStreamAsync(stream);
                            _frames = await LoadFramesAsync(stream);
                        }
                    }
                }
            }
            else
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                using (var stream = await file.OpenReadAsync())
                {
                    _frames = await LoadFramesAsync(stream);
                }
            }
        }

        private async Task<List<BitmapFrame>> LoadFramesAsync(IRandomAccessStream stream)
        {
            var frames = new List<BitmapFrame>();
            var decoder = await BitmapDecoder.CreateAsync(stream);

            uint count = decoder.FrameCount;
            for (uint n = 0; n < count; n++)
            {
                var frame = await decoder.GetFrameAsync(n);
                frames.Add(frame);
            }

            return frames;
        }
    }
}
