using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing a range of Internet Protocol addresses
    /// </summary>
    public class IPRange : IAddress
    {
        private int? _hashCode;

        /// <summary>
        ///     Creates an instance of the IPRange class using the provided values as the start and end of the IP range
        /// </summary>
        /// <param name="address1">One end of the range</param>
        /// <param name="address2">Other end of the range</param>
        /// <exception cref="ArgumentException">Addresses should be of a same family</exception>
        public IPRange(IPAddress address1, IPAddress address2)
        {
            if (address1.AddressFamily != address2.AddressFamily)
                throw new ArgumentException("Addresses of different family can not be used.");
            if (address1.Equals(address2))
                throw new ArgumentException("Both addresses are same.");
            StartAddress = AddressHelper.Min(address1, address2);
            EndAddress = AddressHelper.Max(address1, address2);
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
            return $"{StartAddress}-{EndAddress}";
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
            var ips = str.Split('-');
            if (ips.Length == 2)
            {
                IPAddress address1;
                IPAddress address2;
                if (IPAddress.TryParse(ips[0], out address1) && IPAddress.TryParse(ips[1], out address2))
                    if (!address1.Equals(address2))
                    {
                        addressRange = new IPRange(address1, address2);
                        return true;
                    }
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
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (obj is IPRange)
                return Equals((IPRange) obj);
            return ReferenceEquals(this, obj);
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
                unchecked
                {
                    _hashCode = ((StartAddress?.GetHashCode() ?? 0)*397) ^ (EndAddress?.GetHashCode() ?? 0);
                }
            return _hashCode.Value;
        }

        /// <summary>
        ///     Compares two IP address ranges.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two address ranges are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="comparand">An <see cref="IPRange" /> instance to compare to the current instance. </param>
        private bool Equals(IPRange comparand)
        {
            return StartAddress.Equals(comparand.StartAddress) && EndAddress.Equals(comparand.EndAddress);
        }
    }
}