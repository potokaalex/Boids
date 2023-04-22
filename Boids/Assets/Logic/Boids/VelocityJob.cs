using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Jobs;

[BurstCompile]
public struct VelocityJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> Accelerations;
    [ReadOnly] public NativeArray<float2> Positions;
    public NativeArray<float2> Velocities;

    public Vector2 AreaSize; // Vector2 !
    public float BorderSightDistance;
    public float BorderAvoidanceFactor;

    public float MaximumVelocity;
    public float MinimumVelocity;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(int index)
        => Velocities[index] = GetVelocity(Velocities[index] + Accelerations[index], Positions[index]);

    private float2 GetVelocity(float2 velocity, float2 position)
    {
        if (position.x < BorderSightDistance)
            velocity.x += BorderAvoidanceFactor;

        if (position.x > (AreaSize.x - BorderSightDistance))
            velocity.x -= BorderAvoidanceFactor;

        if (position.y < BorderSightDistance)
            velocity.y += BorderAvoidanceFactor;

        if (position.y > (AreaSize.y - BorderSightDistance))
            velocity.y -= BorderAvoidanceFactor;

        return math.normalize(velocity) * math.clamp
            (((Vector2)velocity).magnitude, MinimumVelocity, MaximumVelocity);//math.abs == Vector.magnitude ? 
    }
}
