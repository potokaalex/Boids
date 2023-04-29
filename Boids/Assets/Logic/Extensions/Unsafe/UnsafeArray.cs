using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.CompilerServices;
using Unity.Collections;
using System;

namespace Extensions
{
    public unsafe struct UnsafeArray<T> : IDisposable where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction] private T* _ptr;
        private Allocator _allocator;
        private int _length;

        public UnsafeArray(int length, Allocator allocator)
        {
            _allocator = allocator;
            _length = length;

            var sizeInBytes = length * sizeof(T);

            _ptr = (T*)UnsafeUtility.Malloc(sizeInBytes, 16, allocator);
            UnsafeUtility.MemClear(_ptr, sizeInBytes);
        }

        public UnsafeArray(T[] items, Allocator allocator)
        {
            _allocator = allocator;
            _length = items.Length;

            var sizeInBytes = _length * sizeof(T);

            _ptr = (T*)UnsafeUtility.Malloc(sizeInBytes, 16, allocator);
            UnsafeUtility.MemClear(_ptr, sizeInBytes);

            for (var i = 0; i < _length; i++)
                _ptr[i] = items[i];
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr[index];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _ptr[index] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength()
            => _length;

        public void Dispose()
        {
            UnsafeUtility.Free(_ptr, _allocator);
            _ptr = null;
        }
    }
}
