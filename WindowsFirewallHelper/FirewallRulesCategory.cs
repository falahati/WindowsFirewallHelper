using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Third-party firewall rule categories to have ownership from Windows Firewall
    /// </summary>
    public enum FirewallRuleCategory
    {
        /// <summary>
        ///     Boot category
        /// </summary>
        Boot = NetFwRuleCategory.Boot,

        /// <summary>
        ///     Stealth category
        /// </summary>
        Stealth = NetFwRuleCategory.Stealth,

        /// <summary>
        ///     Firewall rules
        /// </summary>
        Firewall = NetFwRuleCategory.Firewall,

        /// <summary>
        ///     IPSec rules
        /// </summary>
        ConnectionSecurity = NetFwRuleCategory.ConnectionSecurity
    }
}