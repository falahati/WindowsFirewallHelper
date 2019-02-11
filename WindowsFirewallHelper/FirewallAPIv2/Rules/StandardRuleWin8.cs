using System;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv2.Rules
{
    /// <inheritdoc cref="StandardRuleWin7" />
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule in Windows 8 and above
    /// </summary>
    public class StandardRuleWin8 : StandardRuleWin7, IEquatable<StandardRuleWin8>
    {
        /// <inheritdoc />
        /// <summary>
        ///     Creates a new application rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="filename">Address of the executable file</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public StandardRuleWin8(
            string name,
            string filename,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) : base(name, filename, action, direction, profiles)
        {
        }

        /// <summary>
        ///     Creates a new general rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        protected StandardRuleWin8(string name, FirewallAction action, FirewallDirection direction, FirewallProfiles profiles) :
            base(name, action, direction, profiles)
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a new port rule for Windows Firewall with Advanced Security
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="port">Port number of the rule</param>
        /// <param name="action">Action that this rule defines</param>
        /// <param name="direction">Data direction in which this rule applies to</param>
        /// <param name="profiles">The profile that this rule belongs to</param>
        // ReSharper disable once TooManyDependencies
        public StandardRuleWin8(
            string name,
            ushort port,
            FirewallAction action,
            FirewallDirection direction,
            FirewallProfiles profiles) : base(name, port, action, direction, profiles)
        {
        }

        /// <inheritdoc />
        // ReSharper disable once SuggestBaseTypeForParameter
        internal StandardRuleWin8(INetFwRule3 rule) : base(rule)
        {
        }

        /// <summary>
        ///     Gets or sets the PackageId of the Windows Store Application that this rule applies to
        /// </summary>
        public string ApplicationPackageId
        {
            get => UnderlyingObject.LocalAppPackageId;
            set => UnderlyingObject.LocalAppPackageId = value;
        }

        /// <summary>
        ///     Gets or sets the expected Internet Protocol Security level of this rule
        /// </summary>
        public IPSecSecurityLevel IPSecSecurityLevel
        {
            get
            {
                if (!Enum.IsDefined(typeof(IPSecSecurityLevel), UnderlyingObject.SecureFlags))
                {
                    throw new ArgumentOutOfRangeException();
                }

                return (IPSecSecurityLevel)UnderlyingObject.SecureFlags;
            }
            set
            {
                if (!Enum.IsDefined(typeof(IPSecSecurityLevel), value))
                {
                    throw new ArgumentOutOfRangeException();
                }

                UnderlyingObject.SecureFlags = (int) value;
            }
        }

        /// <summary>
        ///     Returns a Boolean value indicating if these class is available in the current machine
        /// </summary>
        public new static bool IsSupported
        {
            get => StandardRuleWin7.IsSupported && COMHelper.IsSupported<INetFwRule3>();
        }

        /// <summary>
        ///     Gets or sets the list of the authorized local users
        /// </summary>
        public string LocalUserAuthorizedList
        {
            get => UnderlyingObject.LocalUserAuthorizedList;
            set => UnderlyingObject.LocalUserAuthorizedList = value;
        }

        /// <summary>
        ///     Gets or sets the list of the authorized remote machines
        /// </summary>
        public string RemoteMachineAuthorizedList
        {
            get => UnderlyingObject.RemoteMachineAuthorizedList;
            set => UnderlyingObject.RemoteMachineAuthorizedList = value;
        }

        /// <summary>
        ///     Gets or sets the list of the authorized remote users
        /// </summary>
        public string RemoteUserAuthorizedList
        {
            get => UnderlyingObject.RemoteUserAuthorizedList;
            set => UnderlyingObject.RemoteUserAuthorizedList = value;
        }

        protected new INetFwRule3 UnderlyingObject
        {
            get => base.UnderlyingObject as INetFwRule3;
        }

        /// <summary>
        ///     Gets or sets the Domain and User Name of the user that owns this rule
        /// </summary>
        public string UserOwner
        {
            get => UnderlyingObject.LocalUserOwner;
            set => UnderlyingObject.LocalUserOwner = value;
        }

        /// <inheritdoc />
        public bool Equals(StandardRuleWin8 other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!base.Equals(other))
            {
                return false;
            }

            return string.Equals(UnderlyingObject.LocalAppPackageId, other.UnderlyingObject.LocalAppPackageId) &&
                   string.Equals(UnderlyingObject.LocalUserAuthorizedList,
                       other.UnderlyingObject.LocalUserAuthorizedList) &&
                   string.Equals(UnderlyingObject.RemoteMachineAuthorizedList,
                       other.UnderlyingObject.RemoteMachineAuthorizedList) &&
                   string.Equals(UnderlyingObject.RemoteUserAuthorizedList,
                       other.UnderlyingObject.RemoteUserAuthorizedList) &&
                   string.Equals(UnderlyingObject.LocalUserOwner, other.UnderlyingObject.LocalUserOwner) &&
                   UnderlyingObject.SecureFlags == other.UnderlyingObject.SecureFlags;
        }

        public static bool operator ==(StandardRuleWin8 left, StandardRuleWin8 right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        public static bool operator !=(StandardRuleWin8 left, StandardRuleWin8 right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as StandardRuleWin8);
        }

        public new INetFwRule3 GetCOMObject()
        {
            return UnderlyingObject;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = hashCode * 467 + (UnderlyingObject.LocalAppPackageId?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.LocalUserAuthorizedList?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.RemoteMachineAuthorizedList?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.RemoteUserAuthorizedList?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + (UnderlyingObject.LocalUserOwner?.GetHashCode() ?? 0);
                hashCode = hashCode * 467 + UnderlyingObject.SecureFlags;

                return hashCode;
            }
        }
    }
}