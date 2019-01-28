using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WindowsFirewallHelper.COMInterop
{
    [Guid("F7898AF5-CAC4-4632-A2EC-DA06E5111AF2")]
    [ComImport]
    internal interface INetFwMgr
    {
        [DispId(1)]
        INetFwPolicy LocalPolicy
        {
            [DispId(1)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            [return: MarshalAs(UnmanagedType.Interface)]
            get;
        }

        [DispId(2)]
        NET_FW_PROFILE_TYPE CurrentProfileType
        {
            [DispId(2)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            get;
        }

        [DispId(3)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void RestoreDefaults();

        [DispId(4)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        // ReSharper disable once TooManyArguments
        void IsPortAllowed(
            [MarshalAs(UnmanagedType.BStr)] [In] string imageFileName,
            [In] NET_FW_IP_VERSION ipVersion,
            [In] int portNumber,
            [MarshalAs(UnmanagedType.BStr)] [In] string localAddress,
            [In] NET_FW_IP_PROTOCOL ipProtocol,
            [MarshalAs(UnmanagedType.Struct)] out object allowed,
            [MarshalAs(UnmanagedType.Struct)] out object restricted
        );

        [DispId(5)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        // ReSharper disable once TooManyArguments
        void IsIcmpTypeAllowed(
            [In] NET_FW_IP_VERSION ipVersion,
            [MarshalAs(UnmanagedType.BStr)] [In] string localAddress,
            [In] byte type,
            [MarshalAs(UnmanagedType.Struct)] out object allowed,
            [MarshalAs(UnmanagedType.Struct)] out object restricted
        );
    }
}