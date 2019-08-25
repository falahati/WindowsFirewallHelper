using System;

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
            // ReSharper disable once StringLiteralTypo
            get => @"Defaultgateway";
        }

        /// <summary>
        ///     Converts an address string to a <see cref="DefaultGateway" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="DefaultGateway" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public new static DefaultGateway Parse(string str)
        {
            return Parse<DefaultGateway>(str);
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