using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Storage;

namespace AppStudio.Uwp.Controls
{
    public static class BitmapCache
    {
        public const int MAXRESOLUTION = 1920;
        public const int MIDRESOLUTION = 960;

        const string FOLDER_NAME = "ImageCache";

        static private Dictionary<string, Task> _concurrentTasks = new Dictionary<string, Task>();
        static private object _lock = new object();

        static BitmapCache()
        {
            CacheDuration = TimeSpan.FromHours(24);
        }

        static public TimeSpan CacheDuration { get; set; }

        #region ClearCacheAsync
        public static async Task ClearCacheAsync(TimeSpan? duration = null)
        {
            duration = duration ?? TimeSpan.FromSeconds(0);
            DateTime expirationDate = DateTime.Now.Subtract(duration.Value);
            try
            {
                var folder = await GetCacheFolderAsync();
                foreach (var file in await folder.GetFilesAsync())
                {
                    try
                    {
                        if (file.DateCreated < expirationDate)
                        {
                            await file.DeleteAsync();
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
        #endregion

        public static async Task<Uri> GetImageUriAsync(Uri uri, int maxWidth, int maxHeight)
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

            try
            {
                await busy;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            lock (_lock)
            {
                if (_concurrentTasks.ContainsKey(key))
                {
                    _concurrentTasks.Remove(key);
                }
            }

            string fileName = BuildFileName(uri, maxWidth, maxHeight);
            var cacheFolder = await GetCacheFolderAsync();
            if (await cacheFolder.TryGetItemAsync(fileName) != null)
            {
                return new Uri($"ms-appdata:///temp/{FOLDER_NAME}/{fileName}");
            }
            return new Uri($"ms-appdata:///temp/{FOLDER_NAME}/{BuildFileName(uri, MAXRESOLUTION, MAXRESOLUTION)}");
        }

        private static async Task EnsureFilesAsync(Uri uri)
        {
            DateTime expirationDate = DateTime.Now.Subtract(CacheDuration);

            var cacheFolder = await GetCacheFolderAsync();

            string fileName = BuildFileName(uri, MAXRESOLUTION, MAXRESOLUTION);
            StorageFile mainFile = await cacheFolder.TryGetItemAsync(fileName) as StorageFile;
            if (await IsFileOutOfDate(mainFile, expirationDate))
            {
                mainFile = await cacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if (!await BitmapTools.DownloadImageAsync(mainFile, uri, MAXRESOLUTION, MAXRESOLUTION))
                {
                    await mainFile.DeleteAsync();
                    return;
                }
            }

            fileName = BuildFileName(uri, MIDRESOLUTION, MIDRESOLUTION);
            var resizedFile = await cacheFolder.TryGetItemAsync(fileName) as StorageFile;
            if (await IsFileOutOfDate(resizedFile, expirationDate))
            {
                resizedFile = await cacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                try
                {
                    await BitmapTools.ResizeImageUniformAsync(mainFile, resizedFile, MIDRESOLUTION, MIDRESOLUTION);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    await resizedFile.DeleteAsync();
                }
            }
        }

        private static async Task<bool> IsFileOutOfDate(StorageFile file, DateTime expirationDate)
        {
            if (file != null)
            {
                var properties = await file.GetBasicPropertiesAsync();
                return properties.DateModified < expirationDate;
            }
            return true;
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
            return $"{uriHash}";
        }

        private static string BuildFileName(Uri uri, int maxWidth, int maxHeight)
        {
            string prefix = GetPrefixName(maxWidth, maxHeight);
            ulong uriHash = CreateHash64(uri);

            return $"{prefix}.{uriHash}";
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

        private static string GetPrefixName(double width, double height)
        {
            if (width <= MIDRESOLUTION && height <= MIDRESOLUTION)
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
            if (width <= MIDRESOLUTION && height <= MIDRESOLUTION)
            {
                return 1;
            }
            return 0;
        }
        #endregion
    }
}
