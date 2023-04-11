using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct AccelerationJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector2> Positions;
    [ReadOnly] public NativeArray<Vector2> Velocities;
    public NativeArray<Vector2> Accelerations;

    public float AvoidanceDistance;
    public float SightDistance;

    public float alignmentFactor;//
    public float cohesionFactor;//
    public float separationFactor;//

    public void Execute(int index)
        => Accelerations[index] = GetAcceleration(index);

    private Vector2 GetAcceleration(int index)
    {
        var averagePosition = Vector2.zero;
        var averageVelocity = Vector2.zero;
        var avarageDirection = Vector2.zero;
        var numberOfNeighbors = 0;

        var targetPosition = Positions[index];

        for (var i = 0; i < Positions.Length; i++)
        {
            if (index == i)
                continue;

            var otherPosition = Positions[i];
            var distance = Vector2.Distance(targetPosition, otherPosition);

            if (distance < SightDistance)
            {
                averagePosition += otherPosition;
                averageVelocity += Velocities[i];
                numberOfNeighbors += 1;
            }

            if (distance < AvoidanceDistance)
                avarageDirection += targetPosition - otherPosition;
        }

        if (numberOfNeighbors != 0)
        {
            averagePosition /= numberOfNeighbors;
            averageVelocity /= numberOfNeighbors;
        }

        return averagePosition * alignmentFactor + 
            averageVelocity * cohesionFactor + 
            avarageDirection * separationFactor;
    }
}
