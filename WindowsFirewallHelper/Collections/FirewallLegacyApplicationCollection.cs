using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers.Collections;

namespace WindowsFirewallHelper.Collections
{
    internal class FirewallLegacyApplicationCollection :
        ComNativeCollectionBase<INetFwAuthorizedApplications, INetFwAuthorizedApplication, string>
    {
        public FirewallLegacyApplicationCollection(
            INetFwAuthorizedApplications authorizedApplicationsCollection) : base(
            authorizedApplicationsCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public override bool Remove(INetFwAuthorizedApplication item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var oldFilename = item.ProcessImageFileName;
            var tempFilename = Path.GetTempFileName();

            try
            {
                item.ProcessImageFileName = tempFilename;
                base.Remove(item);

                return true;
            }
            catch (Exception)
            {
                item.ProcessImageFileName = oldFilename;

                throw;
            }
            finally
            {
                if (File.Exists(tempFilename))
                {
                    File.Delete(tempFilename);
                }
            }
        }

        /// <inheritdoc />
        protected override string GetCollectionKey(INetFwAuthorizedApplication managed)
        {
            return managed.ProcessImageFileName;
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant()
        {
            return NativeEnumerable.GetEnumeratorVariant();
        }

        /// <inheritdoc />
        protected override void InternalAdd(INetFwAuthorizedApplication native)
        {
            NativeEnumerable.Add(native);
        }

        /// <inheritdoc />
        protected override int InternalCount()
        {
            return NativeEnumerable.Count;
        }

        /// <inheritdoc />
        protected override INetFwAuthorizedApplication InternalItem(string key)
        {
            try
            {
                return NativeEnumerable.Item(key);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override void InternalRemove(string key)
        {
            NativeEnumerable.Remove(key);
        }
    }
}