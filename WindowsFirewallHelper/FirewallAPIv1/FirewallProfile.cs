using System;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    /// <summary>
    ///     Contains properties of a Windows Firewall v1 profile
    /// </summary>
    public class FirewallProfile : IProfile
    {
        internal FirewallProfile(INetFwProfile underlyingObject)
        {
            UnderlyingObject = underlyingObject;
        }

        internal INetFwProfile UnderlyingObject { get; }

        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is enable and available
        /// </summary>
        public bool Enable
        {
            get { return UnderlyingObject.FirewallEnabled; }
            set { UnderlyingObject.FirewallEnabled = value; }
        }

        /// <summary>
        ///     Gets a FirewallProfiles showing the type of this firewall profile
        /// </summary>
        public FirewallProfiles Type
        {
            get
            {
                switch (UnderlyingObject.Type)
                {
                    case NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN:
                        return FirewallProfiles.Domain;
                    case NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_STANDARD:
                        return FirewallProfiles.Private;
                    default:
                        throw new FirewallAPIv1NotSupportedException();
                }
            }
        }

        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is the currently active profile.
        /// </summary>
        public bool IsActive => Firewall.Instance.UnderlyingObject?.CurrentProfileType == UnderlyingObject.Type;


        /// <summary>
        ///     Gets or sets a Boolean value that blocks all inbound traffic completely regardless of any rules in this profile
        /// </summary>
        public bool BlockAllInboundTraffic
        {
            get { return UnderlyingObject.ExceptionsNotAllowed; }
            set { UnderlyingObject.ExceptionsNotAllowed = value; }
        }

        /// <summary>
        ///     Gets the global default behavior regarding inbound traffic
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallAction DefaultInboundAction
        {
            get { return FirewallAction.Block; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets the global default behavior regarding outbound traffic
        /// </summary>
        /// <exception cref="FirewallAPIv1NotSupportedException">Setting a value for this property is not supported</exception>
        public FirewallAction DefaultOutboundAction
        {
            get { return FirewallAction.Allow; }
            set { throw new FirewallAPIv1NotSupportedException(); }
        }

        /// <summary>
        ///     Gets or sets a value indicating if the firewall should send unicast responses to the multicast broadcasts
        /// </summary>
        public bool UnicastResponsesToMulticastBroadcast
        {
            get { return !UnderlyingObject.UnicastResponsesToMulticastBroadcastDisabled; }
            set { UnderlyingObject.UnicastResponsesToMulticastBroadcastDisabled = !value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating if the user should get notifications about rules of this profile
        /// </summary>
        public bool ShowNotifications
        {
            get { return !UnderlyingObject.NotificationsDisabled; }
            set { UnderlyingObject.NotificationsDisabled = !value; }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            try
            {
                return Type.ToString();
            }
            catch (NotSupportedException)
            {
                return base.ToString();
            }
        }
    }
}