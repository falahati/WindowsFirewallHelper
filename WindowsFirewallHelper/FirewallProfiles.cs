using System;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Firewall rule profile
    /// </summary>
    [Flags]
    public enum FirewallProfiles
    {
        /// <summary>
        ///     Domain Profile
        /// </summary>
        Domain = 1,

        /// <summary>
        ///     Private Profile
        /// </summary>
        Private = 2,

        /// <summary>
        ///     Public Profile
        /// </summary>
        Public = 4
    }
}