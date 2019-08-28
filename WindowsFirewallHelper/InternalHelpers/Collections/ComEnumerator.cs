using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WindowsFirewallHelper.InternalHelpers.Collections
{
    internal class ComEnumerator<TSource, TTarget> : IEnumerator<TTarget>
    {
        private readonly object[] _buffer = new object[1];
        private readonly IntPtr _bufferLengthPointer;
        private readonly IEnumVARIANT _enumVariant;
        private readonly Func<TSource, TTarget> _resolveFunction;
        private TSource _currentSource;

        public ComEnumerator(IEnumVARIANT enumVariant, Func<TSource, TTarget> resolveFunction)
        {
            _enumVariant = enumVariant;
            _resolveFunction = resolveFunction;
            _bufferLengthPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        object IEnumerator.Current
        {
            get => Current;
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            var hresult = _enumVariant.Next(_buffer.Length, _buffer, _bufferLengthPointer);

            if (hresult != 0)
            {
                Marshal.ThrowExceptionForHR(hresult);
            }

            var bufferLength = Marshal.ReadInt32(_bufferLengthPointer);

            if (bufferLength > 0)
            {
                _currentSource = (TSource) _buffer[0];

                return true;
            }

            _currentSource = default;

            return false;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _enumVariant.Reset();
        }

        /// <inheritdoc />
        public TTarget Current
        {
            // ReSharper disable once EventExceptionNotDocumented
            get => _resolveFunction(_currentSource);
        }

        private void ReleaseUnmanagedResources()
        {
            Marshal.FreeCoTaskMem(_bufferLengthPointer);
        }

        /// <inheritdoc />
        ~ComEnumerator()
        {
            ReleaseUnmanagedResources();
        }
    }
}