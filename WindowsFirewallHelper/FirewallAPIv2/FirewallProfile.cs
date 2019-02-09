using System;
using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <inheritdoc />
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security profile
    /// </summary>
    public class FirewallProfile : IProfile
    {
        private readonly NET_FW_PROFILE_TYPE2 _profile;

        internal FirewallProfile(Firewall firewall, NET_FW_PROFILE_TYPE2 profile)
        {
            _profile = profile;
            Firewall = firewall;
        }

        private Firewall Firewall { get; }
        
        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets a Boolean value that blocks all inbound traffic completely regardless of any rules in this profile
        /// </summary>
        public bool BlockAllInboundTraffic
        {
            get => Firewall.UnderlyingObject.get_BlockAllInboundTraffic(_profile);
            set => Firewall.UnderlyingObject.set_BlockAllInboundTraffic(_profile, value);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the global default behavior regarding inbound traffic
        /// </summary>
        public FirewallAction DefaultInboundAction
        {
            get => Firewall.UnderlyingObject.get_DefaultInboundAction(_profile) == NET_FW_ACTION.NET_FW_ACTION_ALLOW
                ? FirewallAction.Allow
                : FirewallAction.Block;
            set => Firewall.UnderlyingObject.set_DefaultInboundAction(
                _profile,
                value == FirewallAction.Allow
                    ? NET_FW_ACTION.NET_FW_ACTION_ALLOW
                    : NET_FW_ACTION.NET_FW_ACTION_BLOCK
            );
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the global default behavior regarding outbound traffic
        /// </summary>
        public FirewallAction DefaultOutboundAction
        {
            get => Firewall.UnderlyingObject.get_DefaultOutboundAction(_profile) == NET_FW_ACTION.NET_FW_ACTION_ALLOW
                ? FirewallAction.Allow
                : FirewallAction.Block;
            set => Firewall.UnderlyingObject.set_DefaultOutboundAction(
                _profile,
                value == FirewallAction.Allow
                    ? NET_FW_ACTION.NET_FW_ACTION_ALLOW
                    : NET_FW_ACTION.NET_FW_ACTION_BLOCK
            );
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is enable and available
        /// </summary>
        public bool Enable
        {
            get => Firewall.UnderlyingObject.get_FirewallEnabled(_profile);
            set => Firewall.UnderlyingObject.set_FirewallEnabled(_profile, value);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is the currently active profile.
        /// </summary>
        public bool IsActive
        {
            get => (NET_FW_PROFILE_TYPE2)Firewall.UnderlyingObject.CurrentProfileTypes ==
                   NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_ALL ||
                   // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                   ((NET_FW_PROFILE_TYPE2)Firewall.UnderlyingObject.CurrentProfileTypes & _profile) == _profile;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets a value indicating if the user should get notifications about rules of this profile
        /// </summary>
        public bool ShowNotifications
        {
            get => !Firewall.UnderlyingObject.get_NotificationsDisabled(_profile);
            set => Firewall.UnderlyingObject.set_NotificationsDisabled(_profile, !value);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a FirewallProfiles showing the type of this firewall profile
        /// </summary>
        public FirewallProfiles Type
        {
            get
            {
                if (_profile == NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_ALL)
                {
                    throw new NotSupportedException();
                }

                return (FirewallProfiles) _profile;
            }
        }


        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets a value indicating if the firewall should send unicast responses to the multicast broadcasts
        /// </summary>
        public bool UnicastResponsesToMulticastBroadcast
        {
            get => !Firewall.UnderlyingObject.get_UnicastResponsesToMulticastBroadcastDisabled(_profile);
            set => Firewall.UnderlyingObject.set_UnicastResponsesToMulticastBroadcastDisabled(_profile, !value);
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