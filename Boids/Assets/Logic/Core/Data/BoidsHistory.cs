using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Mathematics;
using System;

namespace BoidSimulation.Data
{
    public struct BoidsHistory : IDisposable
    {
        private const int HistorySize = 100;
        private UnsafeList<UnsafeRingQueue<float2>> _positionHistory;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsHistory(int boidsCount)
        {
            _positionHistory = new(boidsCount, Allocator.Persistent);

            for (var i = 0; i < _positionHistory.Length; i++)
                _positionHistory[i] = new(HistorySize, Allocator.Persistent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeRingQueue<float2> GetPositions(int boidIndex)
            => _positionHistory[boidIndex];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPosition(int boidIndex, float2 position)
        {
            if (_positionHistory[boidIndex].TryEnqueue(position))
                return;

            _positionHistory[boidIndex].Dequeue();
            _positionHistory[boidIndex].Enqueue(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            for (var i = 0; i < _positionHistory.Length; i++)
                _positionHistory[i].Dispose();

            _positionHistory.Dispose();
        }
    }
}
