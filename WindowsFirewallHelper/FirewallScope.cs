using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Firewall rule scope
    /// </summary>
    public enum FirewallScope
    {
        /// <summary>
        ///     All scopes
        /// </summary>
        All = NetFwScope.All,

        /// <summary>
        ///     Local subnet only
        /// </summary>
        LocalSubnet = NetFwScope.LocalSubnet,

        /// <summary>
        ///     Specific list of addresses
        /// </summary>
        Specific = NetFwScope.Custom
    }
}