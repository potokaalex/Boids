using System.Runtime.CompilerServices;
using UnityEngine;
using System;

using Random = Unity.Mathematics.Random;

namespace BoidSimulation
{
    public class BoidFactory
    {
        private Random _random;
        private Vector2 _areaSize;
        private float _minimumVelocity;
        private float _maximumVelocity;

        public BoidFactory(Vector2 areaSize, float minimumVelocity, float maximumVelocity)
        {
            _random = new((uint)DateTime.Now.Ticks);
            _areaSize = areaSize;
            _minimumVelocity = minimumVelocity;
            _maximumVelocity = maximumVelocity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (Vector2 Position, Vector2 Velocity) Create()
        {
            return new()
            {
                Position = GetRandomPosition(),
                Velocity = GetRandomVelocity()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 GetRandomPosition()
        {
            return new()
            {
                x = _random.NextFloat(0, _areaSize.x + 1),
                y = _random.NextFloat(0, _areaSize.y + 1)
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
