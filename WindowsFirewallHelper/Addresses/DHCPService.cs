using System;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc />
    /// <summary>
    ///     A class representing a DHCP service as an address
    /// </summary>
    public sealed class DHCPService : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString
        {
            get => @"DHCP";
        }

        /// <summary>
        ///     Converts an address string to a <see cref="DHCPService" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="DHCPService" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public new static DHCPService Parse(string str)
        {
            return Parse<DHCPService>(str);
        }

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