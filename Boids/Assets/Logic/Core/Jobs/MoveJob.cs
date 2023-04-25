using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using BoidSimulation.Data;

[BurstCompile]
public struct MoveJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> Velocities;
    public NativeArray<float2> Positions;
    public NativeArray<float> Rotations;
    public BoidsHistory BoidsHistory;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(int index)
    {
        var velocity = Velocities[index];

        Positions[index] += velocity;
        Rotations[index] = Quaternion.LookRotation(
            Vector3.forward, Quaternion.Euler(Vector3.forward) * ((Vector2)velocity).normalized).eulerAngles.z;

        BoidsHistory.AddPosition(index, Positions[index]);
    }
}
