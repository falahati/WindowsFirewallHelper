namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing local subnet as an address
    /// </summary>
    public sealed class LocalSubnet : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString => "LocalSubnet";

        /// <summary>
        ///     Determines whether a string is a valid local subnet address
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid local subnet address; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="service">The <see cref="LocalSubnet" /> instance that represents the passed string.</param>
        public static bool TryParse(string str, out LocalSubnet service)
        {
            return TryParse<LocalSubnet>(str, out service);
        }
    }
}