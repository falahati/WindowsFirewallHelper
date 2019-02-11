using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFirewallHelper.InternalHelpers
{
    // ReSharper disable once HollowTypeName
    internal class NativeHelper
    {
        public static string ResolveStringResource(string str)
        {
            if (str?.StartsWith("@") != true)
            {
                return str;
            }

            try
            {
                var buffer = new StringBuilder(8 * 1024);
                var result = SHLoadIndirectString(Environment.ExpandEnvironmentVariables(str), buffer,
                    buffer.Capacity, IntPtr.Zero);

                if (result == 0)
                {
                    str = buffer.ToString();
                }
                else
                {
                    var idIndex = str.LastIndexOf(",", StringComparison.InvariantCulture);

                    if (idIndex > 1)
                    {
                        var idString = str.Substring(idIndex + 1);
                        var fileName = Environment.ExpandEnvironmentVariables(str.Substring(1, idIndex - 1));
                        var id = (uint) Math.Abs(int.Parse(idString));
                        var handle = LoadLibrary(fileName);
                        var size = LoadString(handle, id, buffer, buffer.Capacity);

                        if (size > 0)
                        {
                            str = buffer.ToString();
                        }

                        FreeLibrary(handle);
                    }
                }
            }
            catch
            {
                // ignore
            }

            return str;
        }

        [DllImport(@"kernel32")]
        private static extern int FreeLibrary(IntPtr libraryHandle);

        [DllImport(@"kernel32",
            CharSet = CharSet.Auto,
            SetLastError = true,
            BestFitMapping = false,
            ThrowOnUnmappableChar = true)]
        private static extern IntPtr LoadLibrary(string fileName);

        [DllImport(@"user32",
            CharSet = CharSet.Auto,
            SetLastError = true,
            BestFitMapping = false,
            ThrowOnUnmappableChar = true)]
        // ReSharper disable once TooManyArguments
        private static extern int LoadString(
            IntPtr libraryHandle,
            uint resourceId,
            StringBuilder buffer,
            int bufferSize
        );

        // ReSharper disable once StringLiteralTypo
        [DllImport(@"shlwapi",
            CharSet = CharSet.Auto,
            SetLastError = true,
            BestFitMapping = false,
            ThrowOnUnmappableChar = true)]
        // ReSharper disable once TooManyArguments
        private static extern int SHLoadIndirectString(
            string resourceString,
            StringBuilder buffer,
            int bufferSize,
            IntPtr reserved
        );
    }
}