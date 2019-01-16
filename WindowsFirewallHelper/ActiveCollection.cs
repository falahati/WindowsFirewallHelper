using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace WindowsFirewallHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a dynamic data collection that provides notifications when items get added or removed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ActiveCollection<T> : Collection<T>
    {
        /// <summary>
        ///     An event to be raised when an Item got removed or added to the collection
        /// </summary>
        public event EventHandler<ActiveCollectionChangedEventArgs<T>> ItemsModified;

        private readonly object _syncLock = new object();

        /// <summary>
        ///     Syncs this ActiveCollection object with the provided <see cref="Array" />
        /// </summary>
        /// <param name="newItems"></param>
        // ReSharper disable once TooManyDeclarations
        public void Sync(T[] newItems)
        {
            lock (_syncLock)
            {
                var oldItemsHashTable = this
                    .Select(o => new {Item = o, HashCode = o.GetHashCode()})
                    .ToArray();
                var newItemsHashTable = newItems
                    .Select(n => new {Item = n, HashCode = n.GetHashCode()})
                    .ToArray();

                foreach (var newItem in newItemsHashTable
                    .Where(n => oldItemsHashTable.All(o => o.HashCode != n.HashCode))
                    .Select(n => n.Item)
                )
                {
                    Add(newItem);
                }

                foreach (var obj in oldItemsHashTable
                    .Where(o => newItemsHashTable.All(n => n.HashCode != o.HashCode))
                    .Select(o => o.Item)
                )
                {
                    Remove(obj);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes all elements from the <see cref="T:WindowsFirewallHelper.Helpers.ActiveCollection" />.
        /// </summary>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected override void ClearItems()
        {
            var items = this.ToArray();
            base.ClearItems();

            foreach (var item in items)
            {
                ItemsModified?.Invoke(
                    this,
                    new ActiveCollectionChangedEventArgs<T>(ActiveCollectionChangeType.Removed, item)
                );
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Inserts an element into the <see cref="T:WindowsFirewallHelper.Helpers.ActiveCollection" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is less than zero.-or-
        ///     <paramref name="index" /> is greater than <see cref="P:WindowsFirewallHelper.Helpers.ActiveCollection.Count" />.
        /// </exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            ItemsModified?.Invoke(
                this,
                new ActiveCollectionChangedEventArgs<T>(ActiveCollectionChangeType.Added, item)
            );
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes the element at the specified index of the <see cref="T:WindowsFirewallHelper.Helpers.ActiveCollection" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is less than zero.-or-
        ///     <paramref name="index" /> is equal to or greater than
        ///     <see cref="P:WindowsFirewallHelper.Helpers.ActiveCollection.Count" />.
        /// </exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected override void RemoveItem(int index)
        {
            var item = Items[index];
            base.RemoveItem(index);
            ItemsModified?.Invoke(
                this,
                new ActiveCollectionChangedEventArgs<T>(ActiveCollectionChangeType.Removed, item)
            );
        }

        /// <inheritdoc />
        /// <summary>
        ///     Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is less than zero.-or-
        ///     <paramref name="index" /> is greater than <see cref="P:WindowsFirewallHelper.Helpers.ActiveCollection.Count" />.
        /// </exception>
        /// <exception cref="T:System.Exception">A delegate callback throws an exception.</exception>
        protected override void SetItem(int index, T item)
        {
            var replaced = Items[index];
            base.SetItem(index, item);
            ItemsModified?.Invoke(
                this,
                new ActiveCollectionChangedEventArgs<T>(ActiveCollectionChangeType.Removed, replaced)
            );
            ItemsModified?.Invoke(
                this,
                new ActiveCollectionChangedEventArgs<T>(ActiveCollectionChangeType.Added, item)
            );
        }
    }
}