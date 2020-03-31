using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using WindowsFirewallHelper.COMInterop;
using WindowsFirewallHelper.InternalHelpers.Collections;

namespace WindowsFirewallHelper.Collections
{
    internal class FirewallProductsCollection : ComCollectionBase<INetFwProducts, INetFwProduct, int, FirewallProduct>,
        IFirewallProductsCollection
    {
        /// <inheritdoc />
        public FirewallProductsCollection(INetFwProducts nativeEnumerable) : base(nativeEnumerable)
        {
        }

        /// <inheritdoc />
        public override bool IsReadOnly { get; } = true;

        /// <inheritdoc />
        public int IndexOf(FirewallProduct product)
        {
            var i = 0;

            foreach (var p in this)
            {
                if (p.Equals(product))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

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
        protected override void InternalRemove(int key)
        {
            throw new InvalidOperationException();
        }
    }
}