using System;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Exceptions;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper.FirewallRules
{
    /// <inheritdoc cref="IFirewallRule" />
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule
    /// </summary>
    public class FirewallWASRule : IFirewallRule, IEquatable<FirewallWASRule>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public FirewallWASRule(
            string name,
            string filename,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) : this(name, action, direction, profiles)
        {
            ApplicationName = filename;
        }

        /// <summary>
        ///     Creates a new general rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public FirewallWASRule(
            string name,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) :
            this(ComHelper.CreateInstance<INetFwRule>())
        {
            Name = name;
            Action = action;
            Direction = direction;
            IsEnable = true;
            Profiles = profiles;
        }

        /// <summary>
        ///     Creates a new port rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public FirewallWASRule(
            string name,
            ushort port,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) : this(name, action, direction, profiles)
        {
            Protocol = FirewallProtocol.TCP;
            LocalPorts = new[] {port};
        }

        internal FirewallWASRule(INetFwRule rule)
        {
            UnderlyingObject = rule;
        }

        /// <summary>
        ///     Gets or sets the description string of this rule
        /// </summary>
        public string Description
        {
            get => UnderlyingObject.Description;
            set => UnderlyingObject.Description = value;
        }

        /// <summary>
        ///     Gets or sets if EdgeTraversal is available with this rule
        /// </summary>
        public bool EdgeTraversal
        {
            get => UnderlyingObject.EdgeTraversal;
            set => UnderlyingObject.EdgeTraversal = value;
        }

        /// <summary>
        ///     Gets resolved description string of this rule
        /// </summary>
        public string FriendlyDescription
        {
            get => NativeHelper.ResolveStringResource(Description);
        }

        /// <summary>
        ///     Gets or sets the rule grouping string
        /// </summary>
        public string FriendlyGrouping
        {
            get => NativeHelper.ResolveStringResource(Grouping);
        }

        /// <summary>
        ///     Gets or sets the rule grouping string
        /// </summary>
        public string Grouping
        {
            get => UnderlyingObject.Grouping;
            set => UnderlyingObject.Grouping = value;
        }

        /// <summary>
        ///     Gets or sets the list of the acceptable ICMP Messages with this rule
        /// </summary>
        public FirewallWASInternetControlMessage[] ICMPTypesAndCodes
        {
            get => ICMPHelper.StringToICM(UnderlyingObject.IcmpTypesAndCodes);
            set
            {
                if (value.Length > 0 &&
                    !Protocol.Equals(FirewallProtocol.ICMPv4) &&
                    !Protocol.Equals(FirewallProtocol.ICMPv6))
                {
                    throw new FirewallWASInvalidProtocolException(
                        "ICMPTypesAndCodes property can only be specified for the ICMP protocols.");
                }

                UnderlyingObject.IcmpTypesAndCodes = ICMPHelper.ICMToString(value);
            }
        }

        /// <summary>
        ///     Gets or sets the network interfaces that this rule applies to by name
        /// </summary>
        public NetworkInterface[] Interfaces
        {
            get
            {
                if (!(UnderlyingObject.Interfaces is IEnumerable))
                {
                    return new NetworkInterface[0];
                }

                return
                    NetworkInterfaceHelper.StringToInterfaces(
                        ((IEnumerable) UnderlyingObject.Interfaces)
                        .Cast<object>()
                        .Select((o, i) => o?.ToString())
                        .ToArray()
                    );
            }
            set => UnderlyingObject.Interfaces = NetworkInterfaceHelper.InterfacesToString(value);
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported
        {
            get => ComHelper.IsSupported<INetFwRule>();
        }

        /// <summary>
        ///     Gets or sets the network interfaces that this rule applies to by type
        /// </summary>
        public NetworkInterfaceTypes NetworkInterfaceTypes
        {
            get => NetworkInterfaceHelper.StringToInterfaceTypes(UnderlyingObject.InterfaceTypes);
            set => UnderlyingObject.InterfaceTypes = NetworkInterfaceHelper.InterfaceTypesToString(value);
        }

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        protected INetFwRule UnderlyingObject { get; }

        /// <inheritdoc />
        public virtual bool Equals(FirewallWASRule other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (UnderlyingObject == other.UnderlyingObject)
            {
                return true;
            }

            // ReSharper disable once ComplexConditionExpression
            if (!(string.Equals(UnderlyingObject.Name, other.UnderlyingObject.Name) &&
                  UnderlyingObject.Profiles == other.UnderlyingObject.Profiles &&
                  UnderlyingObject.Protocol == other.UnderlyingObject.Protocol &&
                  UnderlyingObject.Action == other.UnderlyingObject.Action &&
                  UnderlyingObject.Enabled == other.UnderlyingObject.Enabled &&
                  UnderlyingObject.Direction == other.UnderlyingObject.Direction &&
                  UnderlyingObject.RemoteAddresses == other.UnderlyingObject.RemoteAddresses &&
                  UnderlyingObject.RemotePorts == other.UnderlyingObject.RemotePorts &&
                  UnderlyingObject.LocalAddresses == other.UnderlyingObject.LocalAddresses &&
                  UnderlyingObject.LocalPorts == other.UnderlyingObject.LocalPorts &&
                  UnderlyingObject.ApplicationName == other.UnderlyingObject.ApplicationName))
            {
                return false;
            }

            if (UnderlyingObject.Interfaces == other.UnderlyingObject.Interfaces)
            {
                return true;
            }

            if (UnderlyingObject.Interfaces is IEnumerable != other.UnderlyingObject.Interfaces is IEnumerable)
            {
                return false;
            }

            if (!(UnderlyingObject.Interfaces is IEnumerable))
            {
                return true;
            }

            return ((IEnumerable) UnderlyingObject.Interfaces)
                .Cast<object>()
                .Select((o, i) => o?.ToString())
                .SequenceEqual(
                    ((IEnumerable) other.UnderlyingObject.Interfaces)
                    .Cast<object>()
                    .Select((o, i) => o?.ToString())
                );
        }

        /// <inheritdoc />
        public bool Equals(IFirewallRule other)
        {
            return Equals(other as FirewallWASRule);
        }

        /// <inheritdoc />
        public FirewallAction Action
        {
            get => UnderlyingObject.Action == NetFwAction.Allow
                ? FirewallAction.Allow
                : FirewallAction.Block;
            set => UnderlyingObject.Action = value == FirewallAction.Allow
                ? NetFwAction.Allow
                : NetFwAction.Block;
        }

        /// <inheritdoc />
        public string ApplicationName
        {
            get => UnderlyingObject.ApplicationName;
            set => UnderlyingObject.ApplicationName = value;
        }

        /// <inheritdoc />
        public FirewallDirection Direction
        {
            get => UnderlyingObject.Direction == NetFwRuleDirection.Inbound
                ? FirewallDirection.Inbound
                : FirewallDirection.Outbound;
            set => UnderlyingObject.Direction = value == FirewallDirection.Inbound
                ? NetFwRuleDirection.Inbound
                : NetFwRuleDirection.Outbound;
        }

        /// <inheritdoc />
        public string FriendlyName
        {
            get => NativeHelper.ResolveStringResource(Name);
        }

        /// <inheritdoc />
        public bool IsEnable
        {
            get => UnderlyingObject.Enabled;
            set => UnderlyingObject.Enabled = value;
        }

        /// <inheritdoc />
        public IAddress[] LocalAddresses
        {
            get => AddressHelper.StringToAddresses(UnderlyingObject.LocalAddresses);
            set
            {
                try
                {
                    UnderlyingObject.LocalAddresses = AddressHelper.AddressesToString(value);
                }
                catch (COMException exception)
                {
                    if ((uint) exception.ErrorCode == 0xD000000D)
                    {
                        throw new ArgumentException(
                            "An unspecified, multicast, broadcast or loopback IPv6 address was specified.", exception);
                    }

                    throw;
                }
            }
        }

        /// <inheritdoc />
        public ushort[] LocalPorts
        {
            get => PortHelper.StringToPorts(UnderlyingObject.LocalPorts);
            set
            {
                if (value.Length > 0 &&
                    !Protocol.Equals(FirewallProtocol.TCP) &&
                    !Protocol.Equals(FirewallProtocol.UDP))
                {
                    throw new FirewallWASInvalidProtocolException(
                        "Port number can only be specified for the UDP and TCP protocols.");
                }

                UnderlyingObject.LocalPorts = PortHelper.PortsToString(value);
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallWASInvalidProtocolException" accessor="set">Protocol and local port type are not compatible.</exception>
        public FirewallPortType LocalPortType
        {
            get
            {
                if (LocalPorts.Length > 0)
                {
                    return FirewallPortType.Specific;
                }

                var localPorts = UnderlyingObject.LocalPorts;

                if (localPorts?.StartsWith(@"RPC,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return FirewallPortType.RPCDynamicPorts;
                }

                if (localPorts?.StartsWith(@"RPC-EPMap,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return FirewallPortType.RPCEndpointMapper;
                }

                if (localPorts?.StartsWith(@"IPHTTPS,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return FirewallPortType.IPHTTPS;
                }

                if (localPorts?.StartsWith(@"Teredo,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return FirewallPortType.EdgeTraversal;
                }

                if (localPorts?.StartsWith(@"Ply2Disc,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return FirewallPortType.PlayToDiscovery;
                }

                return FirewallPortType.All;
            }
            set
            {
                switch (value)
                {
                    case FirewallPortType.All:
                        LocalPorts = new ushort[0];

                        break;
                    case FirewallPortType.RPCDynamicPorts:

                        if (!Protocol.Equals(FirewallProtocol.TCP))
                        {
                            throw new FirewallWASInvalidProtocolException(
                                "RPCDynamicPorts is only valid fot TCP rules. Try setting the protocol to TCP before applying this value."
                            );
                        }

                        UnderlyingObject.LocalPorts = @"RPC,";

                        break;
                    case FirewallPortType.RPCEndpointMapper:

                        if (!Protocol.Equals(FirewallProtocol.TCP))
                        {
                            throw new FirewallWASInvalidProtocolException(
                                "RPCEndpointMapper is only valid fot TCP rules. Try setting the protocol to TCP before applying this value."
                            );
                        }

                        UnderlyingObject.LocalPorts = @"RPC-EPMap,";

                        break;
                    case FirewallPortType.IPHTTPS:

                        if (!Protocol.Equals(FirewallProtocol.TCP))
                        {
                            throw new FirewallWASInvalidProtocolException(
                                "IPHTTPS is only valid fot TCP rules. Try setting the protocol to TCP before applying this value."
                            );
                        }

                        UnderlyingObject.LocalPorts = @"IPHTTPS,";

                        break;
                    case FirewallPortType.EdgeTraversal:

                        if (!Protocol.Equals(FirewallProtocol.UDP))
                        {
                            throw new FirewallWASInvalidProtocolException(
                                "EdgeTraversal is only valid fot UDP rules. Try setting the protocol to TCP before applying this value."
                            );
                        }

                        UnderlyingObject.LocalPorts = @"Teredo,";

                        break;
                    case FirewallPortType.PlayToDiscovery:

                        if (!Protocol.Equals(FirewallProtocol.UDP))
                        {
                            throw new FirewallWASInvalidProtocolException(
                                "PlayToDiscovery is only valid fot UDP rules. Try setting the protocol to TCP before applying this value."
                            );
                        }

                        UnderlyingObject.LocalPorts = @"Ply2Disc,";

                        break;
                    default:

                        throw new ArgumentException("Use the LocalPorts property to set the exact local ports.");
                }
            }
        }

        /// <inheritdoc />
        public string Name
        {
            get => UnderlyingObject.Name;
            set => UnderlyingObject.Name = value;
        }

        /// <inheritdoc />
        public FirewallProfiles Profiles
        {
            get => (FirewallProfiles) UnderlyingObject.Profiles &
                   (FirewallProfiles.Domain |
                    FirewallProfiles.Private |
                    FirewallProfiles.Public);

            set => UnderlyingObject.Profiles =
                (int) (value &
                       (
                           FirewallProfiles.Domain |
                           FirewallProfiles.Private |
                           FirewallProfiles.Public
                       )
                );
        }

        /// <inheritdoc />
        public FirewallProtocol Protocol
        {
            get => new FirewallProtocol(UnderlyingObject.Protocol);
            set
            {
                if ((Protocol.Equals(FirewallProtocol.TCP) || Protocol.Equals(FirewallProtocol.UDP)) &&
                    !(value.Equals(FirewallProtocol.TCP) || value.Equals(FirewallProtocol.UDP)))
                {
                    LocalPorts = new ushort[0];
                    RemotePorts = new ushort[0];
                }

                if ((Protocol.Equals(FirewallProtocol.ICMPv4) || Protocol.Equals(FirewallProtocol.ICMPv6)) &&
                    !(value.Equals(FirewallProtocol.ICMPv4) || value.Equals(FirewallProtocol.ICMPv6)))
                {
                    ICMPTypesAndCodes = new FirewallWASInternetControlMessage[0];
                }

                UnderlyingObject.Protocol = value.ProtocolNumber;
            }
        }

        /// <inheritdoc />
        public IAddress[] RemoteAddresses
        {
            get => AddressHelper.StringToAddresses(UnderlyingObject.RemoteAddresses);
            set
            {
                try
                {
                    UnderlyingObject.RemoteAddresses = AddressHelper.AddressesToString(value);
                }
                catch (COMException exception)
                {
                    if ((uint) exception.ErrorCode == 0xD000000D)
                    {
                        throw new ArgumentException(
                            "An unspecified, multicast, broadcast or loopback IPv6 address was specified.",
                            exception
                        );
                    }

                    throw;
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="FirewallWASInvalidProtocolException" accessor="set">
        ///     Port number can only be specified for the UDP
        ///     and TCP protocols.
        /// </exception>
        public ushort[] RemotePorts
        {
            get => PortHelper.StringToPorts(UnderlyingObject.RemotePorts);
            set
            {
                if (value.Length > 0 &&
                    !Protocol.Equals(FirewallProtocol.TCP) &&
                    !Protocol.Equals(FirewallProtocol.UDP))
                {
                    throw new FirewallWASInvalidProtocolException(
                        "Port number can only be specified for the UDP and TCP protocols."
                    );
                }

                UnderlyingObject.RemotePorts = PortHelper.PortsToString(value);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException" accessor="set">Use the RemoteAddresses property to set the exact remote addresses</exception>
        public FirewallScope Scope
        {
            get
            {
                if (RemoteAddresses.Length <= 1)
                {
                    foreach (var address in RemoteAddresses)
                    {
                        if (SingleIP.Any.Equals(address))
                        {
                            return FirewallScope.All;
                        }

                        if (address is LocalSubnet)
                        {
                            return FirewallScope.LocalSubnet;
                        }
                    }
                }

                return FirewallScope.Specific;
            }
            set
            {
                switch (value)
                {
                    case FirewallScope.All:
                        RemoteAddresses = new IAddress[] {SingleIP.Any};

                        break;
                    case FirewallScope.LocalSubnet:
                        RemoteAddresses = new IAddress[] {new LocalSubnet()};

                        break;
                    default:

                        throw new ArgumentException(
                            "Use the RemoteAddresses property to set the exact remote addresses"
                        );
                }
            }
        }

        /// <inheritdoc />
        public string ServiceName
        {
            get => UnderlyingObject.serviceName;
            set => UnderlyingObject.serviceName = value;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASRule" /> object</param>
        /// <param name="right">A <see cref="FirewallWASRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallWASRule left, FirewallWASRule right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASRule" /> object</param>
        /// <param name="right">A <see cref="FirewallWASRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallWASRule left, FirewallWASRule right)
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
                var hashCode = 132619;
                hashCode = hashCode * 467 + (int) UnderlyingObject.Action;
                hashCode = hashCode * 467 + (int) UnderlyingObject.Direction;
                hashCode = hashCode * 467 + (UnderlyingObject.Name?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.RemoteAddresses?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.RemotePorts?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.LocalAddresses?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.LocalPorts?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.ApplicationName?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.Grouping?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.Description?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.IcmpTypesAndCodes?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.InterfaceTypes?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.serviceName?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + UnderlyingObject.Profiles;
                hashCode = hashCode * 467 + UnderlyingObject.Protocol;
                hashCode = hashCode * 467 + UnderlyingObject.Enabled.GetHashCode();
                hashCode = hashCode * 467 + UnderlyingObject.EdgeTraversal.GetHashCode();
                var interfaces = (UnderlyingObject.Interfaces as IEnumerable)?.Cast<object>();

                if (interfaces != null)
                {
                    var hash = 260671;

                    foreach (var @interface in interfaces)
                    {
                        hash = hash * 727 + (@interface?.ToString().GetHashCode() ?? 0);
                    }

                    hashCode = hashCode * 467 + hash.GetHashCode();
                }
                else
                {
                    hashCode = hashCode * 467;
                }

                return hashCode;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        ///     Returns the underlying COM object
        /// </summary>
        /// <returns>The underlying COM object</returns>
        public INetFwRule GetCOMObject()
        {
            return UnderlyingObject;
        }
    }
}