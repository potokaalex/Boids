using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Extensions;
using System;

namespace BoidSimulation.Data
{
    public class BoidsData : IDisposable
    {
        public Material BoidMaterial;
        public Sprite BoidSprite;

        public UnsafeArray<float2> Positions;
        public UnsafeArray<float2> Velocities;
        public UnsafeArray<float2> Accelerations;
        public UnsafeArray<float> Rotations;

        private BoidsDataPreset _dataPreset;
        private BoidFactory _factory;
        private int _instanceCount;

        public BoidsData(BoidFactory factory, BoidsDataPreset dataPreset)
        {
            BoidMaterial = dataPreset.BoidMaterial;
            BoidSprite = dataPreset.BoidSprite;
            _factory = factory;
            _dataPreset = dataPreset;

            InitializeArrays(0);
            SetInstanceCount(dataPreset.InstanceCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetInstanceCount()
            => _instanceCount;

        public void SetInstanceCount(int value)
        {
            if (value == _instanceCount)
                return;

            Dispose();
            InitializeArrays(value);

            _instanceCount = value;
        }

        public void Reset()
        {
            BoidMaterial = _dataPreset.BoidMaterial;
            BoidSprite = _dataPreset.BoidSprite;

            SetInstanceCount(_dataPreset.InstanceCount);
        }

        public void Dispose()
        {
            Accelerations.Dispose();
            Velocities.Dispose();
            Positions.Dispose();
            Rotations.Dispose();
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
        }
    }
}
