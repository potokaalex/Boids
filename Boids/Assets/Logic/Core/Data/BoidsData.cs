using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using System;

namespace BoidSimulation.Data
{
    public class BoidsData : IDisposable
    {
        public Material Material;
        public Sprite Sprite;

        public NativeArray<float2> Positions;
        public NativeArray<float2> Velocities;
        public NativeArray<float2> Accelerations;
        public NativeArray<float> Rotations;
        public BoidsHistory BoidsHistory;

        private BoidsDataPreset _dataPreset;
        private BoidFactory _factory;
        private int _instanceCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsData(BoidFactory factory, BoidsDataPreset dataPreset)
        {
            Material = dataPreset.Material;
            Sprite = dataPreset.Sprite;
            _factory = factory;

            InitializeArrays(0);
            SetInstanceCount(dataPreset.InstanceCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetInstanceCount()
            => _instanceCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetInstanceCount(int value)
        {
            if (value == _instanceCount)
                return;

            Dispose();
            InitializeArrays(value);

            _instanceCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Material = _dataPreset.Material;
            Sprite = _dataPreset.Sprite;

            SetInstanceCount(_dataPreset.InstanceCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Accelerations.Dispose();
            Velocities.Dispose();
            Positions.Dispose();
            Rotations.Dispose();
            BoidsHistory.Dispose();
        }

        private void InitializeArrays(int instanceCount)
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
            Rotations = new(instanceCount, Allocator.Persistent);
            Accelerations = new(instanceCount, Allocator.Persistent);
            BoidsHistory = new(instanceCount);
        }
    }
}
