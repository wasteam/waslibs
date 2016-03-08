using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public static class BitmapCache
    {
        const string FOLDER_NAME = "ImageCache";
        const int MAX_RESOLUTION = 1920;

        static private object _lock = new object();

        static private Dictionary<string, Task> _concurrentTasks = new Dictionary<string, Task>();

        #region ClearCache
        public static async Task ClearCache()
        {
            try
            {
                var folder = await GetCacheFolderAsync();
                foreach (var file in await folder.GetFilesAsync())
                {
                    try
                    {
                        await file.DeleteAsync();
                    }
                    catch { }
                }
            }
            catch { }
        }
        #endregion

        public static async Task<BitmapImage> LoadFromCacheAsync(Uri uri, int maxWidth, int maxHeight)
        {
            Task busy = null;
            string key = BuildKey(uri);

            lock (_lock)
            {
                if (_concurrentTasks.ContainsKey(key))
                {
                    busy = _concurrentTasks[key];
                }
                else
                {
                    busy = EnsureFilesAsync(uri);
                    _concurrentTasks.Add(key, busy);
                }
            }

            await busy;

            lock (_lock)
            {
                if (_concurrentTasks.ContainsKey(key))
                {
                    _concurrentTasks.Remove(key);
                }
            }

            return CreateBitmapImage(BuildFileName(uri, maxWidth, maxHeight));
        }

        private static BitmapImage CreateBitmapImage(string fileName)
        {
            return new BitmapImage(new Uri($"ms-appdata:///temp/{FOLDER_NAME}/{fileName}"));
        }

        private static async Task EnsureFilesAsync(Uri uri)
        {
            var folder = await GetCacheFolderAsync();

            string fileName = BuildFileName(uri, MAX_RESOLUTION, MAX_RESOLUTION);
            var baseFile = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (baseFile == null)
            {
                baseFile = await folder.CreateFileAsync(fileName);
                if (!await BitmapTools.DownloadImageAsync(baseFile, uri, MAX_RESOLUTION, MAX_RESOLUTION))
                {
                    await baseFile.DeleteAsync();
                    return;
                }
            }

            fileName = BuildFileName(uri, 960, 960);
            var file = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (file == null)
            {
                file = await folder.CreateFileAsync(fileName);
                try
                {
                    await BitmapTools.ResizeImageUniformAsync(baseFile, file, 960, 960);
                }
                catch
                {
                    await file.DeleteAsync();
                    return;
                }
            }
            baseFile = file;
        }

        private static async Task<StorageFile> TryGetFileAsync(Uri uri, int maxWidth, int maxHeight)
        {
            string fileName = BuildFileName(uri, maxWidth, maxHeight);
            StorageFolder folder = await GetCacheFolderAsync();
            return await folder.TryGetItemAsync(fileName) as StorageFile;
        }

        #region GetCacheFolder
        static private StorageFolder _cacheFolder = null;
        static private SemaphoreSlim _cacheFolderSemaphore = new SemaphoreSlim(1);

        private static async Task<StorageFolder> GetCacheFolderAsync()
        {
            if (_cacheFolder == null)
            {
                await _cacheFolderSemaphore.WaitAsync();
                try
                {
                    _cacheFolder = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(FOLDER_NAME) as StorageFolder;
                    if (_cacheFolder == null)
                    {
                        _cacheFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(FOLDER_NAME);
                    }
                }
                catch { }
                finally
                {
                    _cacheFolderSemaphore.Release();
                }
            }
            return _cacheFolder;
        }
        #endregion

        #region File Hash
        private static string BuildKey(Uri uri)
        {
            ulong uriHash = CreateHash64(uri);
            return $"{uriHash}.jpg";
        }

        private static string BuildFileName(Uri uri, int maxWidth, int maxHeight)
        {
            string prefix = GetPrefixName(maxWidth, maxHeight);
            ulong uriHash = CreateHash64(uri);

            return $"{prefix}.{uriHash}.jpg";
        }

        private static UInt64 CreateHash64(Uri uri)
        {
            return CreateHash64(uri.Host + uri.PathAndQuery);
        }

        private static UInt64 CreateHash64(string str)
        {
            byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(str);

            ulong value = (ulong)utf8.Length;
            for (int n = 0; n < utf8.Length; n++)
            {
                value += (ulong)utf8[n] << ((n * 5) % 56);
            }

            return value;
        }
        #endregion

        #region GetPrefixName
        private static string GetPrefixName(double width, double height)
        {
            if (width <= 960 && height <= 960)
            {
                return "M";
            }
            return "L";
        }
        #endregion

        #region GetSizeLevel
        public static int GetSizeLevel(Size size)
        {
            double width = size.Width;
            double height = size.Height;
            if (width <= 960 && height <= 960)
            {
                return 1;
            }
            return 0;
        }
        #endregion
    }
}
