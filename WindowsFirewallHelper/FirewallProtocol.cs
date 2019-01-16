using System;
using System.Diagnostics.CodeAnalysis;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     A class representing a FirewallProtocol
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierWordIsNotInDictionary")]
    public class FirewallProtocol : IEquatable<FirewallProtocol>
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
        ///     true if the specified <see cref="FirewallProtocol" /> is equal to the current<see cref="FirewallProtocol" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(FirewallProtocol other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ProtocolNumber == other.ProtocolNumber;
        }


        /// <summary>
        ///     Compares two <see cref="FirewallProtocol" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallProtocol" /> object</param>
        /// <param name="right">A <see cref="FirewallProtocol" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallProtocol left, FirewallProtocol right)
        {
            return (((object) left != null) && ((object) right != null) && left.Equals(right)) ||
                   (((object) left == null) && ((object) right == null));
        }

        /// <summary>
        ///     Compares two <see cref="FirewallProtocol" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallProtocol" /> object</param>
        /// <param name="right">A <see cref="FirewallProtocol" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallProtocol left, FirewallProtocol right)
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
            int i;
            if (int.TryParse(str, out i) && (i >= 0) && (i <= 256))
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (obj is FirewallProtocol)
                return Equals((FirewallProtocol) obj);
            if (obj is int)
                return ProtocolNumber.Equals((int) obj);
            return Equals(this, obj);
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