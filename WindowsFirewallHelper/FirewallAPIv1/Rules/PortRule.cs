using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1.Rules
{
    /// <inheritdoc cref="IRule" />
    /// <summary>
    ///     Contains properties of a Windows Firewall v1 port rule
    /// </summary>
    public class PortRule : IRule, IEquatable<PortRule>
    {
        /// <summary>
        ///     Creates a new port rule for Windows Firewall v1
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="profiles">The profiles that this rule belongs to</param>
        public PortRule(string name, ushort port, FirewallProfiles profiles)
        {
            if (EnumHelper.HasFlag(profiles, FirewallProfiles.Public))
            {
                throw new FirewallAPIv1NotSupportedException("Public profile is not supported.");
            }

            foreach (var profile in Enum.GetValues(typeof(FirewallProfiles)).OfType<FirewallProfiles>())
            {
                if (profiles.HasFlag(profile))
                {
                    UnderlyingObjects.Add(
                        profile,
                        COMHelper.CreateInstance<INetFwOpenPort>()
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

        internal PortRule(Dictionary<FirewallProfiles, INetFwOpenPort> openPorts)
        {
            UnderlyingObjects = openPorts;
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported
        {
            get => COMHelper.IsSupported<INetFwOpenPort>();
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

        public bool Equals(IRule other)
        {
            return Equals(other as PortRule);
        }

        /// <summary>
        ///     Determines whether the specified<see cref="PortRule" /> is equal to the current
        ///     <see cref="PortRule" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="PortRule" /> is equal to the current<see cref="PortRule" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(PortRule other)
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


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        FirewallAction IRule.Action
        {
            get => FirewallAction.Allow;
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        FirewallDirection IRule.Direction
        {
            get => FirewallDirection.Inbound;
            set => throw new FirewallAPIv1NotSupportedException();
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
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        IAddress[] IRule.LocalAddresses
        {
            get => new IAddress[] {SingleIP.Any};
            set => throw new FirewallAPIv1NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">
        ///     The array passes to this property as value should have one and
        ///     only one element
        /// </exception>
        ushort[] IRule.LocalPorts
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
                    throw new FirewallAPIv1NotSupportedException(
                        "This property only accept an array of one element length.");
                }

                LocalPort = value[0];
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported.</exception>
        FirewallPortType IRule.LocalPortType
        {
            get => FirewallPortType.Specific;
            set => throw new FirewallAPIv1NotSupportedException("Setting a value for this property is not supported.");
        }

        /// <inheritdoc />
        public string Name
        {
            get => NativeHelper.ResolveStringResource(UnderlyingObjects.Values.First().Name);
            set
            {
                foreach (var openPort in UnderlyingObjects.Values)
                {
                    openPort.Name = value;
                }
            }
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Changing the profile of a rule is not supported in FirewallAPIv1.</exception>
        FirewallProfiles IRule.Profiles
        {
            get => Profiles;
            set => throw new FirewallAPIv1NotSupportedException(
                "Changing the profile of a rule is not supported in FirewallAPIv1. Consider creating another rule."
            );
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
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        ushort[] IRule.RemotePorts
        {
            get => new ushort[0];
            set => throw new FirewallAPIv1NotSupportedException();
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
        ///     Compares two <see cref="PortRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="PortRule" /> object</param>
        /// <param name="right">A <see cref="PortRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(PortRule left, PortRule right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="PortRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="PortRule" /> object</param>
        /// <param name="right">A <see cref="PortRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(PortRule left, PortRule right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as IRule);
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