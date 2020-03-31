using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers.Collections;

namespace WindowsFirewallHelper.Collections
{
    internal class FirewallLegacyPortCollection :
        ComNativeCollectionBase<INetFwOpenPorts, INetFwOpenPort, FirewallLegacyPortCollectionKey>
    {
        private static readonly Random Random = new Random();

        public FirewallLegacyPortCollection(INetFwOpenPorts authorizedPortsCollection) :
            base(authorizedPortsCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public override bool Remove(INetFwOpenPort item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var oldPortNumber = item.Port;

            try
            {
                item.Port = (ushort) Random.Next(10240, ushort.MaxValue);
                base.Remove(item);

                return true;
            }
            catch (Exception)
            {
                item.Port = oldPortNumber;

                throw;
            }
        }

        /// <inheritdoc />
        protected override FirewallLegacyPortCollectionKey GetCollectionKey(INetFwOpenPort managed)
        {
            return new FirewallLegacyPortCollectionKey(managed.Port, managed.Protocol);
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant()
        {
            return NativeEnumerable.GetEnumeratorVariant();
        }

        /// <inheritdoc />
        protected override void InternalAdd(INetFwOpenPort native)
        {
            NativeEnumerable.Add(native);
        }

        /// <inheritdoc />
        protected override int InternalCount()
        {
            return NativeEnumerable.Count;
        }

        /// <inheritdoc />
        protected override INetFwOpenPort InternalItem(FirewallLegacyPortCollectionKey key)
        {
            try
            {
                return NativeEnumerable.Item(key.PortNumber, key.ProtocolType);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override void InternalRemove(FirewallLegacyPortCollectionKey key)
        {
            NativeEnumerable.Remove(key.PortNumber, key.ProtocolType);
        }
    }
}