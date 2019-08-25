using System;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc />
    /// <summary>
    ///     A class representing a DNS service as an address
    /// </summary>
    public sealed class DNSService : SpecialAddress
    {
        /// <inheritdoc />
        protected override string AddressString
        {
            get => @"DNS";
        }

        /// <summary>
        ///     Converts an address string to a <see cref="DNSService" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="DNSService" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public new static DNSService Parse(string str)
        {
            return Parse<DNSService>(str);
        }

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