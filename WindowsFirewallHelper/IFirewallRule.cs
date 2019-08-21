using System;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Defines expected properties of a firewall rule
    /// </summary>
    public interface IFirewallRule : IEquatable<IFirewallRule>
    {
        /// <summary>
        ///     Gets or sets the action that the rules defines
        /// </summary>
        FirewallAction Action { get; set; }

        /// <summary>
        ///     Gets or sets the name of the application that this rule is about
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        ///     Gets or sets the data direction that the rule applies to
        /// </summary>
        FirewallDirection Direction { get; set; }

        /// <summary>
        ///     Gets or sets the resolved name of the rule
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        ///     Gets or sets a Boolean value indicating if this rule is active
        /// </summary>
        bool IsEnable { get; set; }

        /// <summary>
        ///     Gets or sets the local addresses that the rule applies to
        /// </summary>
        IAddress[] LocalAddresses { get; set; }

        /// <summary>
        ///     Gets or sets the local ports that the rule applies to
        /// </summary>
        ushort[] LocalPorts { get; set; }

        /// <summary>
        ///     Gets or sets the type of local ports that the rules applies to
        /// </summary>
        FirewallPortType LocalPortType { get; set; }

        /// <summary>
        ///     Gets or sets the name of the rule in native format w/o auto string resolving
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets the profiles that this rule belongs to
        /// </summary>
        FirewallProfiles Profiles { get; }

        /// <summary>
        ///     Gets or sets the protocol that the rule applies to
        /// </summary>
        FirewallProtocol Protocol { get; set; }

        /// <summary>
        ///     Gets or sets the remote addresses that the rule applies to
        /// </summary>
        IAddress[] RemoteAddresses { get; set; }

        /// <summary>
        ///     Gets or sets the remote ports that the rule applies to
        /// </summary>
        ushort[] RemotePorts { get; set; }

        /// <summary>
        ///     Gets or sets the scope that the rule applies to
        /// </summary>
        FirewallScope Scope { get; set; }

        /// <summary>
        ///     Gets or sets the name of the service that this rule is about
        /// </summary>
        string ServiceName { get; set; }
    }
}