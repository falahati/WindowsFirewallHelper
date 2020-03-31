using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WindowsFirewallHelper.Collections;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Exceptions;
using WindowsFirewallHelper.FirewallRules;
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
            get => ThreadSafeSingleton.GetInstance<FirewallLegacy>();
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

        /// <summary>
        ///     Gets the list of all registered rules of the firewall
        /// </summary>
        public IFirewallLegacyRulesCollection Rules
        {
            get => new FirewallLegacyRulesCollection(Profiles.ToArray());
        }

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
        IFirewallRule IFirewall.CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            return CreateApplicationRule(profiles, name, filename);
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol
        )
        {
            var activeProfile = GetActiveProfile();

            if (activeProfile == null)
            {
                throw new InvalidOperationException("No firewall profile is currently active.");
            }

            return ((IFirewall) this).CreateApplicationRule(
                activeProfile.Type,
                name,
                action,
                filename,
                FirewallProtocol.Any
            );
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreateApplicationRule(string name, FirewallAction action, string filename)
        {
            return ((IFirewall) this).CreateApplicationRule(name, action, filename, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreateApplicationRule(string name, string filename)
        {
            return CreateApplicationRule(name, filename);
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            // ReSharper disable once FlagArgument
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol
        )
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            if (action != FirewallAction.Allow)
            {
                throw new FirewallLegacyNotSupportedException(
                    "Windows Firewall Legacy only accepts allow exception rules.");
            }

            return new FirewallLegacyPortRule(name, portNumber, profiles) {Protocol = protocol};
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber
        )
        {
            return ((IFirewall) this).CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.TCP);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return CreatePortRule(profiles, name, portNumber);
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            string name,
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol
        )
        {
            var activeProfile = GetActiveProfile();

            if (activeProfile == null)
            {
                throw new InvalidOperationException("No firewall profile is currently active.");
            }

            return ((IFirewall) this).CreatePortRule(
                activeProfile.Type,
                name,
                action,
                portNumber,
                FirewallProtocol.TCP
            );
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreatePortRule(string name, FirewallAction action, ushort portNumber)
        {
            return ((IFirewall) this).CreatePortRule(name, action, portNumber, FirewallProtocol.TCP);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreatePortRule(string name, ushort portNumber)
        {
            return CreatePortRule(name, portNumber);
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
        ICollection<IFirewallRule> IFirewall.Rules
        {
            get => Rules;
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallLegacyApplicationRule" /> instance</returns>
        public FirewallLegacyApplicationRule CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            string filename
        )
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            return new FirewallLegacyApplicationRule(name, filename, profiles);
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to the currently active firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallLegacyApplicationRule" /> instance</returns>
        public FirewallLegacyApplicationRule CreateApplicationRule(
            string name,
            string filename
        )
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            var activeProfile = GetActiveProfile();

            if (activeProfile == null)
            {
                throw new InvalidOperationException("No firewall profile is currently active.");
            }

            return new FirewallLegacyApplicationRule(name, filename, activeProfile.Type);
        }

        /// <summary>
        ///     Creates a rule about a TCP port to be registered to a firewall profile regardless of its protocol
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

            return new FirewallLegacyPortRule(name, portNumber, profiles) {Protocol = FirewallProtocol.TCP};
        }

        /// <summary>
        ///     Creates a rule about a TCP port to be registered to the currently active firewall profile regardless of its
        ///     protocol
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallLegacyPortRule" /> instance</returns>
        public FirewallLegacyPortRule CreatePortRule(string name, ushort portNumber)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            var activeProfile = GetActiveProfile();

            if (activeProfile == null)
            {
                throw new InvalidOperationException("No firewall profile is currently active.");
            }

            return new FirewallLegacyPortRule(name, portNumber, activeProfile.Type) {Protocol = FirewallProtocol.TCP};
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