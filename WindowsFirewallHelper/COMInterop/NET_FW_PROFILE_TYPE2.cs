// ReSharper disable InconsistentNaming

namespace WindowsFirewallHelper.COMInterop
{
    internal enum NET_FW_PROFILE_TYPE2
    {
        NET_FW_PROFILE2_DOMAIN = 1,
        NET_FW_PROFILE2_PRIVATE = 2,
        NET_FW_PROFILE2_PUBLIC = 4,
        NET_FW_PROFILE2_ALL = 2147483647 // 0x7FFFFFFF
    }
}