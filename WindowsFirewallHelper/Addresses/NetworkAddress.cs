using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc cref="IAddress" />
    /// <summary>
    ///     A Class representing a range of Internet Protocol addresses using an <see cref="T:System.Net.IPAddress" /> and a
    ///     Subnet Mask
    /// </summary>
    public class NetworkAddress : IAddress, IEquatable<NetworkAddress>
    {
        /// <summary>
        ///     Returns the single host subnet for IPv4 IPs (255.255.255.255)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly IPAddress IPv4SingleHostSubnet = IPAddress.Parse("255.255.255.255");

        /// <summary>
        ///     Returns the single host subnet for IPv6 IPs (ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static readonly IPAddress IPv6SingleHostSubnet =
            IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");

        private int? _hashCode;

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
            {
                throw new ArgumentException("Addresses of different family can not be used.");
            }

            switch (address.AddressFamily)
            {
                case AddressFamily.InterNetwork:

                    if (Equals(address, IPAddress.Any) && !Equals(SubnetMask, IPv4SingleHostSubnet))
                    {
                        throw new ArgumentException("Any IPAddresses are only allowed with single host sub-nets.");
                    }

                    break;
                case AddressFamily.InterNetworkV6:

                    if (Equals(address, IPAddress.IPv6Any) && !Equals(SubnetMask, IPv6SingleHostSubnet))
                    {
                        throw new ArgumentException("Any IPAddresses are only allowed with single host sub-nets.");
                    }

                    break;
                default:

                    throw new ArgumentOutOfRangeException();
            }

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
                {
                    return Address;
                }

                var addressBytes = Address.GetAddressBytes();
                var subnetMaskBytes = SubnetMask.GetAddressBytes();

                for (var i = 0; i < addressBytes.Length; i++)
                {
                    addressBytes[i] |= (byte) (subnetMaskBytes[i] ^ 0xFF);
                }

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
                {
                    return Address;
                }

                var addressBytes = Address.GetAddressBytes();
                var subnetMaskBytes = SubnetMask.GetAddressBytes();

                for (var i = 0; i < addressBytes.Length; i++)
                {
                    addressBytes[i] &= subnetMaskBytes[i];
                }

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
            {
                if (Equals(Address, IPAddress.Any) || Equals(Address, IPAddress.IPv6Any))
                {
                    return "*";
                }

                return Address.ToString();
            }

            return $"{Address}/{SubnetMask}";
        }

        /// <summary>
        ///     Compares two network address.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two network address are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="other">An <see cref="NetworkAddress" /> instance to compare to the current instance. </param>
        public bool Equals(NetworkAddress other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Address.Equals(other.Address) && SubnetMask.Equals(other.SubnetMask);
        }

        /// <summary>
        ///     Compares two <see cref="NetworkAddress" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="NetworkAddress" /> object</param>
        /// <param name="right">A <see cref="NetworkAddress" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(NetworkAddress left, NetworkAddress right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="NetworkAddress" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="NetworkAddress" /> object</param>
        /// <param name="right">A <see cref="NetworkAddress" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(NetworkAddress left, NetworkAddress right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Converts an address string to a <see cref="NetworkAddress" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="NetworkAddress" /> instance.
        /// </returns>
        /// <param name="str">
        ///     A string that contains an address
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="str" /> is not a valid address. </exception>
        public static NetworkAddress Parse(string str)
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
        ///     Determines whether a string is a valid network address.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid network address; otherwise, <see langword="false" />
        ///     .
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="addressNetwork">The <see cref="NetworkAddress" /> version of the string.</param>
        // ReSharper disable once ExcessiveIndentation
        public static bool TryParse(string str, out NetworkAddress addressNetwork)
        {
            try
            {
                if (str == "*")
                {
                    addressNetwork = new NetworkAddress(IPAddress.Any);

                    return true;
                }

                var ips = str.Split('/');

                if (ips.Length == 1)
                {
                    if (IPAddress.TryParse(ips[0], out var address))
                    {
                        addressNetwork = new NetworkAddress(address);

                        return true;
                    }
                }
                else if (ips.Length == 2)
                {
                    if (IPAddress.TryParse(ips[0], out var address1))
                    {
                        // If subnet mask
                        if (int.TryParse(ips[1], out var netMask) && netMask >= 1)
                        {
                            if (address1.AddressFamily == AddressFamily.InterNetwork && netMask <= 32 ||
                                address1.AddressFamily == AddressFamily.InterNetworkV6 && netMask <= 128)
                            {
                                var bytes = new byte[address1.AddressFamily == AddressFamily.InterNetworkV6 ? 16 : 4];

                                for (byte i = 0; i < netMask; i++)
                                {
                                    bytes[(int) Math.Floor(i / 8d)] |= (byte) (1 << (7 - i % 8));
                                }

                                addressNetwork = new NetworkAddress(address1, new IPAddress(bytes));

                                return true;
                            }
                        }

                        // If subnet
                        if ((ips[1].Contains(":") || ips[1].Contains(".")) &&
                            IPAddress.TryParse(ips[1], out var address2))
                        {
                            addressNetwork = new NetworkAddress(address1, address2);

                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
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
            return Equals(obj as NetworkAddress);
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
            {
                unchecked
                {
                    _hashCode = ((Address?.GetHashCode() ?? 0) * 397) ^ (SubnetMask?.GetHashCode() ?? 0);
                }
            }

            return _hashCode.Value;
        }
    }
}