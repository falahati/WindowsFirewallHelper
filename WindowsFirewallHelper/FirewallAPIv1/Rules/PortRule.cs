using System;
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
        private FirewallProfiles _profiles;

        /// <summary>
        ///     Creates a new port rule for Windows Firewall v1
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="profile">The profile that this rule belongs to</param>
        public PortRule(string name, ushort port, FirewallProfiles profile)
        {
            UnderlyingObject = (INetFwOpenPort) Activator.CreateInstance(Type.GetTypeFromProgID(@"HNetCfg.FwOpenPort"));
            Name = name;
            LocalPorts = new[] {port};
            IsEnable = true;
            Scope = FirewallScope.All;
            Profiles = profile;
            IsEnable = true;
        }

        internal PortRule(INetFwOpenPort port, FirewallProfiles profile)
        {
            UnderlyingObject = port;
            _profiles = profile;
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported
        {
            get => Type.GetTypeFromProgID(@"HNetCfg.FwOpenPort") != null;
        }

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        public INetFwOpenPort UnderlyingObject { get; }

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

            return Profiles == other.Profiles &&
                   UnderlyingObject.Port == other.UnderlyingObject.Port &&
                   UnderlyingObject.Protocol == other.UnderlyingObject.Protocol;
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallAction Action
        {
            get => FirewallAction.Allow;
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallDirection Direction
        {
            get => FirewallDirection.Inbound;
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        public bool IsEnable
        {
            get => UnderlyingObject.Enabled;
            set => UnderlyingObject.Enabled = value;
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public IAddress[] LocalAddresses
        {
            get => new IAddress[] {SingleIP.Any};
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        /// <exception cref="NotSupportedException">
        ///     The array passes to this property as value should have one and
        ///     only one element
        /// </exception>
        public ushort[] LocalPorts
        {
            get => new[] {(ushort) UnderlyingObject.Port};
            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException(
                        "You can not change the identity of a port rule. Consider creating an other rule.");
                }

                if (value.Length > 1)
                {
                    throw new NotSupportedException(
                        "This property only accept an array of one element length.");
                }

                UnderlyingObject.Port = value[0];
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallPortType LocalPortType
        {
            get => FirewallPortType.Specific;
            set => throw new FirewallAPIv1NotSupportedException();
        }

        /// <inheritdoc />
        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }

        /// <inheritdoc />
        public string Name
        {
            get => NativeHelper.ResolveStringResource(UnderlyingObject.Name);
            set => UnderlyingObject.Name = value;
        }


        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Public profile or multiple profile registration is not supported</exception>
        public FirewallProfiles Profiles
        {
            get => _profiles;
            set
            {
                if (EnumHelper.HasFlag(value, FirewallProfiles.Public))
                {
                    throw new NotSupportedException("Public profile is not supported.");
                }

                if (value != FirewallProfiles.Private && value != FirewallProfiles.Domain)
                {
                    throw new NotSupportedException("Multiple profile per each rule is not supported.");
                }

                var rules = Firewall.Instance.Rules;
                var rulesArray = rules.ToArray();

                if (_profiles != 0 && value != _profiles && rulesArray.Contains(this))
                {
                    foreach (var rule in rulesArray)
                    {
                        if (Equals(rule) ||
                            LocalPorts.Length == 1 &&
                            (rule as PortRule)?.LocalPorts.Length == 1 &&
                            LocalPorts[0].Equals((rule as PortRule).LocalPorts[0]) &&
                            Protocol?.ProtocolNumber == (rule as PortRule).Protocol?.ProtocolNumber)
                        {
                            rules.Remove(rule);
                        }
                    }

                    _profiles = value;
                    rules.Add(this);
                }
                else
                {
                    _profiles = value;
                }
            }
        }


        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Only acceptable values are UDP, TCP and Any</exception>
        public FirewallProtocol Protocol
        {
            get => new FirewallProtocol((int) UnderlyingObject.Protocol);
            set
            {
                if (!value.Equals(FirewallProtocol.Any) &&
                    !value.Equals(FirewallProtocol.TCP) &&
                    !value.Equals(FirewallProtocol.UDP))
                {
                    throw new NotSupportedException("Acceptable values are UDP, TCP and Any");
                }

                UnderlyingObject.Protocol = (NET_FW_IP_PROTOCOL) value.ProtocolNumber;
            }
        }


        /// <inheritdoc />
        public IAddress[] RemoteAddresses
        {
            get => AddressHelper.StringToAddresses(UnderlyingObject.RemoteAddresses);
            set => UnderlyingObject.RemoteAddresses = AddressHelper.AddressesToString(value);
        }


        /// <inheritdoc />
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public ushort[] RemotePorts
        {
            get => new ushort[0];
            set => throw new FirewallAPIv1NotSupportedException();
        }


        /// <inheritdoc />
        public FirewallScope Scope
        {
            get => UnderlyingObject.Scope == NET_FW_SCOPE.NET_FW_SCOPE_LOCAL_SUBNET
                ? FirewallScope.LocalSubnet
                : FirewallScope.All;
            set
            {
                if (value == FirewallScope.Specific)
                {
                    throw new ArgumentException("Use the RemoteAddresses property to set the exact remote addresses");
                }

                if (value == FirewallScope.LocalSubnet)
                {
                    RemoteAddresses = new IAddress[] {new LocalSubnet()};
                    UnderlyingObject.Scope = NET_FW_SCOPE.NET_FW_SCOPE_LOCAL_SUBNET;
                }
                else
                {
                    RemoteAddresses = new IAddress[] {SingleIP.Any};
                    UnderlyingObject.Scope = NET_FW_SCOPE.NET_FW_SCOPE_ALL;
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

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="PortRule" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="PortRule" />; otherwise,
        ///     false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="PortRule" />. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as PortRule);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="PortRule" />.
        /// </returns>
        public override int GetHashCode()
        {
            return UnderlyingObject?.GetHashCode() ?? 0;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}