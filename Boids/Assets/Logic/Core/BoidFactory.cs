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
        private float _maximumVelocity;
        private float _minimumVelocity;

        public BoidFactory(Vector2 areaSize, float maximumVelocity, float minimumVelocity)
        {
            _random = new((uint)DateTime.Now.Ticks);
            _areaSize = areaSize;
            _maximumVelocity = maximumVelocity;
            _minimumVelocity = minimumVelocity;
        }

        public (Vector2 Position, Vector2 Velocity) Create()
        {
            return new()
            {
                Position = GetRandomPosition(),
                Velocity = GetRandomVelocity()
            };
        }

        private Vector2 GetRandomPosition()
        {
            return new()
            {
                x = _random.NextFloat(0, _areaSize.x + 1),
                y = _random.NextFloat(0, _areaSize.y + 1)
            };
        }

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
