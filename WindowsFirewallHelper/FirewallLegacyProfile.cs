using WindowsFirewallHelper.Collections;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Exceptions;

namespace WindowsFirewallHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Contains properties of a Windows Firewall v1 profile
    /// </summary>
    public class FirewallLegacyProfile : IFirewallProfile
    {
        private readonly FirewallLegacy _firewall;

        internal FirewallLegacyProfile(FirewallLegacy firewall, FirewallProfiles profileType)
        {
            var localPolicy = firewall.UnderlyingObject.LocalPolicy;
            UnderlyingObject = localPolicy.GetProfileByType(GetNativeProfileType(profileType));
            _firewall = firewall;
        }

        /// <summary>
        ///     Gets the list of all firewall rules for this profile
        /// </summary>
        public IFirewallLegacyRulesCollection Rules
        {
            get => new FirewallLegacyRulesCollection(new[] {this});
        }

        internal INetFwProfile UnderlyingObject { get; }

        /// <inheritdoc />
        bool IFirewallProfile.BlockAllInboundTraffic
        {
            get => UnderlyingObject.ExceptionsNotAllowed;
            set => UnderlyingObject.ExceptionsNotAllowed = value;
        }

        /// <inheritdoc />
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     Setting a value for this
        ///     property is not supported
        /// </exception>
        FirewallAction IFirewallProfile.DefaultInboundAction
        {
            get => FirewallAction.Block;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="T:WindowsFirewallHelper.FirewallAPIv1.FirewallAPIv1NotSupportedException">
        ///     Setting a value for this
        ///     property is not supported
        /// </exception>
        FirewallAction IFirewallProfile.DefaultOutboundAction
        {
            get => FirewallAction.Allow;
            set => throw new FirewallLegacyNotSupportedException();
        }

        /// <inheritdoc />
        public bool Enable
        {
            get => UnderlyingObject.FirewallEnabled;
            set => UnderlyingObject.FirewallEnabled = value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a Boolean value showing if this firewall profile is the currently active profile.
        /// </summary>
        public bool IsActive
        {
            get => _firewall?.UnderlyingObject?.CurrentProfileType == UnderlyingObject.Type;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets a value indicating if the user should get notifications about rules of this profile
        /// </summary>
        public bool ShowNotifications
        {
            get => !UnderlyingObject.NotificationsDisabled;
            set => UnderlyingObject.NotificationsDisabled = !value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a FirewallProfiles showing the type of this firewall profile
        /// </summary>
        public FirewallProfiles Type
        {
            get => GetManagedProfileType(UnderlyingObject.Type);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets a value indicating if the firewall should send unicast responses to the multicast broadcasts
        /// </summary>
        public bool UnicastResponsesToMulticastBroadcast
        {
            get => !UnderlyingObject.UnicastResponsesToMulticastBroadcastDisabled;
            set => UnderlyingObject.UnicastResponsesToMulticastBroadcastDisabled = !value;
        }

        private static FirewallProfiles GetManagedProfileType(NetFwProfileType profile)
        {
            switch (profile)
            {
                case NetFwProfileType.Domain:

                    return FirewallProfiles.Domain;
                case NetFwProfileType.Standard:

                    return FirewallProfiles.Private;
                default:

                    throw new FirewallLegacyNotSupportedException();
            }
        }

        private static NetFwProfileType GetNativeProfileType(FirewallProfiles profile)
        {
            switch (profile)
            {
                case FirewallProfiles.Domain:

                    return NetFwProfileType.Domain;
                case FirewallProfiles.Private:

                    return NetFwProfileType.Standard;
                default:

                    throw new FirewallLegacyNotSupportedException();
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
            try
            {
                return Type.ToString();
            }
            // ReSharper disable once CatchAllClause
            catch
            {
                return base.ToString();
            }
        }
    }
}