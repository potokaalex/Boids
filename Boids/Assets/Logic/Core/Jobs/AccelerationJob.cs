using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;
using Extensions;
using System;

[BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
public struct AccelerationJob : IJobParallelFor
{
    public UnsafeArray<float2> Accelerations;
    public UnsafeArray<float2> Velocities;
    public UnsafeArray<float2> Positions;

    public float AvoidanceDistance;
    public float SightDistance;

    public float CohesionFactor;
    public float SeparationFactor;
    public float AlignmentFactor;

    public void Execute(int index)
    {
        var averagePosition = float2.zero;
        var averageVelocity = float2.zero;
        var avarageDirection = float2.zero;
        var targetPosition = Positions[index];
        var numberOfNeighbors = 0;

        for (var i = 0; i < Positions.GetLength(); i++)
        {
            if (index == i)
                continue;

            var otherPosition = Positions[i];
            var distance = GetDistance(targetPosition, otherPosition);

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
            averagePosition -= targetPosition;
        }

        Accelerations[index] =
            GetNormalized(averagePosition) * CohesionFactor +
            GetNormalized(avarageDirection) * SeparationFactor +
            GetNormalized(averageVelocity) * AlignmentFactor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float GetDistance(float2 a, float2 b)
    {
        var length = new float2(a.x - b.x, a.y - b.y);

        return (float)Math.Sqrt(length.x * length.x + length.y * length.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float2 GetNormalized(float2 vector)
    {
        var magnitude = math.length(vector);

        return magnitude == 0 ? float2.zero : vector / magnitude;
    }
}
