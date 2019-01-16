namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing a DNS service as an address
    /// </summary>
    public sealed class DNSService : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString => "DNS";

        /// <summary>
        ///     Determines whether a string is a valid DNS service
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid DNS service; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="service">The <see cref="DNSService" /> instance that represents the passed string.</param>
        public static bool TryParse(string str, out DNSService service)
        {
            return TryParse<DNSService>(str, out service);
        }
    }
}