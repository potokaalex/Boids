using System.Runtime.CompilerServices;
using UnityEngine;
using System;

using Random = Unity.Mathematics.Random;
using Object = UnityEngine.Object;

namespace BoidSimulation
{
    public class BoidFactory
    {
        private GameObject _boidPrefab;
        private Random _random;
        private Vector2 _areaSize;
        private float _minimumVelocity;
        private float _maximumVelocity;

        public BoidFactory(GameObject boidPrefab, Vector2 areaSize, float minimumVelocity, float maximumVelocity)
        {
            _boidPrefab = boidPrefab;
            _areaSize = areaSize;
            _minimumVelocity = minimumVelocity;
            _maximumVelocity = maximumVelocity;

            _random = new((uint)DateTime.Now.Ticks);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Boid Create()
        {
            return new()
            {
                //Transform = Object.Instantiate(_boidPrefab, GetRandomPosition(), Quaternion.identity).transform,
                Position = GetRandomPosition(),
                Velocity = GetRandomVelocity()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetRandomPosition()
        {
            return new()
            {
                x = _random.NextFloat(0, _areaSize.x + 1),
                y = _random.NextFloat(0, _areaSize.y + 1),
                z = 0
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 GetRandomVelocity()
        {
            return new()
            {
                x = _random.NextFloat(_minimumVelocity, _maximumVelocity + 1),
                y = _random.NextFloat(_minimumVelocity, _maximumVelocity + 1),
            };
        }
    }
}
