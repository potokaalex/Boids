using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.CompilerServices;
using Unity.Collections;
using System;

namespace BoidSimulation.Data
{
    public unsafe struct UnsafeRefArray<T> : IDisposable where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction] private T* _ptr;
        private Allocator _allocator;
        private int _length;

        public UnsafeRefArray(int length, Allocator allocator)
        {
            _allocator = allocator;
            _length = length;

            var sizeInBytes = length * sizeof(T);

            _ptr = (T*)UnsafeUtility.Malloc(sizeInBytes, 16, allocator);
            UnsafeUtility.MemClear(_ptr, sizeInBytes);
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckIndexInRange(index);
                return ref _ptr[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int index, ref T value)
        {
            CheckIndexInRange(index);
            _ptr[index] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int index, T value)
        {
            CheckIndexInRange(index);
            _ptr[index] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength()
            => _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            UnsafeUtility.Free(_ptr, _allocator);
            _ptr = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckIndexInRange(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException($"Index {index} must be positive.");

            if (index >= _length)
                throw new IndexOutOfRangeException($"Index {index} is out of range in container of '{_length}' Length.");
        }
    }
}
