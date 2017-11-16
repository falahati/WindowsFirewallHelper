using System;
using System.ComponentModel;
using System.Net;

namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     A class representing a Internet Protocol address
    /// </summary>
    public class SingleIP : IPAddress, IAddress
    {
        /// <summary>
        ///     Provides an IP address that matches any IPAddress. This field is read-only.
        /// </summary>
        public new static readonly SingleIP Any = FromIPAddress(IPAddress.Any);

        /// <summary>
        ///     Provides the IP loopback address. This field is read-only.
        /// </summary>
        public new static readonly SingleIP Loopback = FromIPAddress(IPAddress.Loopback);

        /// <summary>
        ///     Provides the IP broadcast address. This field is read-only.
        /// </summary>
        public new static readonly SingleIP Broadcast = FromIPAddress(IPAddress.Broadcast);

        /// <summary>
        ///     Provides the IP loopback address. This property is read-only.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public new static readonly SingleIP IPv6Loopback = FromIPAddress(IPAddress.IPv6Loopback);

        /// <summary>
        ///     Obsolete - Provides an IP address that indicates that no IPv6 IPAddress is mentioned. This property is read-only.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [Obsolete("Unrelated", true)] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)] public new static readonly SingleIP IPv6None = FromIPAddress(IPAddress.IPv6None);

        /// <summary>
        ///     Obsolete - Provides an IP address that indicates that no IPAddress is mentioned. This property is read-only.
        /// </summary>
        [Obsolete("Unrelated", true)] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)] public new static readonly SingleIP None = FromIPAddress(IPAddress.None);

        /// <summary>
        ///     Obsolete - Provides an IP address that matches any IPv6 IPAddress. This field is read-only.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [Obsolete("Unrelated", true)] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)] public new static readonly SingleIP IPv6Any = FromIPAddress(IPAddress.IPv6Any);


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
        ///     Converts an Internet address to its standard notation.
        /// </summary>
        /// <returns>
        ///     A string that contains the IP address in either IPv4 dotted-quad or in IPv6 colon-hexadecimal notation.
        /// </returns>
        public override string ToString()
        {
            if (Equals(Any))
                return "*";
            return base.ToString();
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="SingleIP" /> class using the provided <see cref="IPAddress" />.
        /// </summary>
        /// <param name="ip">The <see cref="IPAddress" /> to create new object from.</param>
        /// <returns>Newly created <see cref="SingleIP" /> instance</returns>
        public static SingleIP FromIPAddress(IPAddress ip)
        {
            return new SingleIP(ip.GetAddressBytes());
        }

        /// <summary>
        ///     NOT SUPPORTED
        /// </summary>
        public new static bool IsLoopback(IPAddress address)
        {
            throw new NotSupportedException();
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
            return IPAddress.IsLoopback(address.ToIPAddress());
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
                throw new ArgumentNullException(nameof(ipString));
            SingleIP ip;
            if (!TryParse(ipString, out ip))
                throw new FormatException();
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
            if (ipString.Trim() == "*")
            {
                address = Any;
                return true;
            }
            IPAddress ipAddress;
            if (IPAddress.TryParse(ipString, out ipAddress))
            {
                address = FromIPAddress(ipAddress);
                return true;
            }
            address = null;
            return false;
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