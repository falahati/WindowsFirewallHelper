using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.Helpers;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv2.Rules
{
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule
    /// </summary>
    public class StandardRule : IRule, IEquatable<StandardRule>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRule(string name, string filename, FirewallAction action, FirewallDirection direction, FirewallProfiles profiles) : this(name, action, direction, profiles)
        {
            ApplicationName = filename;
        }

        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRule(string name, FirewallAction action, FirewallDirection direction, FirewallProfiles profiles)
        {
            UnderlyingObject = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID(@"HNetCfg.FWRule"));
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
        public StandardRule(string name, ushort port, FirewallAction action, FirewallDirection direction,FirewallProfiles profiles) : this(name, action, direction, profiles)
        {
            Protocol = FirewallProtocol.TCP;
            LocalPorts = new[] {port};
        }

        internal StandardRule(INetFwRule rule)
        {
            UnderlyingObject = rule;
        }

        /// <summary>
        ///     Gets or sets the address of the executable file that this rule is about
        /// </summary>
        public string ApplicationName
        {
            get { return UnderlyingObject.ApplicationName; }
            set { UnderlyingObject.ApplicationName = value; }
        }

        /// <summary>
        ///     Gets or sets the description string about this rule
        /// </summary>
        public string Description
        {
            get { return NativeHelper.ResolveStringResource(UnderlyingObject.Description); }
            set { UnderlyingObject.Description = value; }
        }

        /// <summary>
        ///     Gets or sets if EdgeTraversal is available with this rule
        /// </summary>
        public bool EdgeTraversal
        {
            get { return UnderlyingObject.EdgeTraversal; }
            set { UnderlyingObject.EdgeTraversal = value; }
        }

        /// <summary>
        ///     Gets or sets the rule grouping string
        /// </summary>
        public string Grouping
        {
            get { return NativeHelper.ResolveStringResource(UnderlyingObject.Grouping); }
            set { UnderlyingObject.Grouping = value; }
        }

        /// <summary>
        ///     Gets or sets the list of the acceptable ICMP Messages with this rule
        /// </summary>
        public InternetControlMessage[] ICMPTypesAndCodes
        {
            get { return ICMPHelper.StringToICM(UnderlyingObject.IcmpTypesAndCodes); }
            set
            {
                if (value.Length > 0 && !Protocol.Equals(FirewallProtocol.ICMPv4) && !Protocol.Equals(FirewallProtocol.ICMPv6))
                    throw new FirewallAPIv2InvalidProtocolException("ICMPTypesAndCodes property can only be specifid for the ICMP protocols.");
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
                if (!(UnderlyingObject.Interfaces is object[]))
                    return new NetworkInterface[0];
                return NetworkInterfaceHelper.StringToInterfaces(((object[]) UnderlyingObject.Interfaces).Select((o, i) => o.ToString()).ToArray());
            }
            set { UnderlyingObject.Interfaces = NetworkInterfaceHelper.InterfacesToString(value); }
        }

        /// <summary>
        ///     Gets or sets the network interfaces that this rule applies to by type
        /// </summary>
        public FirewallInterfaceTypes InterfaceTypes
        {
            get { return NetworkInterfaceHelper.StringToInterfaceTypes(UnderlyingObject.InterfaceTypes); }
            set { UnderlyingObject.InterfaceTypes = NetworkInterfaceHelper.InterfaceTypesToString(value); }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported => Type.GetTypeFromProgID(@"HNetCfg.FWRule") != null;

        /// <summary>
        ///     Gets or sets the name of the service that this rule is about
        /// </summary>
        public string ServiceName
        {
            get { return UnderlyingObject.serviceName; }
            set { UnderlyingObject.serviceName = value; }
        }

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        public INetFwRule UnderlyingObject { get; }

        /// <summary>
        ///     Determines whether the specified<see cref="StandardRule" /> is equal to the current
        ///     <see cref="StandardRule" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="StandardRule" /> is equal to the current<see cref="StandardRule" />;
        ///     otherwise, false.
        /// </returns>
        public virtual bool Equals(StandardRule other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (UnderlyingObject == other.UnderlyingObject) return true;
            if (!(string.Equals(UnderlyingObject.Name, other.UnderlyingObject.Name) &&
                  (UnderlyingObject.Profiles == other.UnderlyingObject.Profiles) &&
                  (UnderlyingObject.Protocol == other.UnderlyingObject.Protocol) &&
                  (UnderlyingObject.Action == other.UnderlyingObject.Action) &&
                  (UnderlyingObject.Enabled == other.UnderlyingObject.Enabled) &&
                  (UnderlyingObject.Direction == other.UnderlyingObject.Direction) &&
                  (UnderlyingObject.RemoteAddresses == other.UnderlyingObject.RemoteAddresses) &&
                  (UnderlyingObject.RemotePorts == other.UnderlyingObject.RemotePorts) &&
                  (UnderlyingObject.LocalAddresses == other.UnderlyingObject.LocalAddresses) &&
                  (UnderlyingObject.LocalPorts == other.UnderlyingObject.LocalPorts) &&
                  (UnderlyingObject.ApplicationName == other.UnderlyingObject.ApplicationName)))
                return false;
            if (UnderlyingObject.Interfaces == other.UnderlyingObject.Interfaces)
                return true;
            if (UnderlyingObject.Interfaces is object[] != other.UnderlyingObject.Interfaces is object[])
                return false;
            if (!(UnderlyingObject.Interfaces is object[]))
                return true;
            return
                ((object[]) UnderlyingObject.Interfaces).Select((o, i) => o?.ToString())
                    .SequenceEqual(((object[]) other.UnderlyingObject.Interfaces).Select((o, i) => o?.ToString()));
        }

        /// <summary>
        ///     Gets or sets the action that this rules define
        /// </summary>
        public FirewallAction Action
        {
            get
            {
                return UnderlyingObject.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW
                    ? FirewallAction.Allow
                    : FirewallAction.Block;
            }
            set
            {
                UnderlyingObject.Action = value == FirewallAction.Allow
                    ? NET_FW_ACTION_.NET_FW_ACTION_ALLOW
                    : NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            }
        }

        /// <summary>
        ///     Gets or sets the data direction that rule applies to
        /// </summary>
        public FirewallDirection Direction
        {
            get
            {
                return UnderlyingObject.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN
                    ? FirewallDirection.Inbound
                    : FirewallDirection.Outbound;
            }
            set
            {
                UnderlyingObject.Direction = value == FirewallDirection.Inbound
                    ? NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN
                    : NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            }
        }

        /// <summary>
        ///     Gets or sets a Boolean value indicating if this rule is active
        /// </summary>
        public bool IsEnable
        {
            get { return UnderlyingObject.Enabled; }
            set { UnderlyingObject.Enabled = value; }
        }

        /// <summary>
        ///     Gets or sets the local addresses that rule applies to
        /// </summary>
        public IAddress[] LocalAddresses
        {
            get { return AddressHelper.StringToAddresses(UnderlyingObject.LocalAddresses); }
            set
            {
                try
                {
                    UnderlyingObject.LocalAddresses = AddressHelper.AddressesToString(value);
                }
                catch (COMException exception)
                {
                    if ((uint) exception.ErrorCode == 0xD000000D)
                        throw new ArgumentException(
                            "An unspecified, multicast, broadcast or loopback IPv6 address was specified.", exception);
                    throw;
                }
            }
        }
        
        /// <inheritdoc />
        public LocalPortTypes LocalPortType
        {
            get
            {
                if (LocalPorts.Length > 0)
                {
                    return LocalPortTypes.Specific;
                }
                if (UnderlyingObject.LocalPorts?.StartsWith("RPC,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return LocalPortTypes.RPCDynamicPorts;
                }
                if (UnderlyingObject.LocalPorts?.StartsWith("RPC-EPMap,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return LocalPortTypes.RPCEndpointMapper;
                }
                if (UnderlyingObject.LocalPorts?.StartsWith("IPHTTPS,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return LocalPortTypes.IPHTTPS;
                }
                if (UnderlyingObject.LocalPorts?.StartsWith("Teredo,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return LocalPortTypes.EdgeTraversal;
                }
                if (UnderlyingObject.LocalPorts?.StartsWith("Ply2Disc,", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return LocalPortTypes.PlayToDiscovery;
                }
                return LocalPortTypes.All;
            }
            set
            {
                switch (value)
                {
                    case LocalPortTypes.All:
                        LocalPorts = new ushort[0];
                        break;
                    case LocalPortTypes.RPCDynamicPorts:
                        if (!Protocol.Equals(FirewallProtocol.TCP))
                            throw new FirewallAPIv2InvalidProtocolException("RPCDynamicPorts is only valid fot TCP rules. Try setting the protocol to TCP before applying this value.");
                        UnderlyingObject.LocalPorts = "RPC,";
                        break;
                    case LocalPortTypes.RPCEndpointMapper:
                        if (!Protocol.Equals(FirewallProtocol.TCP))
                            throw new FirewallAPIv2InvalidProtocolException("RPCEndpointMapper is only valid fot TCP rules. Try setting the protocol to TCP before applying this value.");
                        UnderlyingObject.LocalPorts = "RPC-EPMap,";
                        break;
                    case LocalPortTypes.IPHTTPS:
                        if (!Protocol.Equals(FirewallProtocol.TCP))
                            throw new FirewallAPIv2InvalidProtocolException("IPHTTPS is only valid fot TCP rules. Try setting the protocol to TCP before applying this value.");
                        UnderlyingObject.LocalPorts = "IPHTTPS,";
                        break;
                    case LocalPortTypes.EdgeTraversal:
                        if (!Protocol.Equals(FirewallProtocol.UDP))
                            throw new FirewallAPIv2InvalidProtocolException("EdgeTraversal is only valid fot UDP rules. Try setting the protocol to TCP before applying this value.");
                        UnderlyingObject.LocalPorts = "Teredo,";
                        break;
                    case LocalPortTypes.PlayToDiscovery:
                        if (!Protocol.Equals(FirewallProtocol.UDP))
                            throw new FirewallAPIv2InvalidProtocolException("PlayToDiscovery is only valid fot UDP rules. Try setting the protocol to TCP before applying this value.");
                        UnderlyingObject.LocalPorts = "Ply2Disc,";
                        break;
                    default:
                        throw new ArgumentException("Use the LocalPorts property to set the exact local ports.");
                }
            }
        }

        /// <summary>
        ///     Gets or sets the local ports that rule applies to
        /// </summary>
        public ushort[] LocalPorts
        {
            get { return AddressHelper.StringToPorts(UnderlyingObject.LocalPorts); }
            set
            {
                if (value.Length > 0 && !Protocol.Equals(FirewallProtocol.TCP) && !Protocol.Equals(FirewallProtocol.UDP))
                    throw new FirewallAPIv2InvalidProtocolException("Port number can only be specifid for the UDP and TCP protocols.");
                UnderlyingObject.LocalPorts = AddressHelper.PortsToString(value);
            }
        }


        /// <summary>
        ///     Gets or sets the name of the rule
        /// </summary>
        public string Name
        {
            get { return NativeHelper.ResolveStringResource(UnderlyingObject.Name); }
            set { UnderlyingObject.Name = value; }
        }

        /// <summary>
        ///     Gets or sets the profiles that this rule belongs to
        /// </summary>
        public FirewallProfiles Profiles
        {
            get
            {
                return (FirewallProfiles) UnderlyingObject.Profiles &
                       (FirewallProfiles.Domain | FirewallProfiles.Private |
                        FirewallProfiles.Public);
            }
            set
            {
                UnderlyingObject.Profiles = (int) (value &
                                                   (FirewallProfiles.Domain | FirewallProfiles.Private |
                                                    FirewallProfiles.Public));
            }
        }

        /// <summary>
        ///     Gets or sets the protocol that rule applies to
        /// </summary>
        public FirewallProtocol Protocol
        {
            get { return new FirewallProtocol(UnderlyingObject.Protocol); }
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
                    ICMPTypesAndCodes = new InternetControlMessage[0];
                }
                UnderlyingObject.Protocol = value.ProtocolNumber;
            }
        }

        /// <summary>
        ///     Gets or sets the remote addresses that rule applies to
        /// </summary>
        public IAddress[] RemoteAddresses
        {
            get { return AddressHelper.StringToAddresses(UnderlyingObject.RemoteAddresses); }
            set
            {
                try
                {
                    UnderlyingObject.RemoteAddresses = AddressHelper.AddressesToString(value);
                }
                catch (COMException exception)
                {
                    if ((uint) exception.ErrorCode == 0xD000000D)
                        throw new ArgumentException(
                            "An unspecified, multicast, broadcast or loopback IPv6 address was specified.", exception);
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the remote ports that rule applies to
        /// </summary>
        public ushort[] RemotePorts
        {
            get { return AddressHelper.StringToPorts(UnderlyingObject.RemotePorts); }
            set
            {
                if (value.Length > 0 && !Protocol.Equals(FirewallProtocol.TCP) && !Protocol.Equals(FirewallProtocol.UDP))
                    throw new FirewallAPIv2InvalidProtocolException("Port number can only be specifid for the UDP and TCP protocols.");
                UnderlyingObject.RemotePorts = AddressHelper.PortsToString(value);
            }
        }

        /// <summary>
        ///     Gets or sets the scope that this rule applies to
        /// </summary>
        public FirewallScope Scope
        {
            get
            {
                if (RemoteAddresses.Length <= 1)
                    foreach (var address in RemoteAddresses)
                    {
                        if (SingleIP.Any.Equals(address))
                            return FirewallScope.All;
                        if (address is LocalSubnet)
                            return FirewallScope.LocalSubnet;
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
                        throw new ArgumentException("Use the RemoteAddresses property to set the exact remote addresses");
                }
            }
        }

        /// <summary>
        ///     Compares two <see cref="StandardRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="StandardRule" /> object</param>
        /// <param name="right">A <see cref="StandardRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(StandardRule left, StandardRule right)
        {
            return (((object) left != null) && ((object) right != null) && left.Equals(right)) ||
                   (((object) left == null) && ((object) right == null));
        }

        /// <summary>
        ///     Compares two <see cref="StandardRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="StandardRule" /> object</param>
        /// <param name="right">A <see cref="StandardRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(StandardRule left, StandardRule right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="StandardRule" />
        ///     .
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="StandardRule" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="StandardRule" />. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StandardRule);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 132619;
                hashCode = hashCode*467 + (int) UnderlyingObject.Action;
                hashCode = hashCode*467 + (int) UnderlyingObject.Direction;
                hashCode = hashCode*467 + (UnderlyingObject.Name?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.RemoteAddresses?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.RemotePorts?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.LocalAddresses?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.LocalPorts?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.ApplicationName?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.Grouping?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.Description?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.IcmpTypesAndCodes?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.InterfaceTypes?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObject.serviceName?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + UnderlyingObject.Profiles;
                hashCode = hashCode*467 + UnderlyingObject.Protocol;
                hashCode = hashCode*467 + UnderlyingObject.Enabled.GetHashCode();
                hashCode = hashCode*467 + UnderlyingObject.EdgeTraversal.GetHashCode();
                var interfaces = UnderlyingObject.Interfaces as object[];
                if (interfaces != null)
                {
                    var hash = 260671;
                    foreach (var @interface in interfaces)
                        hash = hash*727 + (@interface?.ToString().GetHashCode() ?? 0);
                    hashCode = hashCode*467 + hash.GetHashCode();
                }
                else
                    hashCode = hashCode*467;
                return hashCode;
            }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}