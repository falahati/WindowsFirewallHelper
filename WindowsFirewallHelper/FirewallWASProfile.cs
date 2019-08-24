using System;
using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Contains properties of a Windows Firewall with Advanced Security profile
    /// </summary>
    public class FirewallWASProfile : IFirewallProfile
    {
        private readonly FirewallWAS _firewall;
        private readonly NetFwProfileType2 _profileType;

        internal FirewallWASProfile(FirewallWAS firewall, NetFwProfileType2 profileType)
        {
            _profileType = profileType;
            _firewall = firewall;
        }


        /// <inheritdoc />
        public bool BlockAllInboundTraffic
        {
            get => _firewall.UnderlyingObject.get_BlockAllInboundTraffic(_profileType);
            set => _firewall.UnderlyingObject.set_BlockAllInboundTraffic(_profileType, value);
        }

        /// <inheritdoc />
        public FirewallAction DefaultInboundAction
        {
            get => _firewall.UnderlyingObject.get_DefaultInboundAction(_profileType) == NetFwAction.Allow
                ? FirewallAction.Allow
                : FirewallAction.Block;
            set => _firewall.UnderlyingObject.set_DefaultInboundAction(
                _profileType,
                value == FirewallAction.Allow
                    ? NetFwAction.Allow
                    : NetFwAction.Block
            );
        }

        /// <inheritdoc />
        public FirewallAction DefaultOutboundAction
        {
            get => _firewall.UnderlyingObject.get_DefaultOutboundAction(_profileType) == NetFwAction.Allow
                ? FirewallAction.Allow
                : FirewallAction.Block;
            set => _firewall.UnderlyingObject.set_DefaultOutboundAction(
                _profileType,
                value == FirewallAction.Allow
                    ? NetFwAction.Allow
                    : NetFwAction.Block
            );
        }

        /// <inheritdoc />
        public bool Enable
        {
            get => _firewall.UnderlyingObject.get_FirewallEnabled(_profileType);
            set => _firewall.UnderlyingObject.set_FirewallEnabled(_profileType, value);
        }

        /// <inheritdoc />
        public bool IsActive
        {
            get => (NetFwProfileType2) _firewall.UnderlyingObject.CurrentProfileTypes ==
                   NetFwProfileType2.All ||
                   // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                   ((NetFwProfileType2) _firewall.UnderlyingObject.CurrentProfileTypes & _profileType) == _profileType;
        }

        /// <inheritdoc />
        public bool ShowNotifications
        {
            get => !_firewall.UnderlyingObject.get_NotificationsDisabled(_profileType);
            set => _firewall.UnderlyingObject.set_NotificationsDisabled(_profileType, !value);
        }

        /// <inheritdoc />
        public FirewallProfiles Type
        {
            get
            {
                if (_profileType == NetFwProfileType2.All)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return (FirewallProfiles) _profileType;
            }
        }


        /// <inheritdoc />
        public bool UnicastResponsesToMulticastBroadcast
        {
            get => !_firewall.UnderlyingObject.get_UnicastResponsesToMulticastBroadcastDisabled(_profileType);
            set => _firewall.UnderlyingObject.set_UnicastResponsesToMulticastBroadcastDisabled(_profileType, !value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            try
            {
                return Type.ToString();
            }
            // ReSharper disable once CatchAllClause
            catch (Exception)
            {
                return base.ToString();
            }
        }
    }
}