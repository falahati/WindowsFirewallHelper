namespace WindowsFirewallHelper
{
    public enum FirewallRuleCategory
    {
        Boot = NET_FW_RULE_CATEGORY.NET_FW_RULE_CATEGORY_BOOT,
        Stealth = NET_FW_RULE_CATEGORY.NET_FW_RULE_CATEGORY_STEALTH,
        Firewall = NET_FW_RULE_CATEGORY.NET_FW_RULE_CATEGORY_FIREWALL,
        ConnectionSecurity = NET_FW_RULE_CATEGORY.NET_FW_RULE_CATEGORY_CONSEC
    }
}