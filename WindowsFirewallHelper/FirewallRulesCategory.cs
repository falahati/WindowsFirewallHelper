using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    public enum FirewallRuleCategory
    {
        Boot = NetFwRuleCategory.Boot,
        Stealth = NetFwRuleCategory.Stealth,
        Firewall = NetFwRuleCategory.Firewall,
        ConnectionSecurity = NetFwRuleCategory.ConnectionSecurity
    }
}