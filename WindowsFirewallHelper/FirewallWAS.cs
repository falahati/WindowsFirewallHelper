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
    ///     Contains properties and methods of Windows Firewall with Advanced Security
    /// </summary>
    // ReSharper disable once ClassTooBig
    public class FirewallWAS : IFirewall
    {
        /// <summary>
        ///     Creates a new instance of this class on the current thread and leaves the cross thread control to the user of the
        ///     class.
        /// </summary>
        public FirewallWAS()
        {
            UnderlyingObject = ComHelper.CreateInstance<INetFwPolicy2>();

            Profiles = new ReadOnlyCollection<FirewallWASProfile>(new[]
            {
                new FirewallWASProfile(this, NetFwProfileType2.Domain),
                new FirewallWASProfile(this, NetFwProfileType2.Private),
                new FirewallWASProfile(this, NetFwProfileType2.Public)
            });
        }

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static FirewallWAS Instance
        {
            get => ThreadSafeSingleton.GetInstance<FirewallWAS>();
        }

        /// <summary>
        ///     Gets a Boolean value showing if the firewall is supported in this environment.
        /// </summary>
        public static bool IsSupported
        {
            get => ComHelper.IsSupported<INetFwPolicy2>();
        }

        /// <summary>
        ///     Gets a value indicating if adding or setting a rule or group of rules will take effect in the current firewall
        ///     profile
        /// </summary>
        public FirewallModifyStatePolicy LocalModifyStatePolicy
        {
            get => (FirewallModifyStatePolicy) UnderlyingObject.LocalPolicyModifyState;
        }

        /// <summary>
        ///     Gets the list of all available profiles of the firewall
        /// </summary>
        public ReadOnlyCollection<FirewallWASProfile> Profiles { get; }

        /// <summary>
        ///     Gets the list of all registered rule groups of the firewall
        /// </summary>
        public IEnumerable<FirewallWASRuleGroup> RuleGroups
        {
            get
            {
                return Rules
                    .Select(rule => rule.Grouping)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .Select(s => new FirewallWASRuleGroup(this, s));
            }
        }

        /// <summary>
        ///     Gets the list of all registered rules of the firewall
        /// </summary>
        public IFirewallWASRulesCollection<FirewallWASRule> Rules
        {
            get => new FirewallWASRulesCollection<FirewallWASRule>(UnderlyingObject.Rules);
        }

        internal INetFwPolicy2 UnderlyingObject { get; }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol
        )
        {
            return CreateApplicationRule(profiles, name, action, FirewallDirection.Inbound, filename, protocol);
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename
        )
        {
            return (this as IFirewall).CreateApplicationRule(profiles, name, action, filename, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            return (this as IFirewall).CreateApplicationRule(profiles, name, FirewallAction.Allow, filename);
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
            return ((IFirewall) this).CreateApplicationRule(name, FirewallAction.Allow, filename);
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol
        )
        {
            return CreatePortRule(profiles, name, action, FirewallDirection.Inbound, portNumber, protocol);
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
            return (this as IFirewall).CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.TCP);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return (this as IFirewall).CreatePortRule(profiles, name, FirewallAction.Allow, portNumber);
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
            return ((IFirewall) this).CreatePortRule(name, FirewallAction.Allow, portNumber);
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
            get => "Windows Firewall with Advanced Security";
        }

        /// <inheritdoc />
        ReadOnlyCollection<IFirewallProfile> IFirewall.Profiles
        {
            get => new ReadOnlyCollection<IFirewallProfile>(Profiles.Cast<IFirewallProfile>().ToArray());
        }

        /// <inheritdoc />
        ICollection<IFirewallRule> IFirewall.Rules
        {
            get => new FirewallWASRulesCollection<IFirewallRule>(UnderlyingObject.Rules);
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="direction">The rule control direction</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallWASRule" /> instance or one of its children</returns>
        // ReSharper disable once TooManyArguments
        public FirewallWASRule CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            FirewallDirection direction,
            string filename,
            FirewallProtocol protocol
        )
        {
            if (!IsSupported)
            {
                throw new FirewallWASNotSupportedException();
            }

            if (FirewallWASRuleWin8.IsSupported)
            {
                return new FirewallWASRuleWin8(name, filename, action, direction, profiles)
                {
                    Protocol = protocol
                };
            }

            if (FirewallWASRuleWin7.IsSupported)
            {
                return new FirewallWASRuleWin7(name, filename, action, direction, profiles)
                {
                    Protocol = protocol
                };
            }

            if (FirewallWASRule.IsSupported)
            {
                return new FirewallWASRule(name, filename, action, direction, profiles)
                {
                    Protocol = protocol
                };
            }

            throw new FirewallWASNotSupportedException();
        }

        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to the currently active firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="direction">The rule control direction</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallWASRule" /> instance or one of its children</returns>
        // ReSharper disable once TooManyArguments
        public FirewallWASRule CreateApplicationRule(
            string name,
            FirewallAction action,
            FirewallDirection direction,
            string filename,
            FirewallProtocol protocol
        )
        {
            if (!IsSupported)
            {
                throw new FirewallWASNotSupportedException();
            }

            var activeProfile = GetActiveProfile();

            if (activeProfile == null)
            {
                throw new InvalidOperationException("No firewall profile is currently active.");
            }

            return CreateApplicationRule(activeProfile.Type, name, action, direction, filename, protocol);
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="direction">The rule control direction</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallWASRule" /> instance or one of its children</returns>
        // ReSharper disable once TooManyArguments
        public FirewallWASRule CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            FirewallDirection direction,
            ushort portNumber,
            FirewallProtocol protocol
        )
        {
            if (!IsSupported)
            {
                throw new FirewallWASNotSupportedException();
            }

            if (!protocol.Equals(FirewallProtocol.TCP) &&
                !protocol.Equals(FirewallProtocol.UDP))
            {
                throw new FirewallWASInvalidProtocolException(
                    "Invalid protocol selected; rule's protocol should be TCP or UDP."
                );
            }

            if (FirewallWASRuleWin8.IsSupported)
            {
                return new FirewallWASRuleWin8(
                    name,
                    portNumber,
                    action,
                    direction,
                    profiles
                )
                {
                    Protocol = protocol
                };
            }

            if (FirewallWASRuleWin7.IsSupported)
            {
                return new FirewallWASRuleWin7(
                    name,
                    portNumber,
                    action,
                    direction,
                    profiles
                )
                {
                    Protocol = protocol
                };
            }

            if (FirewallWASRule.IsSupported)
            {
                return new FirewallWASRule(
                    name,
                    portNumber,
                    action,
                    direction,
                    profiles
                )
                {
                    Protocol = protocol
                };
            }

            throw new FirewallWASNotSupportedException();
        }

        /// <summary>
        ///     Creates a rule about a port to be registered to the currently firewall profile
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="direction">The rule control direction</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created <see cref="FirewallWASRule" /> instance or one of its children</returns>
        // ReSharper disable once TooManyArguments
        public FirewallWASRule CreatePortRule(
            string name,
            FirewallAction action,
            FirewallDirection direction,
            ushort portNumber,
            FirewallProtocol protocol
        )
        {
            if (!IsSupported)
            {
                throw new FirewallWASNotSupportedException();
            }

            var activeProfile = GetActiveProfile();

            if (activeProfile == null)
            {
                throw new InvalidOperationException("No firewall profile is currently active.");
            }

            return CreatePortRule(activeProfile.Type, name, action, direction, portNumber, protocol);
        }

        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object or null if no firewall profile is currently active
        /// </returns>
        public FirewallWASProfile GetActiveProfile()
        {
            return Profiles.FirstOrDefault(p => p.IsActive);
        }

        /// <summary>
        ///     Returns a rule group instance by name provided.
        ///     Any value used for name even if not yet used for a rule will returns a valid instance.
        /// </summary>
        /// <param name="name">The name of the group to be requested</param>
        /// <returns>An instance of <see cref="FirewallWASRuleGroup" /> class</returns>
        public FirewallWASRuleGroup GetGroupByName(string name)
        {
            return new FirewallWASRuleGroup(this, name);
        }

        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object</returns>
        /// <exception cref="FirewallWASNotSupportedException">The asked profile is not supported with this class</exception>
        public FirewallWASProfile GetProfile(FirewallProfiles profile)
        {
            return Profiles.FirstOrDefault(p => p.Type == profile) ?? throw new FirewallWASNotSupportedException();
        }

        /// <summary>
        ///     Restores the local firewall configuration to its default state.
        /// </summary>
        public void ResetDefault()
        {
            UnderlyingObject.RestoreLocalFirewallDefaults();
        }
    }
}