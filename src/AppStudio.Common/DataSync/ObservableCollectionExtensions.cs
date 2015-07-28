using System.Collections.Generic;
using System.Linq;

namespace AppStudio.Common.DataSync
{
    public static class ObservableCollectionExtensions
    {
        public static void Sync<T>(this IList<T> oldCollection, IList<T> newItems) where T : ISyncItem<T>
        {
            if (oldCollection != null && newItems != null)
            {
                // Gets all equal items to sync them.
                var modifiedItems = oldCollection.Intersect(newItems);
                foreach (var modifiedItem in modifiedItems)
                {
                    T newItem = newItems.First(ci => ci.Equals(modifiedItem));
                    if (modifiedItem.NeedSync(newItem))
                    {
                        modifiedItem.Sync(newItem);
                    }
                }

                // Removes removed items in the old collection.
                var itemsToRemove = oldCollection.Except(newItems).ToList();
                foreach (var item in itemsToRemove)
                {
                    oldCollection.Remove(item);
                }

                // Adds newly added items.
                var itemsToAdd = newItems.Except(oldCollection);

                foreach (var item in itemsToAdd)
                {
                    var index = newItems.IndexOf(item);
                    if (IsOutOfRange(oldCollection, index))
                    {
                        oldCollection.Add(item);
                    }
                    else
                    {
                        oldCollection.Insert(index, item);
                    }
                }
            }
        }

        private static bool IsOutOfRange<T>(IList<T> list, int index) where T : ISyncItem<T>
        {
            return index > list.Count;
        }
    }
}