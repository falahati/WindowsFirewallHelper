using System;

namespace WindowsFirewallHelper.Helpers
{
    internal static class EnumHelper
    {
        public static bool HasFlag<T>(T value, T flag) where T : IConvertible
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("T must be of enumerable type");
            }
            switch (Convert.GetTypeCode(value))
            {
                case TypeCode.Boolean:
                    return (bool) (object) value == (bool) (object) flag;
                case TypeCode.Char:
                    return (char) (object) value == 0
                        ? (char) (object) flag == 0
                        : ((char) (object) flag & (char) (object) value) ==
                          (char) (object) flag;
                case TypeCode.SByte:
                    return (byte) (sbyte) (object) value == 0
                        ? (byte) (sbyte) (object) flag == 0
                        : ((byte) (sbyte) (object) flag & (byte) (sbyte) (object) value) == (byte) (sbyte) (object) flag;
                case TypeCode.Byte:
                    return (byte) (object) value == 0
                        ? (byte) (object) flag == 0
                        : ((byte) (object) flag & (byte) (object) value) == (byte) (object) flag;
                case TypeCode.Int16:
                    return (ushort) (short) (object) value == 0
                        ? (ushort) (short) (object) flag == 0
                        : ((ushort) (short) (object) flag & (ushort) (short) (object) value) ==
                          (ushort) (short) (object) flag;
                case TypeCode.UInt16:
                    return (ushort) (object) value == 0
                        ? (ushort) (object) flag == 0
                        : ((ushort) (object) flag & (ushort) (object) value) == (ushort) (object) flag;
                case TypeCode.Int32:
                    return (uint) (int) (object) value == 0
                        ? (uint) (int) (object) flag == 0
                        : ((uint) (int) (object) flag & (uint) (int) (object) value) == (uint) (int) (object) flag;
                case TypeCode.UInt32:
                    return (uint) (object) value == 0
                        ? (uint) (object) flag == 0
                        : ((uint) (object) flag & (uint) (object) value) == (uint) (object) flag;
                case TypeCode.Int64:
                    return (ulong) (long) (object) value == 0
                        ? (ulong) (long) (object) flag == 0
                        : ((ulong) (long) (object) flag & (ulong) (long) (object) value) == (ulong) (long) (object) flag;
                case TypeCode.UInt64:
                    return (ulong) (object) value == 0
                        ? (ulong) (object) flag == 0
                        : ((ulong) (object) flag & (ulong) (object) value) == (ulong) (object) flag;
            }
            return value.Equals(flag);
        }
    }
}