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
        Block = NetFwAction.Block,

        /// <summary>
        ///     Allow rule
        /// </summary>
        Allow = NetFwAction.Allow
    }
}