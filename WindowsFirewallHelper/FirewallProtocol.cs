using System;
using System.Diagnostics.CodeAnalysis;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     A class representing a FirewallProtocol
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FirewallProtocol : IEquatable<FirewallProtocol>, IEquatable<int>
    {
        /// <summary>
        ///     Any Protocol
        /// </summary>
        public static FirewallProtocol Any = new FirewallProtocol(256);

        /// <summary>
        ///     Generic Routing Encapsulation Protocol
        /// </summary>
        public static FirewallProtocol GRE = new FirewallProtocol(47);

        /// <summary>
        ///     Hop-by-Hop Option Protocol
        /// </summary>
        public static FirewallProtocol HOPOPT = new FirewallProtocol(0);

        /// <summary>
        ///     Internet Control Message Protocol for IPv4
        /// </summary>
        public static FirewallProtocol ICMPv4 = new FirewallProtocol(1);

        /// <summary>
        ///     Internet Control Message Protocol for IPv6
        /// </summary>
        public static FirewallProtocol ICMPv6 = new FirewallProtocol(58);

        /// <summary>
        ///     Internet Group Management Protocol
        /// </summary>
        public static FirewallProtocol IGMP = new FirewallProtocol(2);

        /// <summary>
        ///     Internet Protocol Version 6
        /// </summary>
        public static FirewallProtocol IPv6 = new FirewallProtocol(41);

        /// <summary>
        ///     Internet Protocol Version 6 Fragmentation Header
        /// </summary>
        public static FirewallProtocol IPv6Frag = new FirewallProtocol(44);

        /// <summary>
        ///     Internet Protocol Version 6 No Next Header
        /// </summary>
        public static FirewallProtocol IPv6NoNxt = new FirewallProtocol(59);

        /// <summary>
        ///     Internet Protocol Version 6 Options Header
        /// </summary>
        public static FirewallProtocol IPv6Opts = new FirewallProtocol(60);

        /// <summary>
        ///     Internet Protocol Version 6 Route Header
        /// </summary>
        public static FirewallProtocol IPv6Route = new FirewallProtocol(43);

        /// <summary>
        ///     Layer 2 Tunneling Protocol
        /// </summary>
        public static FirewallProtocol L2TP = new FirewallProtocol(115);

        /// <summary>
        ///     Pragmatic General Multicast Protocol
        /// </summary>
        public static FirewallProtocol PGM = new FirewallProtocol(113);

        /// <summary>
        ///     Transmission Control Protocol
        /// </summary>
        public static FirewallProtocol TCP = new FirewallProtocol(6);

        /// <summary>
        ///     User Datagram Protocol
        /// </summary>
        public static FirewallProtocol UDP = new FirewallProtocol(17);

        /// <summary>
        ///     Virtual Router Redundancy Protocol
        /// </summary>
        public static FirewallProtocol VRRP = new FirewallProtocol(112);

        /// <summary>
        ///     Creates a new <see cref="FirewallProtocol" /> based on the protocol number
        /// </summary>
        /// <param name="protocolNumber">The protocol number to create <see cref="FirewallProtocol" /> from</param>
        public FirewallProtocol(byte protocolNumber)
        {
            ProtocolNumber = protocolNumber;
        }

        internal FirewallProtocol(int protocolNumber)
        {
            ProtocolNumber = protocolNumber;
        }

        /// <summary>
        ///     Returns the underlying protocol number of this object
        /// </summary>
        public int ProtocolNumber { get; }

        /// <summary>
        ///     Determines whether the specified<see cref="FirewallProtocol" /> is equal to the current
        ///     <see cref="FirewallProtocol" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="FirewallProtocol" /> is equal to the current <see cref="FirewallProtocol" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(FirewallProtocol other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.ProtocolNumber);
        }

        /// <summary>
        ///     Determines whether the specified protocol number is equal to the current
        ///     <see cref="FirewallProtocol" />.
        /// </summary>
        /// <param name="other"> The protocol number to compare with the current object.</param>
        /// <returns>
        ///     true if the specified protocol number is equal to the current <see cref="FirewallProtocol" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(int other)
        {
            return ProtocolNumber == other;
        }

        public static bool operator ==(FirewallProtocol left, FirewallProtocol right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        public static bool operator ==(FirewallProtocol left, int right)
        {
            return left?.Equals(right) == true;
        }

        public static bool operator ==(int left, FirewallProtocol right)
        {
            return right?.Equals(left) == true;
        }

        public static bool operator !=(FirewallProtocol left, FirewallProtocol right)
        {
            return !(left == right);
        }

        public static bool operator !=(FirewallProtocol left, int right)
        {
            return !(left == right);
        }

        public static bool operator !=(int left, FirewallProtocol right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Tries to create a <see cref="FirewallProtocol" /> object from the the string
        /// </summary>
        /// <param name="str">The string to be analyzed</param>
        /// <param name="firewallProtocol">Returning <see cref="FirewallProtocol" /> object</param>
        /// <returns>
        ///     <see langword="true" /> if process ends well and <see cref="FirewallProtocol" /> created; otherwise
        ///     <see langword="false" />
        /// </returns>
        public static bool TryParse(string str, out FirewallProtocol firewallProtocol)
        {
            if (int.TryParse(str, out var i) && i >= 0 && i <= 256)
            {
                firewallProtocol = new FirewallProtocol(i);

                return true;
            }

            firewallProtocol = null;

            return false;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="FirewallProtocol" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="FirewallProtocol" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj is int i)
            {
                return ProtocolNumber.Equals(i);
            }

            return Equals(obj as FirewallProtocol);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="FirewallProtocol" />.
        /// </returns>
        public override int GetHashCode()
        {
            return ProtocolNumber.GetHashCode();
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="FirewallProtocol" />.
        /// </summary>
        /// <returns>
        ///     A string that represents the current <see cref="FirewallProtocol" />.
        /// </returns>
        public override string ToString()
        {
            return ProtocolNumber.ToString();
        }
    }
}