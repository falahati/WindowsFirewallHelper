using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.FirewallAPIv1.Rules;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    /// <inheritdoc cref="IFirewall" />
    /// <summary>
    ///     Contains properties and methods of Windows Firewall v1
    /// </summary>
    public class Firewall : ThreadedSingleton<Firewall>, IFirewall
    {
        public Firewall()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            UnderlyingObject = (INetFwMgr) Activator.CreateInstance(
                Type.GetTypeFromProgID(@"HNetCfg.FwMgr", false)
            );

            Profiles = new[]
            {
                new FirewallProfile(this, NET_FW_PROFILE_TYPE.NET_FW_PROFILE_DOMAIN),
                new FirewallProfile(this, NET_FW_PROFILE_TYPE.NET_FW_PROFILE_STANDARD)
            };
        }

        /// <summary>
        ///     Gets the current singleton instance of this class
        /// </summary>
        public static Firewall Instance
        {
            get => GetInstance();
        }

        public static bool IsSupported
        {
            get => Type.GetTypeFromProgID(@"HNetCfg.FwMgr", false) == null;
        }

        public static bool IsServiceRunning
        {
            get => new ServiceController("SharedAccess").Status == ServiceControllerStatus.Running;
        }

        public FirewallProfile[] Profiles { get; }

        public FirewallRulesCollection Rules
        {
            get => new FirewallRulesCollection(Profiles);
        }

        internal INetFwMgr UnderlyingObject { get; }

        /// <inheritdoc />
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreateApplicationRule(
            FirewallProfiles profile,
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

            if (!protocol.Equals(FirewallProtocol.FirewallV1_TCP_UDP))
            {
                throw new FirewallAPIv1NotSupportedException();
            }

            if (action != FirewallAction.Allow)
            {
                throw new FirewallAPIv1NotSupportedException();
            }

            return new ApplicationRule(name, filename, profile);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="T:WindowsFirewallHelper.IRule" /> interface</returns>
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreateApplicationRule(
            FirewallProfiles profile,
            string name,
            FirewallAction action,
            string filename)
        {
            return ((IFirewall) this).CreateApplicationRule(profile, name, action, filename, FirewallProtocol.FirewallV1_TCP_UDP);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about an executable file (application) to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="T:WindowsFirewallHelper.IRule" /> interface</returns>
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        IRule IFirewall.CreateApplicationRule(FirewallProfiles profile, string name, string filename)
        {
            return ((IFirewall) this).CreateApplicationRule(profile, name, FirewallAction.Allow, filename);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <param name="protocol">Protocol that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="T:WindowsFirewallHelper.IRule" /> interface</returns>
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreatePortRule(
            FirewallProfiles profile,
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
                throw new FirewallAPIv1NotSupportedException();
            }

            return new PortRule(name, portNumber, profile) {Protocol = protocol};
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="T:WindowsFirewallHelper.IRule" /> interface</returns>
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        // ReSharper disable once TooManyArguments
        IRule IFirewall.CreatePortRule(FirewallProfiles profile, string name, FirewallAction action, ushort portNumber)
        {
            return ((IFirewall) this).CreatePortRule(profile, name, action, portNumber, FirewallProtocol.FirewallV1_TCP_UDP);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a rule about a port to be registered to a firewall profile
        /// </summary>
        /// <param name="profile">The profile that the rule belongs to</param>
        /// <param name="name">Name of the rule</param>
        /// <param name="portNumber">Port number that the rule applies to</param>
        /// <returns>Returns the newly created rule object implementing <see cref="T:WindowsFirewallHelper.IRule" /> interface</returns>
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        IRule IFirewall.CreatePortRule(FirewallProfiles profile, string name, ushort portNumber)
        {
            return ((IFirewall) this).CreatePortRule(profile, name, FirewallAction.Allow, portNumber);
        }

        /// <inheritdoc />
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked profile is not
        ///     supported with this class
        /// </exception>
        // ReSharper disable once FlagArgument
        IProfile IFirewall.GetProfile(FirewallProfiles profile)
        {
            return GetProfile(profile);
        }

        /// <inheritdoc />
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked profile is not
        ///     supported with this class
        /// </exception>
        IProfile IFirewall.GetProfile()
        {
            return GetProfile();
        }

        /// <inheritdoc />
        public string Name
        {
            get => "Windows Firewall";
        }

        /// <inheritdoc />
        IProfile[] IFirewall.Profiles
        {
            get => Profiles.Cast<IProfile>().ToArray();
        }

        /// <inheritdoc />
        ICollection<IRule> IFirewall.Rules
        {
            get => Rules;
        }

        public FirewallProfile GetProfile(FirewallProfiles profile)
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            return Profiles.FirstOrDefault(p => p.Type == profile) ?? throw new FirewallAPIv1NotSupportedException();
        }

        public FirewallProfile GetProfile()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException();
            }

            return Profiles.FirstOrDefault(p => p.IsActive);
        }
    }
}