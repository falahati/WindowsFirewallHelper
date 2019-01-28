using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WindowsFirewallHelper.Helpers
{
    internal static class EnumerableHelper
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumVARIANT enumVariant)
        {
            enumVariant.Reset();
            int bufferLength;
            do
            {
                var buffer = new object[1];
                var bufferLengthPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
                enumVariant.Next(buffer.Length, buffer, bufferLengthPointer);
                bufferLength = Marshal.ReadInt32(bufferLengthPointer);
                Marshal.FreeCoTaskMem(bufferLengthPointer);

                for (int i = 0; i < Math.Min(bufferLength, buffer.Length); i++)
                {
                    yield return (T) buffer[i];
                }
            } while (bufferLength > 0);
        }

        public static IEnumerable<T> ToEnumerable<T>(this IEnumVARIANT enumVariant, int count)
        {
            enumVariant.Reset();

            var buffer = new object[count];
            var bufferLengthPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
            enumVariant.Next(buffer.Length, buffer, bufferLengthPointer);
            var bufferLength = Marshal.ReadInt32(bufferLengthPointer);
            Marshal.FreeCoTaskMem(bufferLengthPointer);

            for (int i = 0; i < Math.Min(bufferLength, count); i++)
            {
                yield return (T)buffer[i];
            }
        }

    }
}
