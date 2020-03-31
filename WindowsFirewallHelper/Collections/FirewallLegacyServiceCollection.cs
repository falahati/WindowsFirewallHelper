using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers.Collections;

namespace WindowsFirewallHelper.Collections
{
    internal class FirewallLegacyServiceCollection :
        ComNativeCollectionBase<INetFwServices, INetFwService, NetFwServiceType>
    {
        public FirewallLegacyServiceCollection(INetFwServices servicesCollection) :
            base(servicesCollection)
        {
        }


        /// <inheritdoc />
        public override bool IsReadOnly { get; } = true;

        /// <inheritdoc />
        protected override NetFwServiceType GetCollectionKey(INetFwService managed)
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
        protected override INetFwService InternalItem(NetFwServiceType key)
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
        protected override void InternalRemove(NetFwServiceType key)
        {
            throw new InvalidOperationException();
        }
    }
}