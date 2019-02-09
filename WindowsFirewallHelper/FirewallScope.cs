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
        All = NET_FW_SCOPE.NET_FW_SCOPE_ALL,

        /// <summary>
        ///     Local subnet only
        /// </summary>
        LocalSubnet = NET_FW_SCOPE.NET_FW_SCOPE_LOCAL_SUBNET,

        /// <summary>
        ///     Specific list of addresses
        /// </summary>
        Specific = NET_FW_SCOPE.NET_FW_SCOPE_CUSTOM
    }
}