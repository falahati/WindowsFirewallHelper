using System;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.Helpers;

namespace WindowsFirewallHelper
{
    internal class FirewallProductsCollection : COMCollection<INetFwProducts, INetFwProduct, int, FirewallProduct>
    {
        /// <inheritdoc />
        public FirewallProductsCollection(INetFwProducts nativeEnumerable) : base(nativeEnumerable)
        {
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = true;

        /// <inheritdoc />
        protected override INetFwProduct ConvertManagedToNative(FirewallProduct managed)
        {
            return managed.GetCOMObject();
        }

        /// <inheritdoc />
        protected override FirewallProduct ConvertNativeToManaged(INetFwProduct native)
        {
            return new FirewallProduct(native);
        }

        /// <inheritdoc />
        protected override int GetCollectionKey(FirewallProduct managed)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        protected override IEnumVARIANT GetEnumVariant()
        {
            return NativeEnumerable.GetEnumeratorVariant();
        }

        /// <inheritdoc />
        protected override void InternalAdd(INetFwProduct native)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        protected override int InternalCount()
        {
            return NativeEnumerable.Count;
        }

        /// <inheritdoc />
        protected override INetFwProduct InternalItem(int key)
        {
            return NativeEnumerable.Item(key);
        }

        /// <inheritdoc />
        protected override void InternalRemove(int key)
        {
            throw new InvalidOperationException();
        }
    }
}