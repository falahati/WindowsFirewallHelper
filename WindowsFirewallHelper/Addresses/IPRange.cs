using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc cref="IAddress" />
    /// <summary>
    ///     A class representing a range of Internet Protocol addresses
    /// </summary>
    public class IPRange : IAddress, IEquatable<IPRange>
    {
        private int? _hashCode;

        /// <summary>
        ///     Creates an instance of the IPRange class using the provided values as the start and the end of the IP range
        /// </summary>
        /// <param name="address1">One end of the range</param>
        /// <param name="address2">Other end of the range</param>
        /// <exception cref="ArgumentException">Addresses should be of a same family</exception>
        public IPRange(IPAddress address1, IPAddress address2)
        {
            if (address1.AddressFamily != address2.AddressFamily)
            {
                throw new ArgumentException("Addresses of different family can not be used.");
            }

            if ((address1.Equals(IPAddress.Any) ||
                 address1.Equals(IPAddress.IPv6Any) ||
                 address2.Equals(IPAddress.Any) ||
                 address2.Equals(IPAddress.IPv6Any)) &&
                !address1.Equals(address2))
            {
                throw new ArgumentException("Address ranges starting or ending in `Any` are not supported.");
            }

            StartAddress = AddressHelper.Min(address1, address2);
            EndAddress = AddressHelper.Max(address1, address2);
        }


        /// <summary>
        ///     Creates an instance of the IPRange class using the provided value as both the start and the end of the IP range
        /// </summary>
        /// <param name="address">Both end of the range</param>
        /// <exception cref="ArgumentException">Addresses should be of a same family</exception>
        public IPRange(IPAddress address) : this(address, address)
        {
        }


        /// <summary>
        ///     Gets and sets the upper bound of the range
        /// </summary>
        public IPAddress EndAddress { get; set; }

        /// <summary>
        ///     Gets and sets the lower bound of the range
        /// </summary>
        public IPAddress StartAddress { get; set; }

        /// <summary>
        ///     Returns a string that represents the current <see cref="IPRange" />.
        /// </summary>
        /// <returns>
        ///     A string that represents the current <see cref="IPRange" />.
        /// </returns>
        public override string ToString()
        {
            if (StartAddress.Equals(EndAddress))
            {
                if (StartAddress.Equals(IPAddress.Any) || StartAddress.Equals(IPAddress.IPv6Any))
                {
                    return "*";
                }

                return StartAddress.ToString();
            }

            return $"{StartAddress}-{EndAddress}";
        }

        /// <summary>
        ///     Compares two IP address ranges.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two address ranges are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="other">An <see cref="IPRange" /> instance to compare to the current instance. </param>
        public bool Equals(IPRange other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return StartAddress.Equals(other.StartAddress) && EndAddress.Equals(other.EndAddress);
        }

        /// <summary>
        ///     Compares two <see cref="IPRange" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="IPRange" /> object</param>
        /// <param name="right">A <see cref="IPRange" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(IPRange left, IPRange right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="IPRange" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="IPRange" /> object</param>
        /// <param name="right">A <see cref="IPRange" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(IPRange left, IPRange right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Converts an address string to a <see cref="IPRange" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="IPRange" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public static IPRange Parse(string str)
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
        ///     Determines whether a string is a valid IP address range.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid IP address range; otherwise, <see langword="false" />
        ///     .
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="addressRange">The <see cref="IPRange" /> version of the string.</param>
        public static bool TryParse(string str, out IPRange addressRange)
        {
            try
            {
                if (str == "*")
                {
                    addressRange = new IPRange(IPAddress.Any);

                    return true;
                }

                var ips = str.Split('-');

                if (ips.Length == 1)
                {
                    if (IPAddress.TryParse(ips[0], out var address))
                    {
                        addressRange = new IPRange(address);

                        return true;
                    }
                }
                else if (ips.Length == 2)
                {
                    if (IPAddress.TryParse(ips[0], out var address1) && IPAddress.TryParse(ips[1], out var address2))
                    {
                        addressRange = new IPRange(address1, address2);

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            addressRange = null;

            return false;
        }

        /// <summary>
        ///     Compares two IP address ranges.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two address ranges are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="obj">An <see cref="T:Object" /> instance to compare to the current instance. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as IPRange);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="IPRange" />.
        /// </returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = ((StartAddress?.GetHashCode() ?? 0) * 397) ^ (EndAddress?.GetHashCode() ?? 0);
                }
            }

            return _hashCode.Value;
        }
    }
}