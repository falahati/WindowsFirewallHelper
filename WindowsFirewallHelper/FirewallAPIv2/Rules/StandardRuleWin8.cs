using System;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv2.Rules
{
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule in Windows 8 and above
    /// </summary>
    public class StandardRuleWin8 : StandardRuleWin7, IEquatable<StandardRuleWin8>
    {
        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRuleWin8(string name, string filename, FirewallAction action, FirewallDirection direction,
            FirewallProfiles profiles)
            : base(name, filename, action, direction, profiles)
        {
            if (UnderlyingObjectV3 == null)
                throw new FirewallAPIv2NotSupportedException();
        }

        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRuleWin8(string name, FirewallAction action, FirewallDirection direction,
            FirewallProfiles profiles)
            : base(name, action, direction, profiles)
        {
            if (UnderlyingObjectV3 == null)
                throw new FirewallAPIv2NotSupportedException();
        }

        /// <summary>
        ///     Creates a new port rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        public StandardRuleWin8(string name, ushort port, FirewallAction action, FirewallDirection direction,
            FirewallProfiles profiles)
            : base(name, port, action, direction, profiles)
        {
            if (UnderlyingObjectV3 == null)
                throw new FirewallAPIv2NotSupportedException();
        }

        internal StandardRuleWin8(INetFwRule3 rule) : base(rule)
        {
        }

        /// <summary>
        ///     Gets or sets the PackageId of the Windows Store Application that this rule applies to
        /// </summary>
        public string ApplicationPackageId
        {
            get { return UnderlyingObjectV3.LocalAppPackageId; }
            set { UnderlyingObjectV3.LocalAppPackageId = value; }
        }

        /// <summary>
        ///     Gets or sets the expected Internet Protocol Security level of this rule
        /// </summary>
        public IPSecSecurityLevel IPSecSecurityLevel
        {
            get
            {
                if (!Enum.IsDefined(typeof(IPSecSecurityLevel), UnderlyingObjectV3.SecureFlags))
                    throw new NotSupportedException();
                return (IPSecSecurityLevel) UnderlyingObjectV3.SecureFlags;
            }
            set
            {
                if (!Enum.IsDefined(typeof(IPSecSecurityLevel), value))
                    throw new NotSupportedException();
                UnderlyingObjectV3.SecureFlags = (int) value;
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public new static bool IsSupported => Environment.OSVersion.Version >= new Version(6, 2);

        /// <summary>
        ///     Gets or sets the list of the authorized local users
        /// </summary>
        public string LocalUserAuthorizedList
        {
            get { return UnderlyingObjectV3.LocalUserAuthorizedList; }
            set { UnderlyingObjectV3.LocalUserAuthorizedList = value; }
        }

        /// <summary>
        ///     Gets or sets the list of the authorized remote machines
        /// </summary>
        public string RemoteMachineAuthorizedList
        {
            get { return UnderlyingObjectV3.RemoteMachineAuthorizedList; }
            set { UnderlyingObjectV3.RemoteMachineAuthorizedList = value; }
        }

        /// <summary>
        ///     Gets or sets the list of the authorized remote users
        /// </summary>
        public string RemoteUserAuthorizedList
        {
            get { return UnderlyingObjectV3.RemoteUserAuthorizedList; }
            set { UnderlyingObjectV3.RemoteUserAuthorizedList = value; }
        }

        private INetFwRule3 UnderlyingObjectV3 => UnderlyingObject as INetFwRule3;

        /// <summary>
        ///     Gets or sets the Domain and User Name of the user that owns this rule
        /// </summary>
        public string UserOwner
        {
            get { return UnderlyingObjectV3.LocalUserOwner; }
            set { UnderlyingObjectV3.LocalUserOwner = value; }
        }

        /// <inheritdoc />
        public bool Equals(StandardRuleWin8 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!base.Equals(other)) return false;
            return string.Equals(UnderlyingObjectV3.LocalAppPackageId, other.UnderlyingObjectV3.LocalAppPackageId) &&
                   string.Equals(UnderlyingObjectV3.LocalUserAuthorizedList,
                       other.UnderlyingObjectV3.LocalUserAuthorizedList) &&
                   string.Equals(UnderlyingObjectV3.RemoteMachineAuthorizedList,
                       other.UnderlyingObjectV3.RemoteMachineAuthorizedList) &&
                   string.Equals(UnderlyingObjectV3.RemoteUserAuthorizedList,
                       other.UnderlyingObjectV3.RemoteUserAuthorizedList) &&
                   string.Equals(UnderlyingObjectV3.LocalUserOwner, other.UnderlyingObjectV3.LocalUserOwner) &&
                   (UnderlyingObjectV3.SecureFlags == other.UnderlyingObjectV3.SecureFlags);
        }

        /// <inheritdoc />
        public static bool operator ==(StandardRuleWin8 left, StandardRuleWin8 right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(StandardRuleWin8 left, StandardRuleWin8 right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StandardRuleWin8);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = hashCode*467 + (UnderlyingObjectV3.LocalAppPackageId?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObjectV3.LocalUserAuthorizedList?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObjectV3.RemoteMachineAuthorizedList?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObjectV3.RemoteUserAuthorizedList?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + (UnderlyingObjectV3.LocalUserOwner?.GetHashCode() ?? 0);
                hashCode = hashCode*467 + UnderlyingObjectV3.SecureFlags;
                return hashCode;
            }
        }
    }
}