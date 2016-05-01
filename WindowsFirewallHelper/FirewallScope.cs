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
        All,

        /// <summary>
        ///     Local subnet only
        /// </summary>
        LocalSubnet,

        /// <summary>
        ///     Specific list of addresses
        /// </summary>
        Specific
    }
}