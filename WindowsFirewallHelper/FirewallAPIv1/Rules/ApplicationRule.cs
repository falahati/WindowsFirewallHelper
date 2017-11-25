using System;
using WindowsFirewallHelper.Addresses;
using WindowsFirewallHelper.Helpers;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv1.Rules
{
    /// <summary>
    ///     Contains properties of a Windows Firewall v1 application rule
    /// </summary>
    public class ApplicationRule : IRule, IEquatable<ApplicationRule>
    {
        private FirewallProfiles _profiles;

        /// <summary>
        ///     Creates a new application rule for Windows Firewall v1
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="processAddress">Address of the executable file</param>
        /// <param name="profile">The profile that this rule belongs to</param>
        public ApplicationRule(string name, string processAddress, FirewallProfiles profile)
        {
            UnderlyingObject = (INetFwAuthorizedApplication) Activator.CreateInstance(
                Type.GetTypeFromProgID(@"HNetCfg.FwAuthorizedApplication"));
            Name = name;
            ExecutableAddress = processAddress;
            IsEnable = true;
            Scope = FirewallScope.All;
            Profiles = profile;
            IsEnable = true;
        }

        internal ApplicationRule(INetFwAuthorizedApplication application, FirewallProfiles profile)
        {
            UnderlyingObject = application;
            _profiles = profile;
        }

        /// <summary>
        ///     Gets or sets the address of the executable file that this rule is about
        /// </summary>
        public string ExecutableAddress
        {
            get { return UnderlyingObject.ProcessImageFileName; }
            set { UnderlyingObject.ProcessImageFileName = value; }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public static bool IsSupported => Type.GetTypeFromProgID(@"HNetCfg.FwAuthorizedApplication") != null;

        /// <summary>
        ///     Returns the underlying Windows Firewall Object
        /// </summary>
        public INetFwAuthorizedApplication UnderlyingObject { get; }

        /// <summary>
        ///     Determines whether the specified<see cref="ApplicationRule" /> is equal to the current
        ///     <see cref="ApplicationRule" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="ApplicationRule" /> is equal to the current<see cref="ApplicationRule" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(ApplicationRule other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Profiles == other.Profiles) &&
                   string.Equals(UnderlyingObject.ProcessImageFileName, other.UnderlyingObject.ProcessImageFileName);
        }

        /// <summary>
        ///     Gets the action that this rules define
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallAction Action
        {
            get { return FirewallAction.Allow; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets the data direction that rule applies to
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallDirection Direction
        {
            get { return FirewallDirection.Inbound; }
            set { throw new FirewallAPIv1NotSupportedException(); }
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
        ///     Gets the local addresses that rule applies to
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public IAddress[] LocalAddresses
        {
            get { return new IAddress[] {SingleIP.Any}; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets the local ports that rule applies to
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public ushort[] LocalPorts
        {
            get { return new ushort[0]; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets or sets the name of the rule
        /// </summary>
        public string Name
        {
            get { return UnderlyingObject.Name; }
            set { UnderlyingObject.Name = value; }
        }


        /// <summary>
        ///     Gets or sets the profiles that this rule belongs to
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Public profile or multiple profile registration is not supported</exception>
        public FirewallProfiles Profiles
        {
            get { return _profiles; }
            set
            {
                if (EnumHelper.HasFlag(value, FirewallProfiles.Public))
                    throw new FirewallAPIv1NotSupportedException("Public profile is not supported.");
                if ((value != FirewallProfiles.Private) && (value != FirewallProfiles.Domain))
                    throw new FirewallAPIv1NotSupportedException("Multiple profile per each rule is not supported.");
                var rules = Firewall.Instance.Rules;
                var rulesArray = EnumerableHelper.EnumerableToArray(rules);
                if ((_profiles != 0) && (value != _profiles) && EnumerableHelper.Contains(rulesArray, this))
                {
                    foreach (var rule in rulesArray)
                        if (Equals(rule) ||
                            (string.IsNullOrEmpty(ExecutableAddress?.Trim()) &&
                             string.Equals((rule as ApplicationRule)?.ExecutableAddress, ExecutableAddress,
                                 StringComparison.OrdinalIgnoreCase)))
                            rules.Remove(rule);
                    _profiles = value;
                    rules.Add(this);
                }
                else
                {
                    _profiles = value;
                }
            }
        }

        /// <summary>
        ///     Gets the protocol that rule applies to
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallProtocol Protocol
        {
            get { return FirewallProtocol.Any; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets or sets the remote addresses that rule applies to
        /// </summary>
        public IAddress[] RemoteAddresses
        {
            get { return AddressHelper.StringToAddresses(UnderlyingObject.RemoteAddresses); }
            set { UnderlyingObject.RemoteAddresses = AddressHelper.AddressesToString(value); }
        }

        /// <summary>
        ///     Gets the remote ports that rule applies to
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public ushort[] RemotePorts
        {
            get { return new ushort[0]; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets or sets the scope that this rule applies to
        /// </summary>
        public FirewallScope Scope
        {
            get
            {
                return UnderlyingObject.Scope == NET_FW_SCOPE_.NET_FW_SCOPE_LOCAL_SUBNET
                    ? FirewallScope.LocalSubnet
                    : FirewallScope.All;
            }
            set
            {
                if (value == FirewallScope.Specific)
                    throw new ArgumentException("Use the RemoteAddresses property to set the exact remote addresses");
                if (value == FirewallScope.LocalSubnet)
                {
                    RemoteAddresses = new IAddress[] {new LocalSubnet()};
                    UnderlyingObject.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_LOCAL_SUBNET;
                }
                else
                {
                    RemoteAddresses = new IAddress[] {SingleIP.Any};
                    UnderlyingObject.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
                }
            }
        }


        /// <summary>
        ///     Compares two <see cref="ApplicationRule" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="ApplicationRule" /> object</param>
        /// <param name="right">A <see cref="ApplicationRule" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(ApplicationRule left, ApplicationRule right)
        {
            return (((object) left != null) && ((object) right != null) && left.Equals(right)) ||
                   (((object) left == null) && ((object) right == null));
        }

        /// <summary>
        ///     Compares two <see cref="ApplicationRule" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="ApplicationRule" /> object</param>
        /// <param name="right">A <see cref="ApplicationRule" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(ApplicationRule left, ApplicationRule right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="ApplicationRule" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="ApplicationRule" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="ApplicationRule" />. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ApplicationRule) obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="ApplicationRule" />.
        /// </returns>
        public override int GetHashCode()
        {
            return UnderlyingObject?.GetHashCode() ?? 0;
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="ApplicationRule" /> object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current <see cref="ApplicationRule" /> object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}