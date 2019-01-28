using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WindowsFirewallHelper.Helpers
{
    internal class EnumeratorToEnumVariantMarshaler : ICustomMarshaler
    {
        private static EnumeratorToEnumVariantMarshaler _instance;

        public void CleanUpManagedData(object pManagedObj)
        {
            throw new NotImplementedException();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.Release(pNativeData);
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object pManagedObj)
        {
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            var enumVariant = (IEnumVARIANT) Marshal.GetObjectForIUnknown(pNativeData);

            return new VARIANTEnumerator(enumVariant);
        }

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return _instance ?? (_instance = new EnumeratorToEnumVariantMarshaler());
        }

        [ComImport]
        [Guid("00020404-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IEnumVARIANT
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void Next(int celt, [MarshalAs(UnmanagedType.Struct)] out object rgvar, out uint pceltFetched);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void Skip(uint celt);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void Reset();

            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            IEnumVARIANT Clone();
        }

        // ReSharper disable once InconsistentNaming
        private class VARIANTEnumerator : IEnumerator
        {
            private readonly IEnumVARIANT _comEnum;

            public VARIANTEnumerator(IEnumVARIANT comEnum)
            {
                _comEnum = comEnum;
            }


            public object Current { get; private set; }

            public bool MoveNext()
            {
                _comEnum.Next(1, out var val, out var fetched);

                if (fetched == 0)
                {
                    return false;
                }

                Current = val;

                return true;
            }

            public void Reset()
            {
                _comEnum.Reset();
            }
        }
    }
}