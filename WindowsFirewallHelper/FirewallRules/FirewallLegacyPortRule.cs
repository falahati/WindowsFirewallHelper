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
    ///     Contains properties of a Windows Firewall v1 port rule
    /// </summary>
    public class FirewallLegacyPortRule : IFirewallRule, IEquatable<FirewallLegacyPortRule>
    {
        /// <summary>
        ///     Creates a new port rule for Windows Firewall v1
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="profiles">The profiles that this rule belongs to</param>
        public FirewallLegacyPortRule(string name, ushort port, FirewallProfiles profiles)
        {
            if (profiles.HasFlag(FirewallProfiles.Public))
            {
                throw new FirewallLegacyNotSupportedException("Public profile is not supported.");
            }

            foreach (var profile in Enum.GetValues(typeof(FirewallProfiles)).OfType<FirewallProfiles>())
            {
                if (profiles.HasFlag(profile))
                {
                    UnderlyingObjects.Add(
                        profile,
                        ComHelper.CreateInstance<INetFwOpenPort>()
                    );
                }
            }

            if (UnderlyingObjects.Count == 0)
            {
                throw new ArgumentException("At least one profile is required.", nameof(profiles));
            }

            Name = name;
            LocalPort = port;
            IsEnable = true;
            Scope = FirewallScope.All;
            IsEnable = true;
        }

        internal FirewallLegacyPortRule(Dictionary<FirewallProfiles, INetFwOpenPort> openPorts)
        {
            UnderlyingObjects = openPorts;
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported
        {
            get => ComHelper.IsSupported<INetFwOpenPort>();
        }

        public ushort LocalPort
        {
            get => (ushort) UnderlyingObjects.Values.First().Port;
            set
            {
                foreach (var openPort in UnderlyingObjects.Values)
                {
                    openPort.Port = value;
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
                        (profiles, profile) => profiles & profile
                    );
            }
        }

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        private Dictionary<FirewallProfiles, INetFwOpenPort> UnderlyingObjects { get; }

        /// <summary>
        ///     Determines whether the specified<see cref="FirewallLegacyPortRule" /> is equal to the current
        ///     <see cref="FirewallLegacyPortRule" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="FirewallLegacyPortRule" /> is equal to the current
        ///     <see cref="FirewallLegacyPortRule" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(FirewallLegacyPortRule other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (UnderlyingObjects.Count != other.UnderlyingObjects.Count)
            {
                return false;
            }

            if (!UnderlyingObjects.Keys.SequenceEqual(other.UnderlyingObjects.Keys))
            {
                return false;
            }

            foreach (var profile in UnderlyingObjects.Keys)
            {
                var thisPort = UnderlyingObjects[profile];
                var otherPort = other.UnderlyingObjects[profile];

                if (thisPort.Port != otherPort.Port || thisPort.Protocol != otherPort.Protocol)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(IFirewallRule other)
        {
            return Equals(other as FirewallLegacyPortRule);
        }


        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported</exception>
        FirewallAction IFirewallRule.Action
        {
            get => FirewallAction.Allow;
            set => throw new FirewallLegacyNotSupportedException();
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
            get => UnderlyingObjects.Values.All(port => port.Enabled);
            set
            {
                foreach (var openPort in UnderlyingObjects.Values)
                {
                    openPort.Enabled = value;
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
        /// <exception cref="FirewallLegacyNotSupportedException">
        ///     The array passes to this property as value should have one and
        ///     only one element
        /// </exception>
        ushort[] IFirewallRule.LocalPorts
        {
            get => UnderlyingObjects.Values.Select(port => (ushort) port.Port).Distinct().ToArray();
            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException(
                        "You can not change the identity of a port rule. Consider creating another rule.");
                }

                if (value.Length > 1)
                {
                    throw new FirewallLegacyNotSupportedException(
                        "This property only accept an array of one element length.");
                }

                LocalPort = value[0];
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported.</exception>
        FirewallPortType IFirewallRule.LocalPortType
        {
            get => FirewallPortType.Specific;
            set => throw new FirewallLegacyNotSupportedException("Setting a value for this property is not supported.");
        }

        /// <inheritdoc />
        public string Name
        {
            get => UnderlyingObjects.Values.First().Name;
            set
            {
                foreach (var openPort in UnderlyingObjects.Values)
                {
                    openPort.Name = value;
                }
            }
        }


        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Only acceptable values are UDP, TCP and Any</exception>
        public FirewallProtocol Protocol
        {
            get => new FirewallProtocol((int) UnderlyingObjects.Values.First().Protocol);
            set
            {
                if (!value.Equals(FirewallProtocol.Any) &&
                    !value.Equals(FirewallProtocol.TCP) &&
                    !value.Equals(FirewallProtocol.UDP))
                {
                    throw new NotSupportedException("Acceptable values are UDP, TCP and Any.");
                }

                foreach (var openPort in UnderlyingObjects.Values)
                {
                    openPort.Protocol = (NET_FW_IP_PROTOCOL) value.ProtocolNumber;
                }
            }
        }


        /// <inheritdoc />
        public IAddress[] RemoteAddresses
        {
            get => UnderlyingObjects.Values
                .SelectMany(application => AddressHelper.StringToAddresses(application.RemoteAddresses))
                .Distinct()
                .ToArray();
            set
            {
                foreach (var openPort in UnderlyingObjects.Values)
                {
                    openPort.RemoteAddresses = AddressHelper.AddressesToString(value);
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
            get => (FirewallScope) UnderlyingObjects.Values.First().Scope;
            set
            {
                if (value == FirewallScope.Specific)
                {
                    throw new ArgumentException("Use the RemoteAddresses property to set the exact remote addresses.");
                }

                if (value == FirewallScope.LocalSubnet)
                {
                    RemoteAddresses = new IAddress[] {new LocalSubnet()};

                    foreach (var openPort in UnderlyingObjects.Values)
                    {
                        openPort.Scope = NET_FW_SCOPE.NET_FW_SCOPE_LOCAL_SUBNET;
                    }
                }
                else if (value == FirewallScope.All)
                {
                    RemoteAddresses = new IAddress[] {SingleIP.Any};

                    foreach (var openPort in UnderlyingObjects.Values)
                    {
                        openPort.Scope = NET_FW_SCOPE.NET_FW_SCOPE_ALL;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Compares two <see cref="FirewallLegacyPortRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallLegacyPortRule" /> object</param>
        /// <param name="right">A <see cref="FirewallLegacyPortRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallLegacyPortRule left, FirewallLegacyPortRule right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallLegacyPortRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallLegacyPortRule" /> object</param>
        /// <param name="right">A <see cref="FirewallLegacyPortRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallLegacyPortRule left, FirewallLegacyPortRule right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as IFirewallRule);
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

        // ReSharper disable once FlagArgument
        public INetFwOpenPort GetCOMObject(FirewallProfiles profile)
        {
            if (UnderlyingObjects.ContainsKey(profile))
            {
                return UnderlyingObjects[profile];
            }

            return null;
        }
    }
}