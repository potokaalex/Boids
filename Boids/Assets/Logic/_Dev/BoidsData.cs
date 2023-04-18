using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Jobs;
using UnityEngine;
using System;

namespace BoidSimulation
{
    public class BoidsData : IDisposable
    {
        public const int HistorySize = 100;

        public TransformAccessArray Transforms;
        public NativeArray<Vector2> Positions;
        public NativeArray<Vector2> Velocities;
        public NativeArray<Vector2> Accelerations;
        public NativeArray<Vector2> Accelerations2;

        //public List<Queue<Vector2>> History;//castom native array ?!?!
        private List<Boid> _activeBoids;
        private BoidPool _boidPool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsData(BoidPool pool, int initialQuantity)
        {
            _activeBoids = new(initialQuantity);
            _boidPool = pool;

            CalculateActiveBoids(initialQuantity);
            CalculateBoidsArrays();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeNumberOfBoids(int newValue)
        {
            if (newValue == Transforms.length)
                return;

            Dispose();
            CalculateActiveBoids(newValue);
            CalculateBoidsArrays();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Transforms.Dispose();
            Accelerations.Dispose();
            Velocities.Dispose();
            Positions.Dispose();
        }

        private void CalculateActiveBoids(int activeBoidsCount)
        {
            foreach (var boid in _activeBoids)
                _boidPool.Return(boid);

            _activeBoids.Clear();

            for (var i = 0; i < activeBoidsCount; i++)
                _activeBoids.Add(_boidPool.Rent());
        }

        private void CalculateBoidsArrays()
        {
            var transforms = new Transform[_activeBoids.Count];
            var velocities = new Vector2[_activeBoids.Count];
            var positions = new Vector2[_activeBoids.Count];

            for (var i = 0; i < _activeBoids.Count; i++)
            {
                transforms[i] = _activeBoids[i].Transform;
                velocities[i] = _activeBoids[i].Velocity;
                positions[i] = _activeBoids[i].Transform.position;
            }

            Transforms = new(transforms);
            Accelerations = new(_activeBoids.Count, Allocator.Persistent);
            Velocities = new(velocities, Allocator.Persistent);
            Positions = new(positions, Allocator.Persistent);
        }
    }
}
