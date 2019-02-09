using System.Collections;

namespace WindowsFirewallHelper.Helpers
{
    internal abstract class
        COMNativeCollection<TCollection, TValue, TKey> : COMCollection<TCollection, TValue, TKey, TValue>
        where TCollection : IEnumerable
        where TValue : class
    {
        /// <inheritdoc />
        protected COMNativeCollection(TCollection nativeEnumerable) : base(nativeEnumerable)
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