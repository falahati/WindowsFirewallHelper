using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     A class representing a FirewallProtocol
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FirewallProtocol : IEquatable<FirewallProtocol>, IEquatable<int>
    {
        /// <summary>
        ///     Matches both TCP and UDP protocols for port rules and all protocols for other type of rules
        /// </summary>
        public static readonly FirewallProtocol Any =
            new FirewallProtocol((int) NetFwIPProtocol.Any);

        /// <summary>
        ///     Generic Routing Encapsulation Protocol
        /// </summary>
        public static readonly FirewallProtocol GRE = new FirewallProtocol(47);

        /// <summary>
        ///     Hop-by-Hop Option Protocol
        /// </summary>
        public static readonly FirewallProtocol HOPOPT = new FirewallProtocol(0);

        /// <summary>
        ///     Internet Control Message Protocol for IPv4
        /// </summary>
        public static readonly FirewallProtocol ICMPv4 = new FirewallProtocol(1);

        /// <summary>
        ///     Internet Control Message Protocol for IPv6
        /// </summary>
        public static readonly FirewallProtocol ICMPv6 = new FirewallProtocol(58);

        /// <summary>
        ///     Internet Group Management Protocol
        /// </summary>
        public static readonly FirewallProtocol IGMP = new FirewallProtocol(2);

        /// <summary>
        ///     Internet Protocol Version 6
        /// </summary>
        public static readonly FirewallProtocol IPv6 = new FirewallProtocol(41);

        /// <summary>
        ///     Internet Protocol Version 6 Fragmentation Header
        /// </summary>
        public static readonly FirewallProtocol IPv6Frag = new FirewallProtocol(44);

        /// <summary>
        ///     Internet Protocol Version 6 No Next Header
        /// </summary>
        public static readonly FirewallProtocol IPv6NoNxt = new FirewallProtocol(59);

        /// <summary>
        ///     Internet Protocol Version 6 Options Header
        /// </summary>
        public static readonly FirewallProtocol IPv6Opts = new FirewallProtocol(60);

        /// <summary>
        ///     Internet Protocol Version 6 Route Header
        /// </summary>
        public static readonly FirewallProtocol IPv6Route = new FirewallProtocol(43);

        /// <summary>
        ///     Layer 2 Tunneling Protocol
        /// </summary>
        public static readonly FirewallProtocol L2TP = new FirewallProtocol(115);

        /// <summary>
        ///     Pragmatic General Multicast Protocol
        /// </summary>
        public static readonly FirewallProtocol PGM = new FirewallProtocol(113);

        /// <summary>
        ///     Transmission Control Protocol
        /// </summary>
        public static readonly FirewallProtocol TCP =
            new FirewallProtocol((int) NetFwIPProtocol.TCP);

        /// <summary>
        ///     User Datagram Protocol
        /// </summary>
        public static readonly FirewallProtocol UDP =
            new FirewallProtocol((int) NetFwIPProtocol.UDP);

        /// <summary>
        ///     Virtual Router Redundancy Protocol
        /// </summary>
        public static readonly FirewallProtocol VRRP = new FirewallProtocol(112);

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

        /// <summary>
        ///     Compares two <see cref="FirewallProtocol" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallProtocol" /> object</param>
        /// <param name="right">A <see cref="FirewallProtocol" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallProtocol left, FirewallProtocol right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares one <see cref="FirewallProtocol" /> object with another <see cref="int" /> for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallProtocol" /> object</param>
        /// <param name="right">An <see cref="int" /></param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallProtocol left, int right)
        {
            return left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares one <see cref="int" /> with another <see cref="FirewallProtocol" /> object for equality
        /// </summary>
        /// <param name="left">An <see cref="int" /></param>
        /// <param name="right">A <see cref="FirewallProtocol" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(int left, FirewallProtocol right)
        {
            return right?.Equals(left) == true;
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
        ///     Compares one <see cref="FirewallProtocol" /> object with another <see cref="int" /> for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallProtocol" /> object</param>
        /// <param name="right">An <see cref="int" /></param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallProtocol left, int right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Compares one <see cref="int" /> with another <see cref="FirewallProtocol" /> object for equality
        /// </summary>
        /// <param name="left">An <see cref="int" /></param>
        /// <param name="right">A <see cref="FirewallProtocol" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
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
        // ReSharper disable once TooManyDeclarations
        public override string ToString()
        {
            try
            {
                var knownNames = typeof(FirewallProtocol).GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Where(info => info.FieldType == typeof(FirewallProtocol))
                    .ToDictionary(
                        info => ((FirewallProtocol) info.GetValue(null)).ProtocolNumber,
                        info => info.Name
                    );

                if (knownNames.ContainsKey(ProtocolNumber))
                {
                    return knownNames[ProtocolNumber];
                }
            }
            catch
            {
                // ignored
            }

            return $"Protocol #{ProtocolNumber}";
        }
    }
}