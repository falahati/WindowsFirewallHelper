using System;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <summary>
    ///     Type of the network interface
    /// </summary>
    [Flags]
    public enum FirewallInterfaceTypes
    {
        /// <summary>
        ///     Remove access
        /// </summary>
        RemoteAccess = 1,

        /// <summary>
        ///     Wireless (Wi-Fi)
        /// </summary>
        Wireless = 2,

        /// <summary>
        ///     Local Area Network (Ethernet)
        /// </summary>
        Lan = 4
    }
}