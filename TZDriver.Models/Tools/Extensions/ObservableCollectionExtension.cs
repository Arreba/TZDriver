using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace TZDriver.Models.Tools.Extensions
{
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T).
        /// </summary>
        public static void AddRange<TSource>(this ObservableCollection<TSource> collection, IEnumerable<TSource> source)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in source)
            {
                collection.Add(item);
            }
        }

        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sortedSource = source.OrderBy(keySelector).ToList();

            for (var i = 0; i < sortedSource.Count; i++)
            {
                var itemToSort = sortedSource[i];

                // If the item is already at the right position, leave it and continue.
                if (source.IndexOf(itemToSort) == i)
                {
                    continue;
                }

                source.Remove(itemToSort);
                source.Insert(i, itemToSort);
            }
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The items.</param>
        /// <param name="collection">The collection.</param>
        public static void RemoveRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> collection)
        {
            // Remove range from local items
            collection.Apply(p => source.Remove(p));
        }

        //use it like this collection.RemoveRange(newCollection.Where(p => p.IsSelected == false));
    }
}
