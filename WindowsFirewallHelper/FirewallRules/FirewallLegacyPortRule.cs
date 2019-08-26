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
                throw new FirewallLegacyNotSupportedException(
                    "Public profile is not supported when working with Windows Firewall Legacy."
                );
            }

            UnderlyingObjects = new Dictionary<FirewallProfiles, INetFwOpenPort[]>();

            foreach (var profile in Enum.GetValues(typeof(FirewallProfiles)).OfType<FirewallProfiles>())
            {
                if (profiles.HasFlag(profile))
                {
                    UnderlyingObjects.Add(
                        profile,
                        new[] {ComHelper.CreateInstance<INetFwOpenPort>()}
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

        internal FirewallLegacyPortRule(Dictionary<FirewallProfiles, INetFwOpenPort[]> openPorts)
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

        /// <summary>
        ///     Gets or sets the local port that the rule applies to
        /// </summary>
        public ushort LocalPort
        {
            get => (ushort) UnderlyingObjects.Values.SelectMany(p => p).First().Port;
            set
            {
                foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
                {
                    openPort.Port = value;
                }
            }
        }

        private Dictionary<FirewallProfiles, INetFwOpenPort[]> UnderlyingObjects { get; }


        /// <inheritdoc />
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

            if (Profiles != other.Profiles)
            {
                return false;
            }

            if (LocalPort != other.LocalPort || Protocol != other.Protocol)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
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
        string IFirewallRule.ApplicationName
        {
            get => string.Empty;
            set => throw new ArgumentException(
                "You can not change the identity of a port rule. Consider creating another rule."
            );
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
            get => UnderlyingObjects.Values.SelectMany(p => p).All(port => port.Enabled);
            set
            {
                foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
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
            get => UnderlyingObjects.Values.SelectMany(p => p).Select(port => (ushort) port.Port).Distinct().ToArray();
            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException(
                        "You can not change the identity of a port rule. Consider creating another rule."
                    );
                }

                if (value.Length > 1)
                {
                    throw new FirewallLegacyNotSupportedException(
                        "This property only accept an array of one element length."
                    );
                }

                LocalPort = value[0];
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallLegacyNotSupportedException">Setting a value for this property is not supported.</exception>
        FirewallPortType IFirewallRule.LocalPortType
        {
            get => FirewallPortType.Specific;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        public string Name
        {
            get => UnderlyingObjects.Values.SelectMany(p => p).First().Name;
            set
            {
                foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
                {
                    openPort.Name = value;
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
        /// <exception cref="FirewallLegacyNotSupportedException">
        ///     Acceptable protocols for Windows Firewall Legacy are UDP, TCP and
        ///     Any.
        /// </exception>
        public FirewallProtocol Protocol
        {
            get => new FirewallProtocol((int) UnderlyingObjects.Values.SelectMany(p => p).First().Protocol);
            set
            {
                if (!value.Equals(FirewallProtocol.Any) &&
                    !value.Equals(FirewallProtocol.TCP) &&
                    !value.Equals(FirewallProtocol.UDP))
                {
                    throw new FirewallLegacyNotSupportedException(
                        "Acceptable protocols for Windows Firewall Legacy are UDP, TCP and Any."
                    );
                }

                if (value.Equals(FirewallProtocol.Any) && FirewallWAS.IsSupported)
                {
                    throw new FirewallLegacyNotSupportedException(
                        "`Any` protocol is not available with Windows Firewall Legacy in compatibility mode."
                    );
                }

                foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
                {
                    openPort.Protocol = (NetFwIPProtocol) value.ProtocolNumber;
                }
            }
        }

        /// <inheritdoc />
        public IAddress[] RemoteAddresses
        {
            get => UnderlyingObjects.Values
                .SelectMany(p => p)
                .SelectMany(application => AddressHelper.StringToAddresses(application.RemoteAddresses))
                .Distinct()
                .ToArray();
            set
            {
                foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
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
            get => (FirewallScope) UnderlyingObjects.Values.SelectMany(p => p).First().Scope;
            set
            {
                if (value == FirewallScope.Specific)
                {
                    throw new ArgumentException(
                        "Use the RemoteAddresses property to set the exact remote addresses."
                    );
                }

                if (value == FirewallScope.LocalSubnet)
                {
                    RemoteAddresses = new IAddress[] {new LocalSubnet()};

                    foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
                    {
                        openPort.Scope = NetFwScope.LocalSubnet;
                    }
                }
                else if (value == FirewallScope.All)
                {
                    RemoteAddresses = new IAddress[] {SingleIP.Any};

                    foreach (var openPort in UnderlyingObjects.Values.SelectMany(p => p))
                    {
                        openPort.Scope = NetFwScope.All;
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
                "You can not change the identity of a port rule. Consider creating another rule."
            );
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

        /// <summary>
        ///     Returns the rule underlying object for an specific profile
        /// </summary>
        /// <param name="profile">The firewall profile to get the underlying COM object of this rule for</param>
        /// <returns>The underlying COM object of this rule</returns>
        // ReSharper disable once FlagArgument
        public INetFwOpenPort[] GetCOMObjects(FirewallProfiles profile)
        {
            if (UnderlyingObjects.ContainsKey(profile))
            {
                return UnderlyingObjects[profile];
            }

            return null;
        }
    }
}