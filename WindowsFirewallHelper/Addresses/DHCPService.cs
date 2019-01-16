namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing a DHCP service as an address
    /// </summary>
    public sealed class DHCPService : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString => "DHCP";

        /// <summary>
        ///     Determines whether a string is a valid DHCP service
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid DHCP service; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="service">The <see cref="DHCPService" /> instance that represents the passed string.</param>
        public static bool TryParse(string str, out DHCPService service)
        {
            return TryParse<DHCPService>(str, out service);
        }
    }
}