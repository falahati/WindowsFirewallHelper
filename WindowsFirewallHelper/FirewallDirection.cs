using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Firewall rule direction
    /// </summary>
    public enum FirewallDirection
    {
        /// <summary>
        ///     Inbound data
        /// </summary>
        Inbound = NetFwRuleDirection.Inbound,

        /// <summary>
        ///     Outbound data
        /// </summary>
        Outbound = NetFwRuleDirection.Outbound
    }
}