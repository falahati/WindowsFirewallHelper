using System;
using System.Runtime.InteropServices;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Represents a firewall product registration handle; the registration will be removed when this object is released.
    /// </summary>
    public class FirewallProductRegistrationHandle : IDisposable
    {
        private object _handle;

        internal FirewallProductRegistrationHandle(object handle)
        {
            if (handle == null || !handle.GetType().IsCOMObject)
            {
                throw new ArgumentException("Handle is empty or invalid.", nameof(handle));
            }

            _handle = handle;
        }

        /// <summary>
        ///     Returns a boolean value indicating if this instance contains a valid handle
        /// </summary>
        public bool IsInvalid
        {
            get => _handle == null;
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
                    Marshal.ReleaseComObject(_handle);
                    _handle = null;
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