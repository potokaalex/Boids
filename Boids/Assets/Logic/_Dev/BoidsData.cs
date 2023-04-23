using System.Runtime.CompilerServices;
using Unity.Collections;
using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace BoidSimulation
{
    public class BoidsData : IDisposable
    {
        public NativeArray<float2> Positions;
        public NativeArray<float2> Velocities;
        public NativeArray<float2> Accelerations;
        public BoidsHistory BoidsHistory;

        private int _primaryInstanceCount;
        private int _currentInstanceCount;

        private BoidFactory _factory;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsData(BoidFactory factory, int instanceCount)
        {
            _factory = factory;
            _primaryInstanceCount = instanceCount;

            Initialize(instanceCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetInstanceCount()
            => _currentInstanceCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetInstanceCount(int value)
        {
            if (value == _currentInstanceCount)
                return;

            Dispose();
            Initialize(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
            => SetInstanceCount(_primaryInstanceCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Accelerations.Dispose();
            Velocities.Dispose();
            Positions.Dispose();
            BoidsHistory.Dispose();
        }

        private void Initialize(int instanceCount)
        {
            var velocities = new float2[instanceCount];
            var positions = new float2[instanceCount];

            for (var i = 0; i < instanceCount; i++)
            {
                var boid = _factory.Create();

                velocities[i] = boid.Velocity;
                positions[i] = boid.Position;
            }

            Velocities = new(velocities, Allocator.Persistent);
            Positions = new(positions, Allocator.Persistent);
            Accelerations = new(instanceCount, Allocator.Persistent);
            BoidsHistory = new(instanceCount);
            _currentInstanceCount = instanceCount;
        }
    }


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
