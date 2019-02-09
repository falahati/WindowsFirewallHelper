using System;
using System.Runtime.InteropServices;

namespace WindowsFirewallHelper
{
    public class FirewallProductRegistrationHandle : IDisposable
    {
        private object _handle;

        public FirewallProductRegistrationHandle(object handle)
        {
            if (handle == null || !handle.GetType().IsCOMObject)
            {
                throw new ArgumentException("Handle is empty or invalid.", nameof(handle));
            }

            _handle = handle;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Release();
            GC.SuppressFinalize(this);
        }

        public void Release()
        {
            lock (this)
            {
                if (_handle != null)
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