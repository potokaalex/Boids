using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Jobs;

[BurstCompile]
public struct MoveJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> Velocities;
    public NativeArray<float2> Positions;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(int index)
    {
        var velocity = Velocities[index];

        Positions[index] += velocity;
        //transform.position = Positions[index];
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 1) * velocity.normalized);
    }
}
