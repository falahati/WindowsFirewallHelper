using System;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc cref="IAddress" />
    /// <summary>
    ///     This class is the parent class of all special address values
    /// </summary>
    public abstract class SpecialAddress : IAddress, IEquatable<SpecialAddress>, IEquatable<string>
    {
        /// <summary>
        ///     Should returns the constant value of the special address
        /// </summary>
        protected abstract string AddressString { get; }

        /// <inheritdoc cref="IAddress" />
        public override string ToString()
        {
            return AddressString;
        }

        /// <inheritdoc />
        public bool Equals(SpecialAddress other)
        {
            if (other == null)
            {
                return false;
            }

            return ReferenceEquals(this, other) || Equals(other.AddressString);
        }

        /// <inheritdoc />
        public bool Equals(string other)
        {
            if (other == null)
            {
                return false;
            }

            return AddressString.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Compares two <see cref="SpecialAddress" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="SpecialAddress" /> object</param>
        /// <param name="right">A <see cref="SpecialAddress" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(SpecialAddress left, SpecialAddress right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="SpecialAddress" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="SpecialAddress" /> object</param>
        /// <param name="right">A <see cref="SpecialAddress" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(SpecialAddress left, SpecialAddress right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Converts an address string to a <see cref="SpecialAddress" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="SpecialAddress" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public static SpecialAddress Parse(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (!TryParse(str, out var address))
            {
                throw new FormatException();
            }

            return address;
        }

        /// <summary>
        ///     Determines whether a string is a valid special service address.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid special service address; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="specialAddress">The <see cref="SpecialAddress" /> version of the string.</param>
        public static bool TryParse(string str, out SpecialAddress specialAddress)
        {
            if (DNSService.TryParse(str, out var dns))
            {
                specialAddress = dns;

                return true;
            }

            if (DHCPService.TryParse(str, out var dhcp))
            {
                specialAddress = dhcp;

                return true;
            }

            if (WINSService.TryParse(str, out var wins))
            {
                specialAddress = wins;

                return true;
            }

            if (LocalSubnet.TryParse(str, out var localSubnet))
            {
                specialAddress = localSubnet;

                return true;
            }

            if (DefaultGateway.TryParse(str, out var defaultGateway))
            {
                specialAddress = defaultGateway;

                return true;
            }

            specialAddress = null;

            return false;
        }

        /// <summary>
        ///     Converts an address string to an instance of one of children of the <see cref="SpecialAddress" /> class.
        /// </summary>
        /// <returns>
        ///     An instance from one of children of the <see cref="SpecialAddress" /> class.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        protected static T Parse<T>(string str) where T : SpecialAddress, new()
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (!TryParse<T>(str, out var address))
            {
                throw new FormatException();
            }

            return address;
        }

        /// <summary>
        ///     Determines whether a string is a valid address
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid address; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="service">The instance that represents the passed string.</param>
        protected static bool TryParse<T>(string str, out T service) where T : SpecialAddress, new()
        {
            service = new T();

            if (str.Trim().Equals(service.AddressString, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            service = null;

            return false;
        }

        /// <summary>
        ///     Compares address instances.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="obj">An <see cref="T:Object" /> instance to compare to the current instance. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as SpecialAddress) || Equals(obj as string);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current instance.
        /// </returns>
        public override int GetHashCode()
        {
            return AddressString?.GetHashCode() ?? 0;
        }
    }
}