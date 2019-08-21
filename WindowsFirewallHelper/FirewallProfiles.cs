using System;
using WindowsFirewallHelper.COMInterop;

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
        Domain = NetFwProfileType2.Domain,

        /// <summary>
        ///     Private Profile
        /// </summary>
        Private = NetFwProfileType2.Private,

        /// <summary>
        ///     Public Profile
        /// </summary>
        Public = NetFwProfileType2.Public
    }
}