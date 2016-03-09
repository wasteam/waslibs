using System;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage;

namespace AppStudio.Uwp.Samples
{
    public static class StorageExtensions
    {
        public static async Task<string> ReadTextAsync(this StorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                using (var reader = new StreamReader(stream.AsStreamForRead()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
