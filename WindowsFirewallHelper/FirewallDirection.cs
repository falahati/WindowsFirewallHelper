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
        Inbound = NET_FW_RULE_DIRECTION.NET_FW_RULE_DIR_IN,

        /// <summary>
        ///     Outbound data
        /// </summary>
        Outbound = NET_FW_RULE_DIRECTION.NET_FW_RULE_DIR_OUT
    }
}