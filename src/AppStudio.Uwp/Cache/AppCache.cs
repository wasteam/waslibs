using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
namespace AppStudio.Uwp.Cache
{
    public static class AppCache
    {
        private static Dictionary<string, string> _memoryCache = new Dictionary<string, string>();

        [Obsolete("Use a custom loading strategy in your app")]
        public static async Task<DateTime?> LoadItemsAsync<T>(CacheSettings settings, Func<Task<IEnumerable<T>>> loadDataAsync, Action<CachedContent<T>> parseItems, bool refreshForced = false)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (string.IsNullOrEmpty(settings.Key))
            {
                throw new ArgumentException("Cache key is required");
            }

            var dataInCache = await AppCache.GetItemsAsync<T>(settings.Key);
            if (dataInCache != null)
            {
                parseItems(dataInCache);
            }

            if (CanPerformLoad<T>(settings.NeedsNetwork) && (refreshForced || DataNeedToBeUpdated(dataInCache, settings.Expiration)))
            {
                dataInCache = dataInCache ?? new CachedContent<T>();

                dataInCache.Timestamp = DateTime.Now;
                dataInCache.Items = await loadDataAsync();

                await AppCache.AddItemsAsync(settings.Key, dataInCache, settings.UseStorage);

                parseItems(dataInCache);
            }
            return dataInCache?.Timestamp;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public static async Task<CachedContent<T>> GetItemsAsync<T>(string key)
        {
            string json = null;
            if (_memoryCache.ContainsKey(key))
            {
                json = _memoryCache[key];
            }
            else
            {
                json = await UserStorage.ReadTextFromFileAsync(key);
                _memoryCache[key] = json;
            }
            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    CachedContent<T> records = await Json.ToObjectAsync<CachedContent<T>>(json);
                    return records;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return null;
        }

        public static async Task<T> GetItemAsync<T>(string key)
        {
            string json = null;
            if (_memoryCache.ContainsKey(key))
            {
                json = _memoryCache[key];
            }
            else
            {
                json = await UserStorage.ReadTextFromFileAsync(key);
                _memoryCache[key] = json;
            }
            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    T data = await Json.ToObjectAsync<T>(json);
                    return data;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return default(T);
        }

        public static async Task<List<T>> GetItemsByPrefixAsync<T>(string prefix)
        {
            List<T> results = new List<T>();

            List<string> keys = _memoryCache.Keys.Where(k => k.StartsWith(prefix)).ToList();
            List<string> inFileKeys = await UserStorage.GetMatchingFilesByPrefixAsync(prefix, keys);

            keys.AddRange(inFileKeys);

            foreach (var key in keys)
            {
                T data = await GetItemAsync<T>(key);
                results.Add(data);
            }
            return results;
        }

        public static async Task ClearItemsByPrefixAsync(string prefix)
        {
            List<string> keys = _memoryCache.Keys.Where(k => k.StartsWith(prefix)).ToList();

            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }

            List<string> inFileKeys = await UserStorage.GetMatchingFilesByPrefixAsync(prefix, keys);

            foreach (var fileKey in inFileKeys)
            {
                await UserStorage.DeleteFileIfExistsAsync(fileKey);
            }
        }

        public static async Task AddItemsAsync<T>(string key, CachedContent<T> data)
        {
            await AddItemsAsync<T>(key, data, true);
        }

        public static async Task AddItemsAsync<T>(string key, CachedContent<T> data, bool useStorageCache)
        {
            try
            {
                string json = await Json.StringifyAsync(data);

                if (useStorageCache)
                {
                    await UserStorage.WriteTextAsync(key, json);
                }

                if (_memoryCache.ContainsKey(key))
                {
                    _memoryCache[key] = json;
                }
                else
                {
                    _memoryCache.Add(key, json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        public static async Task AddItemAsync<T>(string key, T data)
        {
            await AddItemAsync<T>(key, data, true);
        }

        public static async Task AddItemAsync<T>(string key, T data, bool useStorageCache)
        {
            try
            {
                string json = await Json.StringifyAsync(data);

                if (useStorageCache)
                {
                    await UserStorage.WriteTextAsync(key, json);
                }

                if (_memoryCache.ContainsKey(key))
                {
                    _memoryCache[key] = json;
                }
                else
                {
                    _memoryCache.Add(key, json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static bool CanPerformLoad<T>(bool needNetwork)
        {
            return !needNetwork || NetworkInterface.GetIsNetworkAvailable();
        }

        private static bool DataNeedToBeUpdated<T>(CachedContent<T> dataInCache, TimeSpan expiration)
        {
            return dataInCache == null || (DateTime.Now - dataInCache.Timestamp > expiration);
        }
    }
}
