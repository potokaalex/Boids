using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


[BurstCompile]
public struct BoundsJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector2> Positions;
    public NativeArray<Vector2> Velocities;
    public float VelocityLimit;// = 15

    public void Execute(int index)
    {
        //Velocities

        LimitSpeed(index);
        KeepWithinBounds(index);
    }

    private void LimitSpeed(int index)
    {
        var magnitude = Velocities[index].magnitude;

        if (magnitude > VelocityLimit)
        {
            Velocities[index] = Velocities[index] * VelocityLimit / magnitude;
            //Velocities[index] *= VelocityLimit / magnitude;
        }
    }

    private void KeepWithinBounds(int index)
    {
        var margin = 120;
        var turnFactor = 1;
        var pos = Positions[index];
        var vel = Velocities[index];

        if (pos.x < margin)
        {
            vel.x += turnFactor;
        }
        if (pos.x > (1920 - margin))
        {
            vel.x -= turnFactor;
        }
        if (pos.y < margin)
        {
            vel.y += turnFactor;
        }
        if (pos.y > (1080 - margin))
        {
            vel.y -= turnFactor;
        }

        Velocities[index] = vel;
    }
}
