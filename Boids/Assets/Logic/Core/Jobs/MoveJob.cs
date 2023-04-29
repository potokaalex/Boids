using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Jobs;
using Extensions;
using Unity.Burst;

[BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
public struct MoveJob : IJobParallelFor
{
    public UnsafeArray<float2> Accelerations;
    public UnsafeArray<float2> Velocities;
    public UnsafeArray<float2> Positions;
    public UnsafeArray<float> Rotations;

    public float2 AreaSize;
    public float BorderSightDistance;
    public float BorderAvoidanceFactor;

    public float MaximumVelocity;
    public float MinimumVelocity;

    public void Execute(int index)
    {
        var velocity = GetVelocity(Velocities[index], Positions[index], Accelerations[index]);

        Velocities[index] = velocity;
        Positions[index] += velocity;
        Rotations[index] = GetRotation(velocity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float2 GetVelocity(float2 velocity, float2 position, float2 acceleration)
    {
        var newVelocity = velocity + acceleration;

        if (position.x < BorderSightDistance)
            newVelocity.x += BorderAvoidanceFactor;

        else if (position.x > (AreaSize.x - BorderSightDistance))
            newVelocity.x -= BorderAvoidanceFactor;

        if (position.y < BorderSightDistance)
            newVelocity.y += BorderAvoidanceFactor;

        else if (position.y > (AreaSize.y - BorderSightDistance))
            newVelocity.y -= BorderAvoidanceFactor;

        return math.normalize(newVelocity) *
            math.clamp(math.length(newVelocity), MinimumVelocity, MaximumVelocity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float GetRotation(float2 velocity)
    {
        var angle = math.atan2(velocity.y, velocity.x);

        if (angle > 0)
            angle -= math.PI / 2;
        else
            angle += math.PI / 2 * 3;

        return angle;
    }
}
