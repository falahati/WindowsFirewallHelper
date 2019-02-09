using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WindowsFirewallHelper.Helpers
{
    internal abstract class
        COMCollection<TCollection, TNative, TKey, TManaged> : ICOMCollection<TKey, TManaged>
        where TCollection : IEnumerable
        where TNative : class
        where TManaged : class
        where TKey : COMCollectionKey
    {
        private const string AddMethodName = "Add";
        private const string CountPropertyName = "Count";
        private const string ItemMethodName = "Item";
        private const string RemoveMethodName = "Remove";

        private readonly TCollection _sourceEnumerable;
        private readonly Type _sourceEnumerableRuntimeType;

        protected COMCollection(TCollection sourceEnumerable)
        {
            if (sourceEnumerable == null)
            {
                throw new ArgumentNullException(nameof(sourceEnumerable));
            }

            _sourceEnumerableRuntimeType = sourceEnumerable.GetType();

            if (!_sourceEnumerableRuntimeType.IsCOMObject)
            {
                throw new ArgumentException(
                    "Passed argument is not a valid COM Enumerable object.",
                    nameof(sourceEnumerable)
                );
            }

            _sourceEnumerable = sourceEnumerable;
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
            get => _sourceEnumerable;
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
                _sourceEnumerableRuntimeType.InvokeMember(
                    AddMethodName,
                    BindingFlags.InvokeMethod,
                    null,
                    _sourceEnumerable,
                    new object[] {nativeObject}
                );
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
                    return (int) _sourceEnumerableRuntimeType.InvokeMember(CountPropertyName, BindingFlags.GetProperty,
                        null,
                        _sourceEnumerable, null);
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
                    var result = _sourceEnumerableRuntimeType.InvokeMember(
                        ItemMethodName,
                        BindingFlags.InvokeMethod,
                        null,
                        _sourceEnumerable,
                        key.Values
                    );

                    if (result == null)
                    {
                        return null;
                    }

                    if (!(result is TNative native))
                    {
                        throw new ArgumentOutOfRangeException(nameof(key));
                    }

                    // ReSharper disable once EventExceptionNotDocumented
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
                _sourceEnumerableRuntimeType.InvokeMember(
                    RemoveMethodName,
                    BindingFlags.InvokeMethod,
                    null,
                    _sourceEnumerable,
                    key.Values
                );

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
            var enumVariant = GetEnumVariant(_sourceEnumerable);

            if (enumVariant == null)
            {
                throw new NotSupportedException("This operation is not supported with the passed COM object.");
            }

            return new COMEnumerator<TNative, TManaged>(enumVariant, ConvertNativeToManaged);
        }

        protected abstract TNative ConvertManagedToNative(TManaged managed);
        protected abstract TManaged ConvertNativeToManaged(TNative native);
        protected abstract TKey GetCollectionKey(TManaged managed);

        protected abstract IEnumVARIANT GetEnumVariant(TCollection sourceCollection);
    }
}