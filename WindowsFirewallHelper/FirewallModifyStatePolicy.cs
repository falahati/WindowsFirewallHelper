using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Firewall modify state policies
    /// </summary>
    public enum FirewallModifyStatePolicy
    {
        /// <summary>
        ///     All modifications take effects immediately
        /// </summary>
        Ok = NetFwModifyState.Ok,

        /// <summary>
        ///     Firewall is controlled by group policy
        /// </summary>
        OverrodeByGroupPolicy = NetFwModifyState.GroupPolicyOverride,

        /// <summary>
        ///     All inbound traffic is blocked regardless of registered rules
        /// </summary>
        InboundBlocked = NetFwModifyState.InboundBlocked
    }
}