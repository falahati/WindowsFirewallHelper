using System;

namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing local subnet as an address
    /// </summary>
    public sealed class LocalSubnet : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString
        {
            get => @"LocalSubnet";
        }

        /// <summary>
        ///     Converts an address string to a <see cref="LocalSubnet" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="LocalSubnet" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public new static LocalSubnet Parse(string str)
        {
            return Parse<LocalSubnet>(str);
        }

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