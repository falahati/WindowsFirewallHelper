namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc />
    /// <summary>
    ///     A class representing default gateway as an address
    /// </summary>
    public sealed class DefaultGateway : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString
        {
            get => @"Defaultgateway";
        }

        /// <summary>
        ///     Determines whether a string is a valid default gateway address
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid default gateway address; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="address">The <see cref="DefaultGateway" /> instance that represents the passed string.</param>
        public static bool TryParse(string str, out DefaultGateway address)
        {
            return TryParse<DefaultGateway>(str, out address);
        }
    }
}