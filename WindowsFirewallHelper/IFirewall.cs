using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Defines expected methods and properties of a firewall program or API
    /// </summary>
    public interface IFirewall
    {
        /// <summary>
        ///     Gets the name of the firewall
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the list of all available profiles of the firewall
        /// </summary>
        ReadOnlyCollection<IFirewallProfile> Profiles { get; }

        /// <summary>
        ///     Gets the list of all registered rules of the firewall
        /// </summary>
        ICollection<IFirewallRule> Rules { get; }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol
        );

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename
        );

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        IFirewallRule CreateApplicationRule(FirewallProfiles profiles, string name, string filename);


        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to the current firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreateApplicationRule(
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol
        );

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered the current firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreateApplicationRule(
            string name,
            FirewallAction action,
            string filename
        );

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered the current firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        IFirewallRule CreateApplicationRule(string name, string filename);

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol
        );

        /// <summary>
        ///     Creates a rule about a TCP port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber
        );

        /// <summary>
        ///     Creates a rule about a TCP port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        IFirewallRule CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber);


        /// <summary>
        ///     Creates a rule about a port to be registered to the current firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreatePortRule(
            string name,
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol
        );

        /// <summary>
        ///     Creates a rule about a TCP port to be registered to the current firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        // ReSharper disable once TooManyArguments
        IFirewallRule CreatePortRule(
            string name,
            FirewallAction action,
            ushort portNumber
        );

        /// <summary>
        ///     Creates a rule about a TCP port to be registered to the current firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IFirewallRule" /> interface</returns>
        IFirewallRule CreatePortRule(string name, ushort portNumber);

        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object implementing <see cref="IFirewallProfile" /> interface or null if no firewall
        ///     profile is currently active
        /// </returns>
        IFirewallProfile GetActiveProfile();

        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object implementing <see cref="IFirewallProfile" /> interface</returns>
        IFirewallProfile GetProfile(FirewallProfiles profile);
    }
}