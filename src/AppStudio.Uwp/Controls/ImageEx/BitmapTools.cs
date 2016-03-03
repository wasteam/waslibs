using System;
using System.Threading;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Web.Http;

namespace AppStudio.Uwp.Controls
{
    public static class BitmapTools
    {
        static private SemaphoreSlim _semaphore = new SemaphoreSlim(10);

        public static async Task<bool> DownloadImageAsync(StorageFile file, Uri uri, int maxWidth = Int32.MaxValue, int maxHeight = Int32.MaxValue)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var httpMessage = await httpClient.GetAsync(uri))
                    {
                        using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await httpMessage.Content.WriteToStreamAsync(fileStream);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task ResizeImageUniformAsync(StorageFile sourceFile, StorageFile targetFile, int maxWidth = Int32.MaxValue, int maxHeight = Int32.MaxValue)
        {
            Size finalSize;
            byte[] pixels = null;

            using (var stream = await sourceFile.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                maxWidth = Math.Min(maxWidth, (int)decoder.OrientedPixelWidth);
                maxHeight = Math.Min(maxHeight, (int)decoder.OrientedPixelHeight);
                var imageSize = new Size(decoder.OrientedPixelWidth, decoder.OrientedPixelHeight);
                finalSize = imageSize.ToUniform(new Size(maxWidth, maxHeight));

                if (finalSize.Width == decoder.OrientedPixelWidth && finalSize.Height == decoder.OrientedPixelHeight)
                {
                    await sourceFile.CopyAndReplaceAsync(targetFile);
                    return;
                }

                await _semaphore.WaitAsync();
                try
                {
                    var bitmapTransform = new BitmapTransform()
                    {
                        ScaledWidth = (uint)finalSize.Width,
                        ScaledHeight = (uint)finalSize.Height,
                        InterpolationMode = BitmapInterpolationMode.Fant
                    };

                    var pixelProvider = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, decoder.BitmapAlphaMode, bitmapTransform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                    pixels = pixelProvider.DetachPixelData();

                    using (var fileStream = await targetFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
                        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)finalSize.Width, (uint)finalSize.Height, 96, 96, pixels);
                        await encoder.FlushAsync();
                    }
                }
                catch
                {
                    _semaphore.Release();
                    throw;
                }

                _semaphore.Release();
            }
        }

        public static async Task ResizeImageUniformToFillAsync(StorageFile sourceFile, StorageFile targetFile, int maxWidth = Int32.MaxValue, int maxHeight = Int32.MaxValue)
        {
            Size finalSize;
            byte[] pixels = null;

            using (var stream = await sourceFile.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                maxWidth = Math.Min(maxWidth, (int)decoder.OrientedPixelWidth);
                maxHeight = Math.Min(maxHeight, (int)decoder.OrientedPixelHeight);
                var imageSize = new Size(decoder.OrientedPixelWidth, decoder.OrientedPixelHeight);
                finalSize = imageSize.ToUniformToFill(new Size(maxWidth, maxHeight));

                var bitmapTransform = new BitmapTransform()
                {
                    ScaledWidth = (uint)finalSize.Width,
                    ScaledHeight = (uint)finalSize.Height,
                    InterpolationMode = BitmapInterpolationMode.Fant
                };

                var pixelProvider = await decoder.GetPixelDataAsync(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Ignore, bitmapTransform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                pixels = pixelProvider.DetachPixelData();
            }

            using (var fileStream = await targetFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Ignore, (uint)finalSize.Width, (uint)finalSize.Height, 96, 96, pixels);
                await encoder.FlushAsync();
            }
        }
    }
}
