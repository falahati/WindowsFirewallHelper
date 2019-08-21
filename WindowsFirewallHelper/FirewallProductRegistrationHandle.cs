using System;
using System.Runtime.InteropServices;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Represents a firewall product registration handle
    /// </summary>
    public class FirewallProductRegistrationHandle : IDisposable
    {
        internal FirewallProductRegistrationHandle(object handle)
        {
            if (handle == null || !handle.GetType().IsCOMObject)
            {
                throw new ArgumentException("Handle is empty or invalid.", nameof(handle));
            }

            DangerousHandle = handle;
        }

        /// <summary>
        ///     Returns the underlying dangerous handle value
        /// </summary>
        public object DangerousHandle { get; private set; }

        /// <summary>
        ///     Returns a boolean value indicating if this instance contains a valid handle
        /// </summary>
        public bool IsInvalid
        {
            get => DangerousHandle == null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Release();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases the handle
        /// </summary>
        public void Release()
        {
            lock (this)
            {
                if (!IsInvalid)
                {
                    Marshal.ReleaseComObject(DangerousHandle);
                    DangerousHandle = null;
                }
            }
        }

        /// <inheritdoc />
        ~FirewallProductRegistrationHandle()
        {
            Release();
        }
    }
}