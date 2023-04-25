using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using System;

using UnityEngine;
using UnityEngine.UIElements;

namespace BoidSimulation.Data
{
    public struct BoidsHistory : IDisposable
    {
        private const int HistorySize = 100;
        private UnsafeRefArray<UnsafeQueue<float2>> _positionHistory;
        private int _historyLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsHistory(int boidsCount)
        {
            _positionHistory = new(boidsCount, Allocator.Persistent);
            _historyLength = boidsCount;

            for (var i = 0; i < boidsCount; i++)
                _positionHistory.Set(i, new(HistorySize, Allocator.Persistent));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref UnsafeQueue<float2> GetPositionsQueue(int boidIndex)
            => ref _positionHistory[boidIndex];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHistoryLength()
            => _historyLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddPosition(int boidIndex, float2 position)
        {
            ref var queue = ref _positionHistory[boidIndex];

            if (queue.TryEnqueue(position))
                return;

            queue.Dequeue();
            queue.Enqueue(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            for (var i = 0; i < _historyLength; i++)
                _positionHistory[i].Dispose();

            _positionHistory.Dispose();
        }
    }
}
