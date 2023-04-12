using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Jobs;

[BurstCompile]
public struct VelocityJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector2> Accelerations;
    [ReadOnly] public NativeArray<Vector2> Positions;
    public NativeArray<Vector2> Velocities;

    public Vector2 AreaSize;
    public float BorderSightDistance;
    public float BorderAvoidanceFactor;

    public float MaximumVelocity;
    public float MinimumVelocity;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(int index)
        => Velocities[index] = GetVelocity(Velocities[index] + Accelerations[index], Positions[index]);

    private Vector2 GetVelocity(Vector2 velocity, Vector2 position)
    {
        if (position.x < BorderSightDistance)
            velocity.x += BorderAvoidanceFactor;

        if (position.x > (AreaSize.x - BorderSightDistance))
            velocity.x -= BorderAvoidanceFactor;

        if (position.y < BorderSightDistance)
            velocity.y += BorderAvoidanceFactor;

        if (position.y > (AreaSize.y - BorderSightDistance))
            velocity.y -= BorderAvoidanceFactor;

        return velocity.normalized * math.clamp
            (velocity.magnitude, MinimumVelocity, MaximumVelocity);
    }
}
