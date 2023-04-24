using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

[BurstCompile(FloatPrecision.Standard, FloatMode.Default)]
public struct AccelerationJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> Positions;
    [ReadOnly] public NativeArray<float2> Velocities;
    [WriteOnly] public NativeArray<float2> Accelerations;

    public float AvoidanceDistance;
    public float SightDistance;

    public float CohesionFactor;
    public float SeparationFactor;
    public float AlignmentFactor;

    public void Execute(int index)
        => Accelerations[index] = GetAcceleration(index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float2 GetAcceleration(int index)
    {
        var averagePosition = float2.zero;
        var averageVelocity = float2.zero;
        var avarageDirection = float2.zero;
        var targetPosition = Positions[index];
        var numberOfNeighbors = 0;

        for (var i = 0; i < Positions.Length; i++)
        {
            if (index == i)
                continue;

            var otherPosition = Positions[i];
            var otherVelocity = Velocities[i];
            var distance = math.distance(targetPosition, otherPosition);

            if (distance < SightDistance)
            {
                averagePosition += otherPosition;
                averageVelocity += otherVelocity;
                numberOfNeighbors += 1;
            }

            if (distance < AvoidanceDistance)
                avarageDirection += targetPosition - otherPosition;
        }

        if (numberOfNeighbors != 0)
        {
            averagePosition /= numberOfNeighbors;
            averagePosition -= targetPosition;
        }

        return GetNormalized(averagePosition) * CohesionFactor +
            GetNormalized(avarageDirection) * SeparationFactor +
            GetNormalized(averageVelocity) * AlignmentFactor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 GetNormalized(Vector2 vector)
    {
        var magnitude = vector.magnitude;

        return magnitude != 0 ? vector / magnitude : Vector2.zero;
    }
}
