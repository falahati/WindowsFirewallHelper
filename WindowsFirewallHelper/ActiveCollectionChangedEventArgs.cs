using System;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     ActiveCollectionChangedEventArgs is a class containing event data about the
    ///     <see cref="ActiveCollectionChangeType" /> event.
    /// </summary>
    public class ActiveCollectionChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WindowsFirewallHelper.Helpers.ActiveCollectionChangedEventArgs" />
        ///     class.
        /// </summary>
        /// <param name="action">The type of change that happened.</param>
        /// <param name="item">The item of the collection that changed.</param>
        public ActiveCollectionChangedEventArgs(ActiveCollectionChangeType action, T item)
        {
            ActionType = action;
            Item = item;
        }

        /// <summary>
        ///     The type of change that happened.
        /// </summary>
        public ActiveCollectionChangeType ActionType { get; }

        /// <summary>
        ///     The item of the collection that changed.
        /// </summary>
        public T Item { get; }
    }
}