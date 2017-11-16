using System;
using NetFwTypeLib;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security profile
    /// </summary>
    public class FirewallProfile : IProfile
    {
        private readonly NET_FW_PROFILE_TYPE2_ _profile;

        internal FirewallProfile(INetFwPolicy2 underlyingObject, NET_FW_PROFILE_TYPE2_ profile)
        {
            _profile = profile;
            UnderlyingObject = underlyingObject;
        }

        internal INetFwPolicy2 UnderlyingObject { get; }


        /// <summary>
        ///     Gets or sets a Boolean value that blocks all inbound traffic completely regardless of any rules in this profile
        /// </summary>
        public bool BlockAllInboundTraffic
        {
            get { return UnderlyingObject.BlockAllInboundTraffic[_profile]; }
            set { UnderlyingObject.BlockAllInboundTraffic[_profile] = value; }
        }

        /// <summary>
        ///     Gets or sets the global default behavior regarding inbound traffic
        /// </summary>
        public FirewallAction DefaultInboundAction
        {
            get
            {
                return UnderlyingObject.DefaultInboundAction[_profile] == NET_FW_ACTION_.NET_FW_ACTION_ALLOW
                    ? FirewallAction.Allow
                    : FirewallAction.Block;
            }
            set
            {
                UnderlyingObject.DefaultInboundAction[_profile] = value == FirewallAction.Allow
                    ? NET_FW_ACTION_.NET_FW_ACTION_ALLOW
                    : NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            }
        }

        /// <summary>
        ///     Gets or sets the global default behavior regarding outbound traffic
        /// </summary>
        public FirewallAction DefaultOutboundAction
        {
            get
            {
                return UnderlyingObject.DefaultOutboundAction[_profile] == NET_FW_ACTION_.NET_FW_ACTION_ALLOW
                    ? FirewallAction.Allow
                    : FirewallAction.Block;
            }
            set
            {
                UnderlyingObject.DefaultOutboundAction[_profile] = value == FirewallAction.Allow
                    ? NET_FW_ACTION_.NET_FW_ACTION_ALLOW
                    : NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            }
        }

        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is enable and available
        /// </summary>
        public bool Enable
        {
            get { return UnderlyingObject.FirewallEnabled[_profile]; }
            set { UnderlyingObject.FirewallEnabled[_profile] = value; }
        }

        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is the currently active profile.
        /// </summary>
        public bool IsActive => ((NET_FW_PROFILE_TYPE2_) UnderlyingObject.CurrentProfileTypes ==
                                 NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) ||
                                (((NET_FW_PROFILE_TYPE2_) UnderlyingObject.CurrentProfileTypes & _profile) == _profile);

        /// <summary>
        ///     Gets or sets a value indicating if the user should get notifications about rules of this profile
        /// </summary>
        public bool ShowNotifications
        {
            get { return !UnderlyingObject.NotificationsDisabled[_profile]; }
            set { UnderlyingObject.NotificationsDisabled[_profile] = !value; }
        }

        /// <summary>
        ///     Gets a FirewallProfiles showing the type of this firewall profile
        /// </summary>
        public FirewallProfiles Type
        {
            get
            {
                if (_profile == NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL)
                    throw new NotSupportedException();
                return (FirewallProfiles) _profile;
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating if the firewall should send unicast responses to the multicast broadcasts
        /// </summary>
        public bool UnicastResponsesToMulticastBroadcast
        {
            get { return !UnderlyingObject.UnicastResponsesToMulticastBroadcastDisabled[_profile]; }
            set { UnderlyingObject.UnicastResponsesToMulticastBroadcastDisabled[_profile] = !value; }
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