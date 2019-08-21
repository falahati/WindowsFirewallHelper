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
    /// <inheritdoc cref="IFirewall" />
    /// <summary>
    ///     Contains properties and methods of Windows Firewall with Advanced Security
    /// </summary>
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
            get => ThreadedSingleton.GetInstance<FirewallWAS>();
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
        public ICollection<FirewallWASRule> Rules
        {
            get => new FirewallWASRulesCollection<FirewallWASRule>(UnderlyingObject.Rules);
        }

        internal INetFwPolicy2 UnderlyingObject { get; }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol)
        {
            if (FirewallWASRuleWin8.IsSupported)
            {
                return new FirewallWASRuleWin8(name, filename, action, FirewallDirection.Inbound, profiles)
                {
                    Protocol = protocol
                };
            }

            if (FirewallWASRuleWin7.IsSupported)
            {
                return new FirewallWASRuleWin7(name, filename, action, FirewallDirection.Inbound, profiles)
                {
                    Protocol = protocol
                };
            }

            if (FirewallWASRule.IsSupported)
            {
                return new FirewallWASRule(name, filename, action, FirewallDirection.Inbound, profiles)
                {
                    Protocol = protocol
                };
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename)
        {
            return (this as IFirewall).CreateApplicationRule(profiles, name, action, filename, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            return (this as IFirewall).CreateApplicationRule(profiles, name, FirewallAction.Allow, filename);
        }


        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol)
        {
            if (!protocol.Equals(FirewallProtocol.TCP) &&
                !protocol.Equals(FirewallProtocol.UDP) &&
                !protocol.Equals(FirewallProtocol.Any))
            {
                throw new FirewallWASInvalidProtocolException(
                    "Invalid protocol selected; rule's protocol should be TCP, UDP or Any (which means both TCP and UDP).");
            }

            if (FirewallWASRuleWin8.IsSupported)
            {
                return new FirewallWASRuleWin8(name, portNumber, action, FirewallDirection.Inbound,
                        profiles)
                    {Protocol = protocol};
            }

            if (FirewallWASRuleWin7.IsSupported)
            {
                return new FirewallWASRuleWin7(name, portNumber, action, FirewallDirection.Inbound,
                        profiles)
                    {Protocol = protocol};
            }

            if (FirewallWASRule.IsSupported)
            {
                return new FirewallWASRule(name, portNumber, action, FirewallDirection.Inbound,
                        profiles)
                    {Protocol = protocol};
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber)
        {
            return (this as IFirewall).CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        IFirewallRule IFirewall.CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return (this as IFirewall).CreatePortRule(profiles, name, FirewallAction.Allow, portNumber);
        }

        /// <inheritdoc />
        IFirewallProfile IFirewall.GetActiveProfile()
        {
            return GetActiveProfile();
        }

        /// <inheritdoc />
        // ReSharper disable once FlagArgument
        IFirewallProfile IFirewall.GetProfile(FirewallProfiles profile)
        {
            return GetProfile(profile);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the name of the firewall
        /// </summary>
        public string Name
        {
            get => "Windows Firewall with Advanced Security";
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the list of all available profiles of the firewall
        /// </summary>
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