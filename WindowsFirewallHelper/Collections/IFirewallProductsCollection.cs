using System.Collections.Generic;

namespace WindowsFirewallHelper.Collections
{
    /// <summary>
    ///     Represents the collection of registered firewall products
    /// </summary>
    public interface IFirewallProductsCollection : ICollection<FirewallProduct>
    {
        /// <summary>
        ///     Gets the element at the specified index
        /// </summary>
        /// <param name="index">The zero-based index of the element to get</param>
        /// <returns>The element at the specified index</returns>
        FirewallProduct this[int index] { get; }

        /// <summary>
        ///     Determines the index of a specific item in the collection
        /// </summary>
        /// <param name="product">The element to get the collection index for</param>
        /// <returns>The index of product if found in the collection; otherwise, -1.</returns>
        int IndexOf(FirewallProduct product);
    }
}