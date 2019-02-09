namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Versions of API
    /// </summary>
    public enum APIVersion
    {
        /// <summary>
        ///     Invalid or unknown version of API
        /// </summary>
        None,

        /// <summary>
        ///     Windows Firewall API v1 (Win XP+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallAPIv1,

        /// <summary>
        ///     Windows Firewall API v2 (Windows Vista+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallAPIv2,

        /// <summary>
        ///     Windows Firewall API v2 (Windows 7+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallAPIv2Win7,

        /// <summary>
        ///     Windows Firewall API v2 (Windows 8+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallAPIv2Win8
    }
}