using System;
using WindowsFirewallHelper.COMInterop;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    internal class COMPortCollectionKey : IEquatable<COMPortCollectionKey>
    {
        /// <inheritdoc />
        public COMPortCollectionKey(int portNumber, NET_FW_IP_PROTOCOL protocolType)
        {
            PortNumber = portNumber;
            ProtocolType = protocolType;
        }


        public int PortNumber { get; }
        public NET_FW_IP_PROTOCOL ProtocolType { get; }

        /// <inheritdoc />
        public bool Equals(COMPortCollectionKey other)
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

        public static bool operator ==(COMPortCollectionKey left, COMPortCollectionKey right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        public static bool operator !=(COMPortCollectionKey left, COMPortCollectionKey right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as COMPortCollectionKey);
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