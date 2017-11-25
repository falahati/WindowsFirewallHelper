using System;

namespace WindowsFirewallHelper.Addresses
{
    /// <summary>
    ///     This class is the parent class of all special address values
    /// </summary>
    public abstract class SpecialAddress : IAddress, IEquatable<SpecialAddress>
    {
        /// <summary>
        ///     Should returns the constant value of the special address
        /// </summary>
        protected abstract string AddressString { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return AddressString;
        }

        /// <inheritdoc />
        public bool Equals(SpecialAddress other)
        {
            return (other != null) && AddressString.Equals(other.AddressString);
        }

        /// <summary>
        ///     Determines whether a string is a valid address
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> is a valid address; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="str">The string to validate.</param>
        /// <param name="service">The instance that represents the passed string.</param>
        protected static bool TryParse<T>(string str, out T service) where T : SpecialAddress, new()
        {
            service = new T();
            if (str.Trim().Equals(service.AddressString, StringComparison.InvariantCultureIgnoreCase))
                return true;
            service = null;
            return false;
        }

        /// <summary>
        ///     Compares address instances.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="obj">An <see cref="T:Object" /> instance to compare to the current instance. </param>
        public override bool Equals(object obj)
        {
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (obj is SpecialAddress)
                return Equals((SpecialAddress) obj);
            return ReferenceEquals(this, obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current instance.
        /// </returns>
        public override int GetHashCode()
        {
            return AddressString?.GetHashCode() ?? 0;
        }
    }
}