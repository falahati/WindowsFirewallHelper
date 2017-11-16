namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing a WINS service as an address
    /// </summary>
    public sealed class WINSService : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString => "WINS";

        /// <summary>
        ///     Determines whether a string is a valid WINS service
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid WINS service; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="service">The <see cref="WINSService" /> instance that represents the passed string.</param>
        public static bool TryParse(string str, out WINSService service)
        {
            return TryParse<WINSService>(str, out service);
        }
    }
}