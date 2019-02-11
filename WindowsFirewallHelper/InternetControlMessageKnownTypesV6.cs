namespace WindowsFirewallHelper
{
    /// <summary>
    ///     List of known ICM types of Internet Control Message Protocol version 6 (ICMPv6)
    /// </summary>
    public enum InternetControlMessageKnownTypesV6 : byte
    {
        /// <summary>
        ///     Destination Unreachable Message
        /// </summary>
        DestinationUnreachable = 1,

        /// <summary>
        ///     Packet Too Big Message
        /// </summary>
        PacketTooBig = 2,

        /// <summary>
        ///     Time Exceeded Message
        /// </summary>
        TimeExceeded = 3,

        /// <summary>
        ///     Parameter Problem Message
        /// </summary>
        ParameterProblem = 4,

        /// <summary>
        ///     Echo Request Message
        /// </summary>
        EchoRequest = 128,

        /// <summary>
        ///     Echo Reply Message
        /// </summary>
        EchoReply = 129,

        /// <summary>
        ///     Multicast Listener Query Message
        /// </summary>
        MulticastListenerQuery = 130,

        /// <summary>
        ///     Multicast Listener Report Message
        /// </summary>
        MulticastListenerReport = 131,

        /// <summary>
        ///     Multicast Listener Done Message
        /// </summary>
        MulticastListenerDone = 132,

        /// <summary>
        ///     Router Solicitation Message
        /// </summary>
        RouterSolicitation = 133,

        /// <summary>
        ///     Router Advertisement Message
        /// </summary>
        RouterAdvertisement = 134,

        /// <summary>
        ///     Neighbor Solicitation Message
        /// </summary>
        NeighborSolicitation = 135,

        /// <summary>
        ///     Neighbor Advertisement Message
        /// </summary>
        NeighborAdvertisement = 136,

        /// <summary>
        ///     Redirect Message
        /// </summary>
        Redirect = 137,

        /// <summary>
        ///     Router Renumbering Message
        /// </summary>
        RouterRenumbering = 138,

        /// <summary>
        ///     ICMP Node Information Query Message
        /// </summary>
        ICMPNodeInformationQuery = 139,

        /// <summary>
        ///     ICMP Node Information Response Message
        /// </summary>
        ICMPNodeInformationResponse = 140,

        /// <summary>
        ///     Inverse Neighbor Discovery Solicitation Message
        /// </summary>
        InverseNeighborDiscoverySolicitation = 141,

        /// <summary>
        ///     Inverse Neighbor Discovery Advertisement Message
        /// </summary>
        InverseNeighborDiscoveryAdvertisement = 142,

        /// <summary>
        ///     Multicast Listener Report Version 2 Message
        /// </summary>
        MulticastListenerReportVersion2 = 143,

        /// <summary>
        ///     Home Agent Address Discovery Request Message
        /// </summary>
        HomeAgentAddressDiscoveryRequest = 144,

        /// <summary>
        ///     Home Agent Address Discovery Reply Message
        /// </summary>
        HomeAgentAddressDiscoveryReply = 145,

        /// <summary>
        ///     Mobile Prefix Solicitation Message
        /// </summary>
        MobilePrefixSolicitation = 146,

        /// <summary>
        ///     Mobile Prefix Advertisement Message
        /// </summary>
        MobilePrefixAdvertisement = 147,

        /// <summary>
        ///     Certification Path Solicitation Message
        /// </summary>
        CertificationPathSolicitation = 148,

        /// <summary>
        ///     Certification Path Advertisement Message
        /// </summary>
        CertificationPathAdvertisement = 149,

        /// <summary>
        ///     Multicast Router Advertisement Message
        /// </summary>
        MulticastRouterAdvertisement = 151,

        /// <summary>
        ///     Multicast Router Solicitation Message
        /// </summary>
        MulticastRouterSolicitation = 152,

        /// <summary>
        ///     Multicast Router Termination Message
        /// </summary>
        MulticastRouterTermination = 153,

        /// <summary>
        ///     Fast Handover for Mobile Internet Protocol (FMIPv6) Messages
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FMIPv6Messages = 154,

        /// <summary>
        ///     'Routing Protocol for Low-Power and Lossy Networks' Control Message
        /// </summary>
        RPLControl = 155,

        /// <summary>
        ///     Locator Network Protocol (ILNPv6) Locator Update Message
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ILNPv6LocatorUpdate = 156,

        /// <summary>
        ///     Duplicate Address Request Message
        /// </summary>
        DuplicateAddressRequest = 157,

        /// <summary>
        ///     Duplicate Address Confirmation Message
        /// </summary>
        DuplicateAddressConfirmation = 158,

        /// <summary>
        ///     'Multicast Protocol for Low-Power and Lossy' Control Message
        /// </summary>
        MPLControl = 159
    }
}