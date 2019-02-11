using System.Collections;
using System.Collections.Generic;

namespace WindowsFirewallHelper.InternalHelpers.Collections
{
    /// <inheritdoc cref="ICollection" />
    /// <summary>Represents a generic collection of COM object representations.</summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IComCollection<in TKey, TValue> : ICollection<TValue>, ICollection
        where TValue : class
    {
        /// <summary>Gets or sets the element with the specified key.</summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        TValue this[TKey key] { get; }

        /// <summary>
        ///     Determines whether the <see cref="IComCollection{TKey,TValue}"></see> contains an element with the specified
        ///     key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IComCollection{TKey,TValue}"></see>.</param>
        /// <returns>true if the <see cref="IComCollection{TKey,TValue}"></see> contains an element with the key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        bool Contains(TKey key);

        /// <summary>Removes the element with the specified key from the <see cref="IComCollection{TKey,TValue}"></see>.</summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///     true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///     <paramref name="key">key</paramref> was not found in the original
        ///     <see cref="IComCollection{TKey,TValue}"></see>.
        /// </returns>
        bool Remove(TKey key);
    }
}