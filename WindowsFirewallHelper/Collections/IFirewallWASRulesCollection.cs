using System.Collections.Generic;
using WindowsFirewallHelper.FirewallRules;

namespace WindowsFirewallHelper.Collections
{
    /// <summary>
    ///     Represents a collection of firewall with advanced security rules
    /// </summary>
    public interface IFirewallWASRulesCollection<T> : ICollection<T>
    {
        /// <summary>
        ///     Gets a rule by name
        ///     Note: The rule returned by this method is the first rule that meets the criteria. Use with caution.
        /// </summary>
        /// <param name="name">The rule's name</param>
        /// <returns>An instance of the <see cref="FirewallWASRule" /> class on success or null on failure</returns>
        FirewallWASRule this[string name] { get; }

        /// <summary>
        ///     Removes a rule by the name
        /// </summary>
        /// <param name="name">The rule's name</param>
        /// <returns>Returns a <see cref="bool" /> value indicating the success of the operation</returns>
        bool Remove(string name);
    }
}