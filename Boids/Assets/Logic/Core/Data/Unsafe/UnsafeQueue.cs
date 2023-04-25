using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.CompilerServices;
using Unity.Collections;
using System;

namespace BoidSimulation.Data
{
    public unsafe struct UnsafeQueue<T> : IDisposable where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction] private T* _ptr;
        private Allocator _allocator;
        private int _currentLength;
        private int _size;

        private int _readIndex;
        private int _writeIndex;

        public UnsafeQueue(int size, Allocator allocator)
        {
            _allocator = allocator;
            _currentLength = 0;
            _size = size;

            _readIndex = 0;
            _writeIndex = 0;

            var sizeInBytes = size * sizeof(T);

            _ptr = (T*)UnsafeUtility.Malloc(sizeInBytes, 16, allocator);
            UnsafeUtility.MemClear(_ptr, sizeInBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetCurrentLength()
            => _currentLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetSize()
            => _size;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(T value)
        {
            if (!TryEnqueue(value))
                throw new InvalidOperationException("Trying to enqueue into full queue.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Dequeue()
        {
            if (!TryDequeue(out T item))
                throw new InvalidOperationException("Trying to dequeue from an empty queue");

            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryEnqueue(T value)
        {
            if (_currentLength == _size)
                return false;

            _ptr[_writeIndex] = value;
            _writeIndex = GetNextIndex(_writeIndex);
            _currentLength++;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out T item)
        {
            item = _ptr[_readIndex];

            if (_currentLength == 0)
                return false;

            _currentLength--;
            _readIndex = GetNextIndex(_readIndex);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetItems(ref T[] items)
        {
            if (items.Length < _currentLength)
                throw new Exception("the length of the array is less than the number of items");

            for (var i = 0; i < _currentLength; i++)
                items[i] = _ptr[GetNextIndex(_readIndex + i - 1)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            UnsafeUtility.Free(_ptr, _allocator);
            _ptr = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetNextIndex(int currentIndex)
        {
            if (currentIndex >= _size - 1)
                return 0;

            return currentIndex + 1;
        }
    }
}
