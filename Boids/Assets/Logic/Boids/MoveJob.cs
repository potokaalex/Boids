using System.Drawing;
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct MoveJob : IJobParallelForTransform
{
    [ReadOnly] public NativeArray<Vector2> Accelerations;
    public NativeArray<Vector2> Positions;
    public NativeArray<Vector2> Velocities;

    public void Execute(int index, TransformAccess transform)
    {
        Velocities[index] += Accelerations[index];
        Positions[index] += Velocities[index];

        transform.position = Positions[index];
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 1) * Velocities[index].normalized);
    }
}
