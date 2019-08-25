using System;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc />
    /// <summary>
    ///     A class representing a WINS service as an address
    /// </summary>
    public sealed class WINSService : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString
        {
            get => @"WINS";
        }

        /// <summary>
        ///     Converts an address string to a <see cref="WINSService" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="WINSService" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public new static WINSService Parse(string str)
        {
            return Parse<WINSService>(str);
        }

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