using System;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1.COMCollectionProxy
{
    internal class COMPortCollectionKey : COMCollectionKey
    {
        /// <inheritdoc />
        public COMPortCollectionKey(int portNumber, NET_FW_IP_PROTOCOL protocolType)
        {
            PortNumber = portNumber;
            ProtocolType = protocolType;
        }

        public COMPortCollectionKey(INetFwOpenPort rule) : this(rule.Port, rule.Protocol)
        {
        }


        public int PortNumber { get; }
        public NET_FW_IP_PROTOCOL ProtocolType { get; }


        /// <inheritdoc />
        public override Type[] Types { get; } = {typeof(int), typeof(NET_FW_IP_PROTOCOL)};

        /// <inheritdoc />
        public override object[] Values
        {
            get => new object[] {PortNumber, ProtocolType};
        }
    }
}