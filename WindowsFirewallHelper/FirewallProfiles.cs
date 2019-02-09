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
        Domain = NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_DOMAIN,

        /// <summary>
        ///     Private Profile
        /// </summary>
        Private = NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PRIVATE,

        /// <summary>
        ///     Public Profile
        /// </summary>
        Public = NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PUBLIC
    }
}