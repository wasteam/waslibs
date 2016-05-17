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

        static private SemaphoreSlim _semaphore = new SemaphoreSlim(10);

        public static async Task ResizeImageUniformAsync(StorageFile sourceFile, StorageFile targetFile, int maxWidth = Int32.MaxValue, int maxHeight = Int32.MaxValue)
        {
            using (var stream = await sourceFile.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                if (IsGifImage(decoder))
                {
                    await sourceFile.CopyAndReplaceAsync(targetFile);
                    return;
                }

                maxWidth = Math.Min(maxWidth, (int)decoder.OrientedPixelWidth);
                maxHeight = Math.Min(maxHeight, (int)decoder.OrientedPixelHeight);
                var imageSize = new Size(decoder.OrientedPixelWidth, decoder.OrientedPixelHeight);
                var finalSize = imageSize.ToUniform(new Size(maxWidth, maxHeight));

                if (finalSize.Width == decoder.OrientedPixelWidth && finalSize.Height == decoder.OrientedPixelHeight)
                {
                    await sourceFile.CopyAndReplaceAsync(targetFile);
                    return;
                }

                await ResizeImageAsync(decoder, targetFile, finalSize);
            }
        }

        public static async Task ResizeImageUniformToFillAsync(StorageFile sourceFile, StorageFile targetFile, int maxWidth = Int32.MaxValue, int maxHeight = Int32.MaxValue)
        {
            using (var stream = await sourceFile.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                if (IsGifImage(decoder))
                {
                    await sourceFile.CopyAndReplaceAsync(targetFile);
                    return;
                }

                maxWidth = Math.Min(maxWidth, (int)decoder.OrientedPixelWidth);
                maxHeight = Math.Min(maxHeight, (int)decoder.OrientedPixelHeight);
                var imageSize = new Size(decoder.OrientedPixelWidth, decoder.OrientedPixelHeight);
                var finalSize = imageSize.ToUniformToFill(new Size(maxWidth, maxHeight));

                if (finalSize.Width == decoder.OrientedPixelWidth && finalSize.Height == decoder.OrientedPixelHeight)
                {
                    await sourceFile.CopyAndReplaceAsync(targetFile);
                    return;
                }

                await ResizeImageAsync(decoder, targetFile, finalSize);
            }
        }

        private static async Task ResizeImageAsync(BitmapDecoder decoder, StorageFile targetFile, Size finalSize)
        {
            await _semaphore.WaitAsync();

            try
            {
                using (var fileStream = await targetFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);
                    encoder.BitmapTransform.ScaledWidth = (uint)finalSize.Width;
                    encoder.BitmapTransform.ScaledHeight = (uint)finalSize.Height;
                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                    await encoder.FlushAsync();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static bool IsGifImage(BitmapDecoder decoder)
        {
            foreach (var mimeType in decoder.DecoderInformation.MimeTypes)
            {
                if (mimeType.Equals("image/gif", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
