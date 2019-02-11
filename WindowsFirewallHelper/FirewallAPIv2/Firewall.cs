using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallAPIv2.Rules;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <inheritdoc cref="IFirewall" />
    /// <summary>
    ///     Contains properties and methods of Windows Firewall with Advanced Security
    /// </summary>
    public class Firewall : ThreadedSingleton<Firewall>, IFirewall
    {
        /// <inheritdoc />
        public Firewall()
        {
            UnderlyingObject = COMHelper.CreateInstance<INetFwPolicy2>();

            Profiles = new IProfile[]
            {
                new FirewallProfile(this, NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_DOMAIN),
                new FirewallProfile(this, NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PRIVATE),
                new FirewallProfile(this, NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PUBLIC)
            };
        }

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static Firewall Instance
        {
            get => GetInstance();
        }

        /// <summary>
        ///     Gets a Boolean value showing if the firewall is supported in this environment.
        /// </summary>
        public static bool IsSupported
        {
            get => COMHelper.IsSupported<INetFwPolicy2>();
        }

        /// <summary>
        ///     Gets a value indicating if adding or setting a rule or group of rules will take effect in the current firewall
        ///     profile
        /// </summary>
        public FirewallModifyStatePolicy LocalModifyStatePolicy
        {
            get => (FirewallModifyStatePolicy) UnderlyingObject.LocalPolicyModifyState;
        }

        public IEnumerable<FirewallRuleGroup> RuleGroups
        {
            get
            {
                return Rules
                    .Select(rule => rule.Grouping)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .Select(s => new FirewallRuleGroup(this, s));
            }
        }

        public ICollection<StandardRule> Rules
        {
            get => new FirewallRulesCollection<StandardRule>(UnderlyingObject.Rules);
        }

        internal INetFwPolicy2 UnderlyingObject { get; }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol)
        {
            if (StandardRuleWin8.IsSupported)
            {
                return new StandardRuleWin8(name, filename, action, FirewallDirection.Inbound, profiles)
                {
                    Protocol = protocol
                };
            }

            if (StandardRuleWin7.IsSupported)
            {
                return new StandardRuleWin7(name, filename, action, FirewallDirection.Inbound, profiles)
                {
                    Protocol = protocol
                };
            }

            if (StandardRule.IsSupported)
            {
                return new StandardRule(name, filename, action, FirewallDirection.Inbound, profiles)
                {
                    Protocol = protocol
                };
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename)
        {
            return (this as IFirewall).CreateApplicationRule(profiles, name, action, filename, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        IRule IFirewall.CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            return (this as IFirewall).CreateApplicationRule(profiles, name, FirewallAction.Allow, filename);
        }


        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreatePortRule(
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
                throw new FirewallAPIv2InvalidProtocolException(
                    "Invalid protocol selected; rule's protocol should be TCP, UDP or Any (which means both TCP and UDP).");
            }

            if (StandardRuleWin8.IsSupported)
            {
                return new StandardRuleWin8(name, portNumber, action, FirewallDirection.Inbound,
                        profiles)
                    {Protocol = protocol};
            }

            if (StandardRuleWin7.IsSupported)
            {
                return new StandardRuleWin7(name, portNumber, action, FirewallDirection.Inbound,
                        profiles)
                    {Protocol = protocol};
            }

            if (StandardRule.IsSupported)
            {
                return new StandardRule(name, portNumber, action, FirewallDirection.Inbound,
                        profiles)
                    {Protocol = protocol};
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreatePortRule(FirewallProfiles profiles, string name, FirewallAction action, ushort portNumber)
        {
            return (this as IFirewall).CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        IRule IFirewall.CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return (this as IFirewall).CreatePortRule(profiles, name, FirewallAction.Allow, portNumber);
        }

        /// <inheritdoc />
        IProfile IFirewall.GetActiveProfile()
        {
            return GetActiveProfile();
        }

        /// <inheritdoc />
        // ReSharper disable once FlagArgument
        IProfile IFirewall.GetProfile(FirewallProfiles profile)
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
        public IProfile[] Profiles { get; }

        /// <inheritdoc />
        ICollection<IRule> IFirewall.Rules
        {
            get => new FirewallRulesCollection<IRule>(UnderlyingObject.Rules);
        }

        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object implementing <see cref="IProfile" /> interface or null if no firewall
        ///     profile is currently active
        /// </returns>
        public FirewallProfile GetActiveProfile()
        {
            return Profiles.FirstOrDefault(p => p.IsActive);
        }

        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object implementing <see cref="IProfile" /> interface</returns>
        /// <exception cref="FirewallAPIv2NotSupportedException">The asked profile is not supported with this class</exception>
        public FirewallProfile GetProfile(FirewallProfiles profile)
        {
            return Profiles.FirstOrDefault(p => p.Type == profile) ?? throw new FirewallAPIv2NotSupportedException();
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