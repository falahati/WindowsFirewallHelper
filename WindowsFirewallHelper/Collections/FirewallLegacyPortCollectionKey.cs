using System;
using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper.Collections
{
    internal class FirewallLegacyPortCollectionKey : IEquatable<FirewallLegacyPortCollectionKey>
    {
        public FirewallLegacyPortCollectionKey(int portNumber, NetFwIPProtocol protocolType)
        {
            PortNumber = portNumber;
            ProtocolType = protocolType;
        }

        public int PortNumber { get; }
        public NetFwIPProtocol ProtocolType { get; }

        /// <inheritdoc />
        public bool Equals(FirewallLegacyPortCollectionKey other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return PortNumber == other.PortNumber && ProtocolType == other.ProtocolType;
        }

        public static bool operator ==(FirewallLegacyPortCollectionKey left, FirewallLegacyPortCollectionKey right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        public static bool operator !=(FirewallLegacyPortCollectionKey left, FirewallLegacyPortCollectionKey right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as FirewallLegacyPortCollectionKey);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (PortNumber * 397) ^ (int) ProtocolType;
            }
        }
    }
}