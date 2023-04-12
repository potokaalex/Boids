using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Burst;
using UnityEngine;
using Unity.Jobs;

[BurstCompile]
public struct AccelerationJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector2> Positions;
    [ReadOnly] public NativeArray<Vector2> Velocities;
    public NativeArray<Vector2> Accelerations;

    public float AvoidanceDistance;
    public float SightDistance;

    public float CohesionFactor;
    public float SeparationFactor;
    public float AlignmentFactor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(int index)
        => Accelerations[index] = GetAcceleration(index);

    private Vector2 GetAcceleration(int index)
    {
        var averagePosition = Vector2.zero;
        var averageVelocity = Vector2.zero;
        var avarageDirection = Vector2.zero;
        var targetPosition = Positions[index];
        var numberOfNeighbors = 0;

        for (var i = 0; i < Positions.Length; i++)
        {
            if (index == i)
                continue;

            var otherPosition = Positions[i];
            var otherVelocity = Velocities[i];
            var distance = Vector2.Distance(targetPosition, otherPosition);

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
