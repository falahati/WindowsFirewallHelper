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
    ///     Contains properties and methods of Windows Firewall v1
    /// </summary>
    public class FirewallLegacy : IFirewall
    {
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

        public static bool IsSupported
        {
            get => ComHelper.IsSupported<INetFwMgr>();
        }

        public ReadOnlyCollection<FirewallLegacyProfile> Profiles { get; }

        internal INetFwMgr UnderlyingObject { get; }

        /// <inheritdoc />
        /// <exception cref="T:System.NotSupportedException">This class is not supported on this machine</exception>
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked setting is not
        ///     supported with this class
        /// </exception>
        // ReSharper disable once TooManyArguments
        IFirewallRule IFirewall.CreateApplicationRule(
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
                throw new FirewallLegacyNotSupportedException();
            }

            if (action != FirewallAction.Allow)
            {
                throw new FirewallLegacyNotSupportedException();
            }

            return new FirewallLegacyApplicationRule(name, filename, profile);
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
        IFirewallRule IFirewall.CreateApplicationRule(
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
        IFirewallRule IFirewall.CreateApplicationRule(FirewallProfiles profile, string name, string filename)
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
        IFirewallRule IFirewall.CreatePortRule(
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
                throw new FirewallLegacyNotSupportedException();
            }

            return new FirewallLegacyPortRule(name, portNumber, profile) {Protocol = protocol};
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
        IFirewallRule IFirewall.CreatePortRule(
            FirewallProfiles profile,
            string name,
            FirewallAction action,
            ushort portNumber)
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
        IFirewallRule IFirewall.CreatePortRule(FirewallProfiles profile, string name, ushort portNumber)
        {
            return ((IFirewall) this).CreatePortRule(profile, name, FirewallAction.Allow, portNumber);
        }

        /// <inheritdoc />
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     The asked profile is not
        ///     supported with this class
        /// </exception>
        IFirewallProfile IFirewall.GetActiveProfile()
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
        IFirewallProfile IFirewall.GetProfile(FirewallProfiles profile)
        {
            return GetProfile(profile);
        }

        /// <inheritdoc />
        public string Name
        {
            get => "Windows Firewall";
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

        public FirewallLegacyProfile GetActiveProfile()
        {
            return Profiles.FirstOrDefault(p => p.IsActive);
        }

        public FirewallLegacyProfile GetProfile(FirewallProfiles profile)
        {
            return Profiles.FirstOrDefault(p => p.Type == profile) ?? throw new FirewallLegacyNotSupportedException();
        }
    }
}