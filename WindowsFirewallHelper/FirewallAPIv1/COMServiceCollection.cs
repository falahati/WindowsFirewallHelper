using System;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper.FirewallAPIv1
{
    internal class COMServiceCollection :
        COMNativeCollection<INetFwServices, INetFwService, NET_FW_SERVICE_TYPE>
    {
        public COMServiceCollection(INetFwServices servicesCollection) :
            base(servicesCollection)
        {
        }


        /// <inheritdoc />
        public override bool IsReadOnly { get; } = true;

        /// <inheritdoc />
        protected override NET_FW_SERVICE_TYPE GetCollectionKey(INetFwService managed)
        {
            return managed.Type;
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant()
        {
            return NativeEnumerable.GetEnumeratorVariant();
        }

        /// <inheritdoc />
        protected override void InternalAdd(INetFwService native)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        protected override int InternalCount()
        {
            return NativeEnumerable.Count;
        }

        /// <inheritdoc />
        protected override INetFwService InternalItem(NET_FW_SERVICE_TYPE key)
        {
            return NativeEnumerable.Item(key);
        }

        /// <inheritdoc />
        protected override void InternalRemove(NET_FW_SERVICE_TYPE key)
        {
            throw new InvalidOperationException();
        }
    }
}