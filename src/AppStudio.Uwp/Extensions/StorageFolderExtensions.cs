using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppStudio.Uwp
{
    public static class StorageFolderExtensions
    {
        public static async Task<IStorageFile> TryGetFileAsync(this StorageFolder folder, string name)
        {
            return await folder.TryGetItemAsync(name) as IStorageFile;
        }
    }
}
