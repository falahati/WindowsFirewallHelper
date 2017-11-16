using System;

namespace WindowsFirewallHelper.FirewallAPIv2
{
    /// <summary>
    ///     A class representing a Internet Control Message (ICM) type
    /// </summary>
    public class InternetControlMessage : IEquatable<InternetControlMessage>
    {
        /// <summary>
        ///     All ICM types and codes
        /// </summary>
        public static readonly InternetControlMessage Any = new InternetControlMessage();

        private readonly byte? _code;
        private readonly byte? _type;

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified ICM type number
        /// </summary>
        /// <param name="type">ICM type number to create the instance from</param>
        public InternetControlMessage(byte type)
        {
            _type = type;
            _code = null;
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified ICM type number and type code
        /// </summary>
        /// <param name="type">ICM type number to create the instance from</param>
        /// <param name="code">ICM type code to create the instance from</param>
        public InternetControlMessage(byte type, byte code)
        {
            _type = type;
            _code = code;
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        public InternetControlMessage(InternetControlMessageKnownTypesV6 type) : this((byte) type)
        {
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type and type code
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        /// <param name="code">ICM type code to create the instance from</param>
        public InternetControlMessage(InternetControlMessageKnownTypesV6 type, byte code) : this((byte) type, code)
        {
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        public InternetControlMessage(InternetControlMessageKnownTypes type) : this((byte) type)
        {
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type and type code
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        /// <param name="code">ICM type code to create the instance from</param>
        public InternetControlMessage(InternetControlMessageKnownTypes type, byte code) : this((byte) type, code)
        {
        }

        private InternetControlMessage()
        {
            _type = null;
            _code = null;
        }

        /// <summary>
        ///     Gets the corresponding ICM type code
        /// </summary>
        public int Code => _code ?? -1;

        /// <summary>
        ///     Gets the corresponding ICM type number
        /// </summary>
        public int Type => _type ?? -1;

        /// <summary>
        ///     Determines whether the specified<see cref="InternetControlMessage" /> is equal to the current
        ///     <see cref="InternetControlMessage" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="InternetControlMessage" /> is equal to the current
        ///     <see cref="InternetControlMessage" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(InternetControlMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Type == other.Type) && (Code == other.Code);
        }

        /// <summary>
        ///     Compares two <see cref="InternetControlMessage" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="InternetControlMessage" /> object</param>
        /// <param name="right">A <see cref="InternetControlMessage" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(InternetControlMessage left, InternetControlMessage right)
        {
            return (((object) left != null) && ((object) right != null) && left.Equals(right)) ||
                   (((object) left == null) && ((object) right == null));
        }

        /// <summary>
        ///     Compares two <see cref="InternetControlMessage" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="InternetControlMessage" /> object</param>
        /// <param name="right">A <see cref="InternetControlMessage" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(InternetControlMessage left, InternetControlMessage right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Tries to create a <see cref="InternetControlMessage" /> object from the the string
        /// </summary>
        /// <param name="str">The string to be analyzed</param>
        /// <param name="icm">Returning <see cref="InternetControlMessage" /> object</param>
        /// <returns>
        ///     <see langword="true" /> if process ends well and <see cref="InternetControlMessage" /> created; otherwise
        ///     <see langword="false" />
        /// </returns>
        public static bool TryParse(string str, out InternetControlMessage icm)
        {
            var parts = str.Split(':');
            if (parts.Length == 1)
            {
                if (parts[0].Trim() == "*")
                {
                    icm = Any;
                    return true;
                }
            }
            else if (parts.Length == 2)
            {
                if ((parts[0].Trim() == "*") && (parts[1].Trim() == "*"))
                {
                    icm = Any;
                    return true;
                }
                byte t;
                if (byte.TryParse(parts[0].Trim(), out t))
                {
                    byte c;
                    if (parts[1].Trim() == "*")
                    {
                        icm = new InternetControlMessage(t);
                        return true;
                    }
                    if (byte.TryParse(parts[1].Trim(), out c))
                    {
                        icm = new InternetControlMessage(t, c);
                        return true;
                    }
                }
            }
            icm = null;
            return false;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="InternetControlMessage" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="InternetControlMessage" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="InternetControlMessage" />. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InternetControlMessage) obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="InternetControlMessage" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Type.GetHashCode()*397) ^ Code.GetHashCode();
            }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (Equals(Any))
                return "*";
            return $"{_type}:{_code?.ToString() ?? "*"}";
        }
    }
}