namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Defines expected properties of a firewall profile
    /// </summary>
    public interface IFirewallProfile
    {
        /// <summary>
        ///     Gets or sets a Boolean value that blocks all inbound traffic completely regardless of any rules in this profile
        /// </summary>
        bool BlockAllInboundTraffic { get; set; }

        /// <summary>
        ///     Gets or sets the global default behavior regarding inbound traffic
        /// </summary>
        FirewallAction DefaultInboundAction { get; set; }

        /// <summary>
        ///     Gets or sets the global default behavior regarding outbound traffic
        /// </summary>
        FirewallAction DefaultOutboundAction { get; set; }

        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is enable and available
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is the currently active profile.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        ///     Gets or sets a value indicating if the user should get notifications about rules of this profile
        /// </summary>
        bool ShowNotifications { get; set; }

        /// <summary>
        ///     Gets a FirewallProfiles showing the type of this firewall profile
        /// </summary>
        FirewallProfiles Type { get; }

        /// <summary>
        ///     Gets or sets a value indicating if the firewall should send unicast responses to the multicast broadcasts
        /// </summary>
        bool UnicastResponsesToMulticastBroadcast { get; set; }
    }
}