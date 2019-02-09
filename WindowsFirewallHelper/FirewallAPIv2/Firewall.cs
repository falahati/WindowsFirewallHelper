using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            UnderlyingObject = (INetFwPolicy2) Activator.CreateInstance(
                Type.GetTypeFromProgID(@"HNetCfg.FwPolicy2")
            );

            Profiles = new IProfile[]
            {
                new FirewallProfile(this, NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_DOMAIN),
                new FirewallProfile(this, NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PRIVATE),
                new FirewallProfile(this, NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PUBLIC)
            };
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

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static Firewall Instance
        {
            get => GetInstance();
        }

        /// <summary>
        ///     Gets a value indicating if adding or setting a rule or group of rules will take effect in the current firewall
        ///     profile
        /// </summary>
        public FirewallModifyStatePolicy LocalModifyStatePolicy
        {
            get
            {
                if (!IsSupported)
                {
                    throw new NotSupportedException();
                }

                return (FirewallModifyStatePolicy) UnderlyingObject.LocalPolicyModifyState;
            }
        }

        internal INetFwPolicy2 UnderlyingObject { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        public IRule CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

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
        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        public IRule CreateApplicationRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            string filename)
        {
            return CreateApplicationRule(profiles, name, action, filename, FirewallProtocol.Any);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreateApplicationRule(FirewallProfiles profiles, string name, string filename)
        {
            return CreateApplicationRule(profiles, name, FirewallAction.Allow, filename);
        }


        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        public IRule CreatePortRule(
            FirewallProfiles profiles,
            string name,
            FirewallAction action,
            ushort portNumber,
            FirewallProtocol protocol)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            if (!protocol.Equals(FirewallProtocol.TCP) && !protocol.Equals(FirewallProtocol.UDP))
            {
                throw new FirewallAPIv2InvalidProtocolException(
                    "Invalid protocol selected; rule's protocol should be TCP or UDP.");
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
        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        // ReSharper disable once TooManyArguments
        public IRule CreatePortRule(FirewallProfiles profiles, string name, FirewallAction action, ushort portNumber)
        {
            return CreatePortRule(profiles, name, action, portNumber, FirewallProtocol.TCP);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profiles">The profile or profiles that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="IRule" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        public IRule CreatePortRule(FirewallProfiles profiles, string name, ushort portNumber)
        {
            return CreatePortRule(profiles, name, FirewallAction.Allow, portNumber);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns a specific firewall profile
        /// </summary>
        /// <param name="profile">Requested firewall profile</param>
        /// <returns>Firewall profile object implementing <see cref="IProfile" /> interface</returns>
        /// <exception cref="NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="FirewallAPIv2NotSupportedException">The asked profile is not supported with this class</exception>
        // ReSharper disable once FlagArgument
        public IProfile GetProfile(FirewallProfiles profile)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            foreach (var p in Profiles)
            {
                if (p.Type == profile)
                {
                    return p;
                }
            }

            throw new FirewallAPIv2NotSupportedException();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns the active firewall profile, if any
        /// </summary>
        /// <returns>
        ///     The active firewall profile object implementing <see cref="IProfile" /> interface or null if no firewall
        ///     profile is currently active
        /// </returns>
        public IProfile GetProfile()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            foreach (var p in Profiles)
            {
                if (p.IsActive)
                {
                    return p;
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets a Boolean value showing if the firewall is supported in this environment.
        /// </summary>
        public static bool IsSupported
        {
            get => Type.GetTypeFromProgID(@"HNetCfg.FwPolicy2", false) != null;
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
            get
            {
                return new FirewallRulesCollection<IRule>(UnderlyingObject.Rules);
            }
        }

        public ICollection<StandardRule> Rules
        {
            get
            {
                return new FirewallRulesCollection<StandardRule>(UnderlyingObject.Rules);
            }
        }

        /// <summary>
        ///     Restores the local firewall configuration to its default state.
        /// </summary>
        public void ResetDefault()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            UnderlyingObject.RestoreLocalFirewallDefaults();
        }

    }
}