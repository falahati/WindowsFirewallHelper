using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Exceptions;
using WindowsFirewallHelper.FirewallRules;
using WindowsFirewallHelper.InternalCollections;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Contains properties and methods of Windows Firewall v1
    /// </summary>
    public class FirewallLegacy : IFirewall
    {
        /// <summary>
        ///     Creates a new instance of this class on the current thread and leaves the cross thread control to the user of the
        ///     class.
        /// </summary>
        public FirewallLegacy()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException("This type is not supported in this environment.");
            }

            UnderlyingObject = ComHelper.CreateInstance<INetFwMgr>();

            Profiles = new ReadOnlyCollection<FirewallLegacyProfile>(new[]
            {
                new FirewallLegacyProfile(this, FirewallProfiles.Domain),
                new FirewallLegacyProfile(this, FirewallProfiles.Private)
            });
        }

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static FirewallLegacy Instance
        {
            get => ThreadedSingleton.GetInstance<FirewallLegacy>();
        }

        /// <summary>
        ///     Gets a Boolean value showing if the firewall is supported in this environment.
        /// </summary>
        public static bool IsSupported
        {
            get => ComHelper.IsSupported<INetFwMgr>();
        }

        /// <summary>
        ///     Gets the list of all available profiles of the firewall
        /// </summary>
        public ReadOnlyCollection<FirewallLegacyProfile> Profiles { get; }

        internal INetFwMgr UnderlyingObject { get; }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            // ReSharper disable once FlagArgument
            FirewallAction action,
            string filename,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            if (!protocol.Equals(FirewallProtocol.Any))
            {
                throw new FirewallLegacyNotSupportedException(
                    "Application rules are only supported along with the `FirewallProtocol.Any` protocol in Windows Firewall Legacy.");
            }

            if (action != FirewallAction.Allow)
            {
                throw new FirewallLegacyNotSupportedException(
                    "Windows Firewall Legacy only accepts allow exception rules.");
            }

            return CreateApplicationRule(profiles, name, filename);
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            FirewallProfiles profile,
            string name,
            FirewallAction action,
            string filename)
        {
            return ((IFirewall) this).CreateApplicationRule(profile, name, action, filename, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        public IFirewallRule CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            return new FirewallLegacyApplicationRule(name, filename, profiles);
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            // ReSharper disable once FlagArgument
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            if (action != FirewallAction.Allow)
            {
                throw new FirewallLegacyNotSupportedException("Windows Firewall Legacy only accepts allow exception rules.");
            }

            return new FirewallLegacyPortRule(name, portNumber, profiles) {Protocol = protocol};
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber)
        {
            return ((IFirewall) this).CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.TCP);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return ((IFirewall)this).CreatePortRule(profiles, name, FirewallAction.Allow, portNumber, FirewallProtocol.TCP);
        }

        /// <inheritdoc />
        IFirewallProfile IFirewall.GetActiveProfile()
        {
            return GetActiveProfile();
        }

        /// <inheritdoc />
        IFirewallProfile IFirewall.GetProfile(FirewallProfiles profile)
        {
            return GetProfile(profile);
        }

        /// <inheritdoc />
        public string Name
        {
            get => "Windows Firewall Legacy";
        }

        /// <inheritdoc />
        ReadOnlyCollection<IFirewallProfile> IFirewall.Profiles
        {
            get => new ReadOnlyCollection<IFirewallProfile>(Profiles.Cast<IFirewallProfile>().ToArray());
        }

        /// <inheritdoc />
        public ICollection<IFirewallRule> Rules
        {
            get => new FirewallLegacyRulesCollection(Profiles.ToArray());
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile regardless of its protocol (TCP or UDP)
        /// </summary>
        /// <param name="profiles">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallLegacyPortRule" /> instance</returns>
        public FirewallLegacyPortRule CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            return new FirewallLegacyPortRule(name, portNumber, profiles) {Protocol = FirewallProtocol.Any};
        }

        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object or null if no firewall profile is currently active
        /// </returns>
        public FirewallLegacyProfile GetActiveProfile()
        {
            return Profiles.FirstOrDefault(p => p.IsActive);
        }

        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object</returns>
        public FirewallLegacyProfile GetProfile(FirewallProfiles profile)
        {
            return Profiles.FirstOrDefault(p => p.Type == profile) ?? throw new FirewallLegacyNotSupportedException();
        }
    }
}