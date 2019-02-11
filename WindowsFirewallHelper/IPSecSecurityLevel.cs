namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Internet Protocol Security level
    /// </summary>
    public enum IPSecSecurityLevel
    {
        /// <summary>
        ///     No Internet Protocol Security
        /// </summary>
        None,

        /// <summary>
        ///     Internet Protocol Security with no encapsulation security payload (ESP)
        /// </summary>
        IPSecNoEncapsulation,

        /// <summary>
        ///     Internet Protocol Security with integrity protection check value (ICV)
        /// </summary>
        IPSecWithIntegrityProtection,

        /// <summary>
        ///     Internet Protocol Security with negotiation and mutual decision about the encryption level
        /// </summary>
        IPSecWithEncryptionNegotiation,

        /// <summary>
        ///     Internet Protocol Security with full encryption of data
        /// </summary>
        IPSecWithFullEncryption
    }
}