using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                throw new NotSupportedException("This type is not supported in this environment.");
            }

            UnderlyingObject = COMHelper.CreateInstance<INetFwMgr>();

            Profiles = new ReadOnlyCollection<FirewallProfile>(new[]
            {
                new FirewallProfile(this, FirewallProfiles.Domain),
                new FirewallProfile(this, FirewallProfiles.Private)
            });
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
            get => COMHelper.IsSupported<INetFwMgr>();
        }
        
        public ReadOnlyCollection<FirewallProfile> Profiles { get; }
        
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

            if (!protocol.Equals(FirewallProtocol.Any))
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
            return ((IFirewall) this).CreateApplicationRule(profile, name, action, filename,
                FirewallProtocol.Any);
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
            return ((IFirewall) this).CreatePortRule(profile, name, action, portNumber,
                FirewallProtocol.Any);
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
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked profile is not
        ///     supported with this class
        /// </exception>
        IProfile IFirewall.GetActiveProfile()
        {
            return GetActiveProfile();
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
        public string Name
        {
            get => "Windows Firewall";
        }

        /// <inheritdoc />
        ReadOnlyCollection<IProfile> IFirewall.Profiles
        {
            get => new ReadOnlyCollection<IProfile>(Profiles.Cast<IProfile>().ToArray());
        }

        /// <inheritdoc />
        public ICollection<IRule> Rules
        {
            get => new FirewallRulesCollection(Profiles.ToArray());
        }

        public FirewallProfile GetProfile(FirewallProfiles profile)
        {
            return Profiles.FirstOrDefault(p => p.Type == profile) ?? throw new FirewallAPIv1NotSupportedException();
        }

        public FirewallProfile GetActiveProfile()
        {
            return Profiles.FirstOrDefault(p => p.IsActive);
        }
    }
}