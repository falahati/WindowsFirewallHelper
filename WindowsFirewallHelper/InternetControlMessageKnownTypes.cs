namespace WindowsFirewallHelper
{
    /// <summary>
    ///     List of known ICM types for Internet Control Message Protocol
    /// </summary>
    public enum InternetControlMessageKnownTypes : byte
    {
        /// <summary>
        ///     Echo Reply Message
        /// </summary>
        EchoReply = 0,

        /// <summary>
        ///     Destination Unreachable Message
        /// </summary>
        DestinationUnreachable = 3,

        /// <summary>
        ///     Source Quench Message
        /// </summary>
        SourceQuench = 4,

        /// <summary>
        ///     Redirect Message
        /// </summary>
        Redirect = 5,

        /// <summary>
        ///     Alternate Host Address Message
        /// </summary>
        AlternateHostAddress = 6,

        /// <summary>
        ///     Echo Message
        /// </summary>
        Echo = 8,

        /// <summary>
        ///     Router Advertisement Message
        /// </summary>
        RouterAdvertisement = 9,

        /// <summary>
        ///     Router Selection Message
        /// </summary>
        RouterSelection = 10,

        /// <summary>
        ///     Time Exceeded Message
        /// </summary>
        TimeExceeded = 11,

        /// <summary>
        ///     Parameter Problem Message
        /// </summary>
        ParameterProblem = 12,

        /// <summary>
        ///     Timestamp Message
        /// </summary>
        Timestamp = 13,

        /// <summary>
        ///     Timestamp Reply Message
        /// </summary>
        TimestampReply = 14,

        /// <summary>
        ///     Information Request Message
        /// </summary>
        InformationRequest = 15,

        /// <summary>
        ///     Information Reply Message
        /// </summary>
        InformationReply = 16,

        /// <summary>
        ///     Address Mask Request Message
        /// </summary>
        AddressMaskRequest = 17,

        /// <summary>
        ///     Address Mask Reply Message
        /// </summary>
        AddressMaskReply = 18,

        /// <summary>
        ///     Trace-route Message
        /// </summary>
        TraceRoute = 30,

        /// <summary>
        ///     Datagram Conversion Error Message
        /// </summary>
        DatagramConversionError = 31,

        /// <summary>
        ///     Mobile Host Redirect Message
        /// </summary>
        MobileHostRedirect = 32,

        /// <summary>
        ///     IPv6 'Where Are You' Message
        /// </summary>
        // ReSharper disable once InconsistentNaming
        IPv6WhereAreYou = 33,

        /// <summary>
        ///     IPv6 'I Am Here' Message
        /// </summary>
        // ReSharper disable once InconsistentNaming
        IPv6IAmHere = 34,

        /// <summary>
        ///     Mobile Registration Request Message
        /// </summary>
        MobileRegistrationRequest = 35,

        /// <summary>
        ///     Mobile Registration Reply Message
        /// </summary>
        MobileRegistrationReply = 36,

        /// <summary>
        ///     Domain Name Request Message
        /// </summary>
        DomainNameRequest = 37,

        /// <summary>
        ///     Domain Name Reply Message
        /// </summary>
        DomainNameReply = 38,

        /// <summary>
        ///     Skip Message
        /// </summary>
        Skip = 39,

        /// <summary>
        ///     Photuris Message
        /// </summary>
        Photuris = 40
    }
}