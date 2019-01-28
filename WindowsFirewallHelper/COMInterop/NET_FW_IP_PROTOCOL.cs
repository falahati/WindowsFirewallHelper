// ReSharper disable InconsistentNaming

namespace WindowsFirewallHelper.COMInterop
{
    public enum NET_FW_IP_PROTOCOL
    {
        NET_FW_IP_PROTOCOL_TCP = 6,
        NET_FW_IP_PROTOCOL_UDP = 17, // 0x00000011
        NET_FW_IP_PROTOCOL_ANY = 256 // 0x00000100
    }
}