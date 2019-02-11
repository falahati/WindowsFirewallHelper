using System.Collections;

namespace WindowsFirewallHelper.InternalHelpers.Collections
{
    internal abstract class
        ComNativeCollectionBase<TCollection, TValue, TKey> : ComCollectionBase<TCollection, TValue, TKey, TValue>
        where TCollection : IEnumerable
        where TValue : class
    {
        /// <inheritdoc />
        protected ComNativeCollectionBase(TCollection nativeEnumerable) : base(nativeEnumerable)
        {
        }

        /// <inheritdoc />
        protected override TValue ConvertManagedToNative(TValue managed)
        {
            return managed;
        }

        /// <inheritdoc />
        protected override TValue ConvertNativeToManaged(TValue native)
        {
            return native;
        }
    }
}