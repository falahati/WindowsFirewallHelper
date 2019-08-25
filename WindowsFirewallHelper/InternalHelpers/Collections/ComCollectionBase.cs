using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WindowsFirewallHelper.InternalHelpers.Collections
{
    internal abstract class ComCollectionBase<TCollection, TNative, TKey, TManaged> : IComCollection<TKey, TManaged>
        where TCollection : IEnumerable
        where TNative : class
        where TManaged : class
    {
        protected readonly TCollection NativeEnumerable;

        protected ComCollectionBase(TCollection nativeEnumerable)
        {
            if (nativeEnumerable == null)
            {
                throw new ArgumentNullException(nameof(nativeEnumerable));
            }

            if (!nativeEnumerable.GetType().IsCOMObject)
            {
                throw new ArgumentException(
                    "Passed argument is not a valid COM Enumerable object.",
                    nameof(nativeEnumerable)
                );
            }

            NativeEnumerable = nativeEnumerable;
        }

        /// <inheritdoc />
        public virtual void CopyTo(Array array, int index)
        {
            foreach (var target in this)
            {
                array.SetValue(target, index);
                index++;
            }
        }

        /// <inheritdoc cref="ICollection.IsSynchronized" />
        bool ICollection.IsSynchronized
        {
            get => false;
        }

        /// <inheritdoc cref="ICollection.SyncRoot" />
        object ICollection.SyncRoot
        {
            get => NativeEnumerable;
        }

        /// <inheritdoc />
        // ReSharper disable once MethodNameNotMeaningful
        public virtual void Add(TManaged item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException("Collection is readonly.");
            }

            var nativeObject = ConvertManagedToNative(item);

            if (nativeObject == null)
            {
                return;
            }

            try
            {
                InternalAdd(nativeObject);
            }
            catch (COMException e)
            {
                throw new NotSupportedException("This operation is not supported with the passed COM object.", e);
            }
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual bool Contains(TManaged item)
        {
            return this.Any(target => target == item);
        }

        /// <inheritdoc />
        public virtual void CopyTo(TManaged[] array, int arrayIndex)
        {
            CopyTo(array as Array, arrayIndex);
        }

        /// <inheritdoc cref="ICollection.Count" />
        public virtual int Count
        {
            get
            {
                try
                {
                    return InternalCount();
                }
                catch (COMException e)
                {
                    throw new NotSupportedException("This property is not supported with the passed COM object.", e);
                }
            }
        }

        /// <inheritdoc />
        public abstract bool IsReadOnly { get; }

        /// <inheritdoc />
        public virtual bool Remove(TManaged item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var key = GetCollectionKey(item);

            if (key == null)
            {
                return false;
            }

            Remove(key);

            return true;
        }

        /// <inheritdoc />
        public virtual bool Contains(TKey key)
        {
            return this.Select(GetCollectionKey).Any(key1 => key1.Equals(key));
        }

        /// <inheritdoc />
        public virtual TManaged this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException();
                }

                try
                {
                    var native = InternalItem(key);

                    if (native == null)
                    {
                        return null;
                    }

                    return ConvertNativeToManaged(native);
                }
                catch (COMException e)
                {
                    throw new NotSupportedException("This operation is not supported with the passed COM object.", e);
                }
            }
        }

        /// <inheritdoc />
        public virtual bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException("Collection is readonly.");
            }

            try
            {
                InternalRemove(key);

                return true;
            }
            catch (COMException e)
            {
                throw new NotSupportedException("This operation is not supported with the passed COM object.", e);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public virtual IEnumerator<TManaged> GetEnumerator()
        {
            // ReSharper disable once EventExceptionNotDocumented
            var enumVariant = GetEnumVariant();

            if (enumVariant == null)
            {
                throw new NotSupportedException("This operation is not supported with the passed COM object.");
            }

            return new ComEnumerator<TNative, TManaged>(enumVariant, ConvertNativeToManaged);
        }

        protected abstract TNative ConvertManagedToNative(TManaged managed);
        protected abstract TManaged ConvertNativeToManaged(TNative native);
        protected abstract TKey GetCollectionKey(TManaged managed);
        protected abstract IEnumVARIANT GetEnumVariant();
        protected abstract void InternalAdd(TNative native);
        protected abstract int InternalCount();
        protected abstract TNative InternalItem(TKey key);
        protected abstract void InternalRemove(TKey key);
    }
}