using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Firewall rule actions
    /// </summary>
    public enum FirewallAction
    {
        /// <summary>
        ///     Block rule
        /// </summary>
        Block = NET_FW_ACTION.NET_FW_ACTION_BLOCK,

        /// <summary>
        ///     Allow rule
        /// </summary>
        Allow = NET_FW_ACTION.NET_FW_ACTION_ALLOW
    }
}