using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Exceptions;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper.FirewallRules
{
    /// <inheritdoc cref="IFirewallRule" />
    /// <summary>
    ///     Contains properties of a Windows Firewall v1 application rule
    /// </summary>
    public class FirewallLegacyApplicationRule : IFirewallRule, IEquatable<FirewallLegacyApplicationRule>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall v1
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="processAddress">Address of the executable file</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public FirewallLegacyApplicationRule(string name, string processAddress, FirewallProfiles profiles)
        {
            if (profiles.HasFlag(FirewallProfiles.Public))
            {
                throw new FirewallLegacyNotSupportedException(
                    "Public profile is not supported when working with Windows Firewall Legacy."
                );
            }

            UnderlyingObjects = new Dictionary<FirewallProfiles, INetFwAuthorizedApplication[]>();

            foreach (var profile in Enum.GetValues(typeof(FirewallProfiles)).OfType<FirewallProfiles>())
            {
                if (profiles.HasFlag(profile))
                {
                    UnderlyingObjects.Add(
                        profile,
                        new[] {ComHelper.CreateInstance<INetFwAuthorizedApplication>()}
                    );
                }
            }

            if (UnderlyingObjects.Count == 0)
            {
                throw new ArgumentException("At least one profile is required.", nameof(profiles));
            }

            Name = name;
            ApplicationName = processAddress;
            IsEnable = true;
            Scope = FirewallScope.All;
            IsEnable = true;
        }

        internal FirewallLegacyApplicationRule(
            Dictionary<FirewallProfiles, INetFwAuthorizedApplication[]> authorizedApplications)
        {
            UnderlyingObjects = authorizedApplications;
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported
        {
            get => ComHelper.IsSupported<INetFwAuthorizedApplication>();
        }

        private Dictionary<FirewallProfiles, INetFwAuthorizedApplication[]> UnderlyingObjects { get; }

        /// <summary>
        ///     Determines whether the specified<see cref="FirewallLegacyApplicationRule" /> is equal to the current
        ///     <see cref="FirewallLegacyApplicationRule" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="FirewallLegacyApplicationRule" /> is equal to the current
        ///     <see cref="FirewallLegacyApplicationRule" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(FirewallLegacyApplicationRule other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Profiles != other.Profiles)
            {
                return false;
            }

            if (!string.Equals(ApplicationName, other.ApplicationName))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public bool Equals(IFirewallRule other)
        {
            return Equals(other as FirewallLegacyApplicationRule);
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        FirewallAction IFirewallRule.Action
        {
            get => FirewallAction.Allow;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <summary>
        ///     Gets or sets the address of the executable file that this rule is about
        /// </summary>
        public string ApplicationName
        {
            get => UnderlyingObjects.Values.SelectMany(a => a).First().ProcessImageFileName;
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values.SelectMany(a => a))
                {
                    authorizedApplication.ProcessImageFileName = value;
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        FirewallDirection IFirewallRule.Direction
        {
            get => FirewallDirection.Inbound;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }


        /// <inheritdoc />
        public bool IsEnable
        {
            get => UnderlyingObjects.Values.SelectMany(a => a).All(port => port.Enabled);
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values.SelectMany(a => a))
                {
                    authorizedApplication.Enabled = value;
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        IAddress[] IFirewallRule.LocalAddresses
        {
            get => new IAddress[] {SingleIP.Any};
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        ushort[] IFirewallRule.LocalPorts
        {
            get => new ushort[0];
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        FirewallPortType IFirewallRule.LocalPortType
        {
            get => FirewallPortType.All;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        public string Name
        {
            get => UnderlyingObjects.Values.SelectMany(a => a).First().Name;
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values.SelectMany(a => a))
                {
                    authorizedApplication.Name = value;
                }
            }
        }

        /// <inheritdoc />
        public FirewallProfiles Profiles
        {
            get
            {
                return UnderlyingObjects.Keys.ToArray()
                    .Aggregate(
                        (FirewallProfiles) 0,
                        (profiles, profile) => profiles | profile
                    );
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        FirewallProtocol IFirewallRule.Protocol
        {
            get => FirewallProtocol.Any;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        public IAddress[] RemoteAddresses
        {
            get => UnderlyingObjects.Values
                .SelectMany(a => a)
                .SelectMany(application => AddressHelper.StringToAddresses(application.RemoteAddresses))
                .Distinct()
                .ToArray();
            set
            {
                foreach (var authorizedApplication in UnderlyingObjects.Values.SelectMany(a => a))
                {
                    authorizedApplication.RemoteAddresses = AddressHelper.AddressesToString(value);
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        ushort[] IFirewallRule.RemotePorts
        {
            get => new ushort[0];
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        public FirewallScope Scope
        {
            get => (FirewallScope) UnderlyingObjects.Values.SelectMany(a => a).First().Scope;
            set
            {
                if (value == FirewallScope.Specific)
                {
                    throw new ArgumentException(
                        "Use the RemoteAddresses property to set the exact remote addresses"
                    );
                }

                if (value == FirewallScope.LocalSubnet)
                {
                    RemoteAddresses = new IAddress[] {new LocalSubnet()};

                    foreach (var authorizedApplication in UnderlyingObjects.Values.SelectMany(a => a))
                    {
                        authorizedApplication.Scope = NetFwScope.LocalSubnet;
                    }
                }
                else if (value == FirewallScope.All)
                {
                    RemoteAddresses = new IAddress[] {SingleIP.Any};

                    foreach (var authorizedApplication in UnderlyingObjects.Values.SelectMany(a => a))
                    {
                        authorizedApplication.Scope = NetFwScope.All;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        string IFirewallRule.ServiceName
        {
            get => string.Empty;
            set => throw new ArgumentException(
                "You can not change the identity of an application rule. Consider creating another rule."
            );
        }

        /// <summary>
        ///     Compares two <see cref="FirewallLegacyApplicationRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallLegacyApplicationRule" /> object</param>
        /// <param name="right">A <see cref="FirewallLegacyApplicationRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallLegacyApplicationRule left, FirewallLegacyApplicationRule right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallLegacyApplicationRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallLegacyApplicationRule" /> object</param>
        /// <param name="right">A <see cref="FirewallLegacyApplicationRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallLegacyApplicationRule left, FirewallLegacyApplicationRule right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return Equals(other as IFirewallRule);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return UnderlyingObjects.Values.Aggregate(0, (hashCode, port) => hashCode + port.GetHashCode());
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        ///     Returns the rule underlying object for an specific profile
        /// </summary>
        /// <param name="profile">The firewall profile to get the underlying COM object of this rule for</param>
        /// <returns>The underlying COM object of this rule</returns>
        // ReSharper disable once FlagArgument
        public INetFwAuthorizedApplication[] GetCOMObjects(FirewallProfiles profile)
        {
            if (UnderlyingObjects.ContainsKey(profile))
            {
                return UnderlyingObjects[profile];
            }

            return null;
        }
    }
}