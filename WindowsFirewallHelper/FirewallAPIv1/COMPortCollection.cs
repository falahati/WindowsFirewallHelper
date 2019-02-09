using System;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    internal class COMPortCollection :
        COMNativeCollection<INetFwOpenPorts, INetFwOpenPort, COMPortCollectionKey>
    {
        private static readonly Random Random = new Random();

        public COMPortCollection(INetFwOpenPorts authorizedPortsCollection) :
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
            finally
            {
                item.Port = oldPortNumber;
            }
        }

        /// <inheritdoc />
        protected override COMPortCollectionKey GetCollectionKey(INetFwOpenPort managed)
        {
            return new COMPortCollectionKey(managed.Port, managed.Protocol);
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
        protected override INetFwOpenPort InternalItem(COMPortCollectionKey key)
        {
            return NativeEnumerable.Item(key.PortNumber, key.ProtocolType);
        }

        /// <inheritdoc />
        protected override void InternalRemove(COMPortCollectionKey key)
        {
            NativeEnumerable.Remove(key.PortNumber, key.ProtocolType);
        }
    }
}