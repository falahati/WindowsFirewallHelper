using System;
using System.ComponentModel;
using System.Net;

namespace WindowsFirewallHelper.Addresses
{
    /// <inheritdoc cref="IAddress" />
    /// <summary>
    ///     A class representing an Internet Protocol address
    /// </summary>
    public class SingleIP : IPAddress, IAddress, IEquatable<SingleIP>, IEquatable<IPAddress>
    {
        /// <summary>
        ///     Provides an IP address that matches any IPAddress. This field is read-only.
        /// </summary>
        public new static readonly SingleIP Any = new SingleIP(IPAddress.Any);

        /// <summary>
        ///     Provides the IP broadcast address. This field is read-only.
        /// </summary>
        public new static readonly SingleIP Broadcast = new SingleIP(IPAddress.Broadcast);

        /// <summary>
        ///     Provides an IP address that matches any IPv6 IPAddress. This field is read-only.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public new static readonly SingleIP IPv6Any = new SingleIP(IPAddress.IPv6Any);

        /// <summary>
        ///     Provides the IP loopback address. This property is read-only.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public new static readonly SingleIP IPv6Loopback = new SingleIP(IPAddress.IPv6Loopback);

        /// <summary>
        ///     Obsolete - Provides an IP address that indicates that no IPv6 IPAddress is mentioned. This property is read-only.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [Obsolete("Unrelated", true)] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)]
        // ReSharper disable once InconsistentNaming
        public new static readonly SingleIP IPv6None = new SingleIP(IPAddress.IPv6None);

        /// <summary>
        ///     Provides the IP loopback address. This field is read-only.
        /// </summary>
        public new static readonly SingleIP Loopback = new SingleIP(IPAddress.Loopback);

        /// <summary>
        ///     Obsolete - Provides an IP address that indicates that no IPAddress is mentioned. This property is read-only.
        /// </summary>
        [Obsolete("Unrelated", true)] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)]
        public new static readonly SingleIP None = new SingleIP(IPAddress.None);


        /// <summary>
        ///     Creates a new instance of the SingleIP class with IP Address passed as an integer value.
        /// </summary>
        /// <param name="newAddress">Integer value of the IP Address</param>
        public SingleIP(long newAddress) : base(newAddress)
        {
        }

        /// <summary>
        ///     Creates a new instance of the SingleIP class with IP Address passed as a <see langword="byte" /> array.
        /// </summary>
        /// <param name="address"><see langword="byte" /> array containing the IP Address</param>
        public SingleIP(byte[] address) : base(address)
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="SingleIP" /> class using the provided <see cref="IPAddress" />.
        /// </summary>
        /// <param name="ip"></param>
        public SingleIP(IPAddress ip) : this(ip.GetAddressBytes())
        {
        }


        /// <summary>
        ///     Converts an Internet address to its standard notation.
        /// </summary>
        /// <returns>
        ///     A string that contains the IP address in either IPv4 dotted-quad or in IPv6 colon-hexadecimal notation.
        /// </returns>
        public override string ToString()
        {
            if (Equals(Any) || Equals(IPv6Any))
            {
                return "*";
            }

            return base.ToString();
        }

        /// <inheritdoc />
        public bool Equals(IPAddress other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(SingleIP other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.ToIPAddress());
        }

        /// <summary>
        ///     Indicates whether the specified IP address is the loopback address.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="address" /> is the loopback address; otherwise, false.
        /// </returns>
        /// <param name="address">An IP address. </param>
        public static bool IsLoopback(SingleIP address)
        {
            return IsLoopback(address.ToIPAddress());
        }

        /// <summary>
        ///     Compares two <see cref="SingleIP" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="SingleIP" /> object</param>
        /// <param name="right">A <see cref="SingleIP" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(SingleIP left, SingleIP right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares one <see cref="SingleIP" /> object with another <see cref="IPAddress" /> object for equality
        /// </summary>
        /// <param name="left">A <see cref="SingleIP" /> object</param>
        /// <param name="right">A <see cref="IPAddress" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(SingleIP left, IPAddress right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares one <see cref="IPAddress" /> object with another <see cref="SingleIP" /> object for equality
        /// </summary>
        /// <param name="left">A <see cref="IPAddress" /> object</param>
        /// <param name="right">A <see cref="SingleIP" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(IPAddress left, SingleIP right)
        {
            return Equals(left, right) || right?.Equals(left) == true;
        }

        /// <summary>
        ///     Compares two <see cref="SingleIP" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="SingleIP" /> object</param>
        /// <param name="right">A <see cref="SingleIP" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(SingleIP left, SingleIP right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Compares one <see cref="SingleIP" /> object with another <see cref="IPAddress" /> object for equality
        /// </summary>
        /// <param name="left">A <see cref="SingleIP" /> object</param>
        /// <param name="right">A <see cref="IPAddress" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(SingleIP left, IPAddress right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Compares one <see cref="IPAddress" /> object with another <see cref="SingleIP" /> object for equality
        /// </summary>
        /// <param name="left">A <see cref="IPAddress" /> object</param>
        /// <param name="right">A <see cref="SingleIP" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(IPAddress left, SingleIP right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Converts an IP address string to an <see cref="SingleIP" /> instance.
        /// </summary>
        /// <returns>
        ///     An <see cref="SingleIP" /> instance.
        /// </returns>
        /// <param name="ipString">
        ///     A string that contains an IP address in dotted-quad notation for IPv4 and in colon-hexadecimal
        ///     notation for IPv6.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="ipString" /> is null. </exception>
        /// <exception cref="FormatException"><paramref name="ipString" /> is not a valid IP address. </exception>
        public new static SingleIP Parse(string ipString)
        {
            if (ipString == null)
            {
                throw new ArgumentNullException(nameof(ipString));
            }

            if (!TryParse(ipString, out SingleIP ip))
            {
                throw new FormatException();
            }

            return ip;
        }

        /// <summary>
        ///     NOT SUPPORTED
        /// </summary>
        public new static bool TryParse(string ipString, out IPAddress address)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        ///     Determines whether a string is a valid IP address.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="ipString" /> is a valid IP address; otherwise, false.
        /// </returns>
        /// <param name="ipString">The string to validate.</param>
        /// <param name="address">The <see cref="SingleIP" /> version of the string.</param>
        public static bool TryParse(string ipString, out SingleIP address)
        {
            // Check if this is "Any Address" special string
            if (ipString.Trim() == "*")
            {
                address = Any;

                return true;
            }

            // Check if this is a valid IPAddress
            if (IPAddress.TryParse(ipString, out var ipAddress))
            {
                address = new SingleIP(ipAddress);

                return true;
            }

            // Check if this is a IP range with only one IP
            if (IPRange.TryParse(ipString, out var ipRange) &&
                ipRange.StartAddress.Equals(ipRange.EndAddress))
            {
                address = new SingleIP(ipRange.StartAddress);

                return true;
            }

            // Check if this is a single IP NetworkAddress
            if (NetworkAddress.TryParse(ipString, out var networkAddress) &&
                networkAddress.StartAddress.Equals(networkAddress.EndAddress))
            {
                address = new SingleIP(networkAddress.Address);

                return true;
            }

            address = null;

            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return Equals(other as SingleIP) || Equals(other as IPAddress);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        ///     Cast the current <see cref="SingleIP" /> object to <see cref="IPAddress" /> and returns it.
        /// </summary>
        /// <returns>Returns the corresponding <see cref="IPAddress" /> object</returns>
        public IPAddress ToIPAddress()
        {
            return this;
        }
    }
}