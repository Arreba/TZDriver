using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZDriver.Models.Tools.Extensions;

namespace TZDriver.Models.Tools.Helpers
{
    /// <summary>
    /// Grouping of items by key into ObservableCollection
    /// </summary>
    public class Grouping<TKey, TItem> : ObservableCollection<TItem>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public TKey Key { get; }

        /// <summary>
        /// Returns list of items in the grouping.
        /// </summary>
        public new IList<TItem> Items => base.Items;

        /// <summary>
        /// Initializes a new instance of the Grouping class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="items">Items.</param>
        public Grouping(TKey key, IEnumerable<TItem> items)
        {
            Key = key;
            this.AddRange(items);
        }
    }

    /// <summary>
    /// Grouping of items by key into ObservableRange
    /// </summary>
    public class Grouping<TKey, TSubKey, TItem> : ObservableCollection<TItem>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public TKey Key { get; }

        /// <summary>
        /// Gets the subkey of the grouping
        /// </summary>
        public TSubKey SubKey { get; }

        /// <summary>
        /// Returns list of items in the grouping.
        /// </summary>
        public new IList<TItem> Items => base.Items;

        /// <summary>
        /// Initializes a new instance of the Grouping class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="subkey">Subkey</param>
        /// <param name="items">Items.</param>
        public Grouping(TKey key, TSubKey subkey, IEnumerable<TItem> items)
        {
            Key = key;
            SubKey = subkey;
            this.AddRange(items);
        }
    }
}
