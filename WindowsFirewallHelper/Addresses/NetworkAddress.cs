using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A Class representing a range of Internet Protocol addresses using an <see cref="IPAddress" /> and a Subnet Mask
    /// </summary>
    public class NetworkAddress : IAddress
    {
        private int? _hashCode;

        /// <summary>
        ///     Returns the single host subnet for IPv4 IPs (255.255.255.255)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly IPAddress IPv4SingleHostSubnet = IPAddress.Parse("255.255.255.255");

        /// <summary>
        ///     Returns the single host subnet for IPv6 IPs (ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly IPAddress IPv6SingleHostSubnet = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");

        /// <summary>
        ///     Creates an instance of the <see cref="NetworkAddress" /> class using an <see cref="IPAddress" /> and
        ///     255.255.255.255 or ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff as the Subnet Mask
        /// </summary>
        /// <param name="address">IPAddress to create an instance of <see cref="NetworkAddress" /> with</param>
        public NetworkAddress(IPAddress address)
        {
            switch (address.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    SubnetMask = IPv4SingleHostSubnet;
                    break;
                case AddressFamily.InterNetworkV6:
                    SubnetMask = IPv6SingleHostSubnet;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Address = address;
        }

        /// <summary>
        ///     Creates an instance of the <see cref="NetworkAddress" /> class using an <see cref="IPAddress" /> and a Subnet Mask
        /// </summary>
        /// <param name="address">
        ///     <see cref="IPAddress" /> to be used as the base IPAddress of the <see cref="NetworkAddress" />
        /// </param>
        /// <param name="subnetMask">
        ///     <see cref="IPAddress" /> to be used as the Subnet Mask of the <see cref="NetworkAddress" />
        /// </param>
        /// <exception cref="ArgumentException">Addresses should be of a same family</exception>
        public NetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            if (address.AddressFamily != subnetMask.AddressFamily)
                throw new ArgumentException("Addresses of different family can not be used.");
            Address = address;
            SubnetMask = subnetMask;
        }

        /// <summary>
        ///     Gets or sets the base <see cref="IPAddress" /> in which this <see cref="NetworkAddress" /> is based on
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        ///     Gets the calculated upper bound of the range
        /// </summary>
        public IPAddress EndAddress
        {
            get
            {
                if (SubnetMask.Equals(IPv4SingleHostSubnet) || SubnetMask.Equals(IPv6SingleHostSubnet))
                    return Address;
                var addressBytes = Address.GetAddressBytes();
                var subnetMaskBytes = SubnetMask.GetAddressBytes();
                for (var i = 0; i < addressBytes.Length; i++)
                    addressBytes[i] |= (byte) (subnetMaskBytes[i] ^ 0xFF);
                addressBytes[addressBytes.Length - 1] &= 0xFE;
                return new IPAddress(addressBytes);
            }
        }

        /// <summary>
        ///     Gets the calculated lower bound of the range
        /// </summary>
        public IPAddress StartAddress
        {
            get
            {
                if (SubnetMask.Equals(IPv4SingleHostSubnet) || SubnetMask.Equals(IPv6SingleHostSubnet))
                    return Address;
                var addressBytes = Address.GetAddressBytes();
                var subnetMaskBytes = SubnetMask.GetAddressBytes();
                for (var i = 0; i < addressBytes.Length; i++)
                    addressBytes[i] &= subnetMaskBytes[i];
                addressBytes[addressBytes.Length - 1] |= 0x01;
                return new IPAddress(addressBytes);
            }
        }

        /// <summary>
        ///     Gets or sets the Subnet Mask in which this <see cref="NetworkAddress" /> is based on
        /// </summary>
        public IPAddress SubnetMask { get; set; }


        /// <summary>
        ///     Returns a string that represents the current <see cref="NetworkAddress" />.
        /// </summary>
        /// <returns>
        ///     A string that represents the current <see cref="NetworkAddress" />.
        /// </returns>
        public override string ToString()
        {
            if (StartAddress.Equals(EndAddress))
                return Address.ToString();
            return $"{Address}/{SubnetMask}";
        }

        /// <summary>
        ///     Determines whether a string is a valid network address.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid network address; otherwise, <see langword="false" />
        ///     .
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="addressNetwork">The <see cref="NetworkAddress" /> version of the string.</param>
        public static bool TryParse(string str, out NetworkAddress addressNetwork)
        {
            var ips = str.Split('/');
            if (ips.Length == 1)
            {
                IPAddress address;
                if (IPAddress.TryParse(ips[0], out address))
                {
                    addressNetwork = new NetworkAddress(address);
                    return true;
                }
            }
            else if (ips.Length == 2)
            {
                IPAddress address1;
                if (IPAddress.TryParse(ips[0], out address1))
                {
                    int netmask;
                    if (int.TryParse(ips[1], out netmask) && (netmask >= 1))
                        if (((address1.AddressFamily == AddressFamily.InterNetwork) && (netmask <= 32)) ||
                            ((address1.AddressFamily == AddressFamily.InterNetworkV6) && (netmask <= 128)))
                        {
                            var bytes = new byte[address1.AddressFamily == AddressFamily.InterNetworkV6 ? 16 : 4];
                            for (byte i = 0; i < netmask; i++)
                                bytes[(int) Math.Floor(i/8d)] |= (byte) (1 << (7 - i%8));
                            addressNetwork = new NetworkAddress(address1, new IPAddress(bytes));
                            return true;
                        }
                    IPAddress address2;
                    if (IPAddress.TryParse(ips[1], out address2))
                    {
                        addressNetwork = new NetworkAddress(address1, address2);
                        return true;
                    }
                }
            }
            addressNetwork = null;
            return false;
        }

        /// <summary>
        ///     Compares two network addresses.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two network address are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="obj">An <see cref="T:Object" /> instance to compare to the current instance. </param>
        public override bool Equals(object obj)
        {
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (obj is NetworkAddress)
                return Equals((NetworkAddress) obj);
            return ReferenceEquals(this, obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="NetworkAddress" />.
        /// </returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            if (_hashCode == null)
                unchecked
                {
                    _hashCode = ((Address?.GetHashCode() ?? 0)*397) ^ (SubnetMask?.GetHashCode() ?? 0);
                }
            return _hashCode.Value;
        }

        /// <summary>
        ///     Compares two network address.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two network address are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="comparand">An <see cref="NetworkAddress" /> instance to compare to the current instance. </param>
        protected bool Equals(NetworkAddress comparand)
        {
            return Address.Equals(comparand.Address) && SubnetMask.Equals(comparand.SubnetMask);
        }
    }
}