using System.Collections.Generic;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallRules;

namespace WindowsFirewallHelper.Collections
{
    /// <summary>
    ///     Represents a collection of firewall legacy rules containing both port based and application based rules
    /// </summary>
    public interface IFirewallLegacyRulesCollection : ICollection<IFirewallRule>
    {
        /// <summary>
        ///     Gets a legacy application rule
        ///     Note: The rule returned by this method may not be complete or even correct if multiple rules meet the criteria. Use
        ///     with caution.
        /// </summary>
        /// <param name="applicationPath">The rule's full application path</param>
        /// <returns>An instance of the <see cref="FirewallLegacyApplicationRule" /> class on success or null on failure</returns>
        FirewallLegacyApplicationRule this[string applicationPath] { get; }

        /// <summary>
        ///     Gets a legacy port rule
        ///     Note: The rule returned by this method may not be complete or even correct if multiple rules meet the criteria. Use
        ///     with caution.
        /// </summary>
        /// <param name="portNumber">The rule's port number</param>
        /// <param name="protocol">The rule's protocol</param>
        /// <returns>An instance of the <see cref="FirewallLegacyPortRule" /> class on success or null on failure</returns>
        FirewallLegacyPortRule this[ushort portNumber, NetFwIPProtocol protocol] { get; }

        /// <summary>
        ///     Removes a legacy port rule using the port number and the protocol
        /// </summary>
        /// <param name="portNumber">The rule's port number</param>
        /// <param name="protocol">The rule's protocol</param>
        /// <returns>Returns a <see cref="bool" /> value indicating the success of the operation</returns>
        bool Remove(ushort portNumber, NetFwIPProtocol protocol);

        /// <summary>
        ///     Removes a legacy application rule using the full application path
        /// </summary>
        /// <param name="applicationPath">The rule's full application path</param>
        /// <returns>Returns a <see cref="bool" /> value indicating the success of the operation</returns>
        bool Remove(string applicationPath);
    }
}