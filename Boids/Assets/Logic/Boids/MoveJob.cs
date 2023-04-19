using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine.Jobs;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public struct MoveJob : IJobParallelForTransform
{
    [ReadOnly] public NativeArray<Vector2> Velocities;
    public NativeArray<Vector2> Positions;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(int index, TransformAccess transform)
    {
        var velocity = Velocities[index];

        Positions[index] += velocity;
        //transform.position = Positions[index];
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 1) * velocity.normalized);
    }
}
