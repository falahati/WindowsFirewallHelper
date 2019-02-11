using System;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     A class representing a Internet Control Message (ICM) type
    /// </summary>
    public class FirewallWASInternetControlMessage : IEquatable<FirewallWASInternetControlMessage>
    {
        /// <summary>
        ///     All ICM types and codes
        /// </summary>
        public static readonly FirewallWASInternetControlMessage Any = new FirewallWASInternetControlMessage();

        private readonly byte? _code;
        private readonly byte? _type;

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified ICM type number
        /// </summary>
        /// <param name="type">ICM type number to create the instance from</param>
        public FirewallWASInternetControlMessage(byte type)
        {
            _type = type;
            _code = null;
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified ICM type number and type code
        /// </summary>
        /// <param name="type">ICM type number to create the instance from</param>
        /// <param name="code">ICM type code to create the instance from</param>
        public FirewallWASInternetControlMessage(byte type, byte code)
        {
            _type = type;
            _code = code;
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        public FirewallWASInternetControlMessage(InternetControlMessageKnownTypesV6 type) : this((byte) type)
        {
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type and type code
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        /// <param name="code">ICM type code to create the instance from</param>
        public FirewallWASInternetControlMessage(InternetControlMessageKnownTypesV6 type, byte code) : this((byte) type,
            code)
        {
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        public FirewallWASInternetControlMessage(InternetControlMessageKnownTypes type) : this((byte) type)
        {
        }

        /// <summary>
        ///     Creates a new instance of the InternetControlMessage class with the specified known ICM type and type code
        /// </summary>
        /// <param name="type">ICM type to create the instance from</param>
        /// <param name="code">ICM type code to create the instance from</param>
        public FirewallWASInternetControlMessage(InternetControlMessageKnownTypes type, byte code) : this((byte) type,
            code)
        {
        }

        private FirewallWASInternetControlMessage()
        {
            _type = null;
            _code = null;
        }

        /// <summary>
        ///     Gets the corresponding ICM type code
        /// </summary>
        public int Code
        {
            get => _code ?? -1;
        }

        /// <summary>
        ///     Gets the corresponding ICM type number
        /// </summary>
        public int Type
        {
            get => _type ?? -1;
        }

        /// <summary>
        ///     Determines whether the specified<see cref="FirewallWASInternetControlMessage" /> is equal to the current
        ///     <see cref="FirewallWASInternetControlMessage" />.
        /// </summary>
        /// <param name="other"> The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="FirewallWASInternetControlMessage" /> is equal to the current
        ///     <see cref="FirewallWASInternetControlMessage" />;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(FirewallWASInternetControlMessage other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Type == other.Type && Code == other.Code;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASInternetControlMessage" /> objects for equality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASInternetControlMessage" /> object</param>
        /// <param name="right">A <see cref="FirewallWASInternetControlMessage" /> object</param>
        /// <returns>true if two sides are equal; otherwise false</returns>
        public static bool operator ==(FirewallWASInternetControlMessage left, FirewallWASInternetControlMessage right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        /// <summary>
        ///     Compares two <see cref="FirewallWASInternetControlMessage" /> objects for inequality
        /// </summary>
        /// <param name="left">A <see cref="FirewallWASInternetControlMessage" /> object</param>
        /// <param name="right">A <see cref="FirewallWASInternetControlMessage" /> object</param>
        /// <returns>true if two sides are not equal; otherwise false</returns>
        public static bool operator !=(FirewallWASInternetControlMessage left, FirewallWASInternetControlMessage right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Tries to create a <see cref="FirewallWASInternetControlMessage" /> object from the the string
        /// </summary>
        /// <param name="str">The string to be analyzed</param>
        /// <param name="icm">Returning <see cref="FirewallWASInternetControlMessage" /> object</param>
        /// <returns>
        ///     <see langword="true" /> if process ends well and <see cref="FirewallWASInternetControlMessage" /> created;
        ///     otherwise
        ///     <see langword="false" />
        /// </returns>
        // ReSharper disable once MethodTooLong
        // ReSharper disable once ExcessiveIndentation
        public static bool TryParse(string str, out FirewallWASInternetControlMessage icm)
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
                if (parts[0].Trim() == "*" && parts[1].Trim() == "*")
                {
                    icm = Any;

                    return true;
                }

                if (byte.TryParse(parts[0].Trim(), out var type))
                {
                    if (parts[1].Trim() == "*")
                    {
                        icm = new FirewallWASInternetControlMessage(type);

                        return true;
                    }

                    if (byte.TryParse(parts[1].Trim(), out var code))
                    {
                        icm = new FirewallWASInternetControlMessage(type, code);

                        return true;
                    }
                }
            }

            icm = null;

            return false;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="FirewallWASInternetControlMessage" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="FirewallWASInternetControlMessage" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///     The <see cref="T:System.Object" /> to compare with the current
        ///     <see cref="FirewallWASInternetControlMessage" />.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return Equals(obj as FirewallWASInternetControlMessage);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="FirewallWASInternetControlMessage" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Type.GetHashCode() * 397) ^ Code.GetHashCode();
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
            {
                return "*";
            }

            return $"{_type}:{_code?.ToString() ?? "*"}";
        }
    }
}