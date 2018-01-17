using System;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv2.Rules
{
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security rule in Windows 8 and above
    /// </summary>
    public class StandardRuleWin8 : StandardRuleWin7
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
    }
}