namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Versions of API
    /// </summary>
    public enum FirewallAPIVersion
    {
        /// <summary>
        ///     Invalid or unknown version of API
        /// </summary>
        None,

        /// <summary>
        ///     Windows Firewall Legacy API (Win XP+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallLegacy,

        /// <summary>
        ///     Windows Firewall With Advanced Security API (Windows Vista+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallWAS,

        /// <summary>
        ///     Windows Firewall With Advanced Security API (Windows 7+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallWASWin7,

        /// <summary>
        ///     Windows Firewall With Advanced Security API (Windows 8+)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FirewallWASWin8
    }
}