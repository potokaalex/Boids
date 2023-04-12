using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using System.Linq;
using System;
using System.Security.Cryptography;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using Unity.Jobs;

public class Pure_Boids : MonoBehaviour
{
    [SerializeField] private GameObject _boidPrefab;
    [SerializeField] private int _numberOfBoids;

    public Vector2 AreaSize;
    public float BorderSightDistance = 150;
    public float BorderAvoidanceFactor = 1;

    public float MaximumVelocity = 15;
    public float MinimumVelocity = 10;

    public float AvoidanceDistance = 20;
    public float SightDistance = 75;

    public float CohesionFactor = 1;
    public float SeparationFactor = 5;
    public float AlignmentFactor = 1;

    //public const int HistorySize = 100;
    //public Queue<Vector2> History;

    private NativeArray<Vector2> _positions;
    private NativeArray<Vector2> _velocities;
    private NativeArray<Vector2> _accelerations;
    private TransformAccessArray _transforms;


    private void Start()
    {
        var transforms = new Transform[_numberOfBoids];
        var velocities = new Vector2[_numberOfBoids];
        var positions = new Vector2[_numberOfBoids];

        for (var i = 0; i < _numberOfBoids; i++)
        {
            transforms[i] = Instantiate(_boidPrefab).transform;
            velocities[i] = GetRandomVelocity();
            positions[i] = GetRandomPosition();
        }

        _transforms = new(transforms);
        _accelerations = new(_numberOfBoids, Allocator.Persistent);
        _velocities = new(velocities, Allocator.Persistent);
        _positions = new(positions, Allocator.Persistent);
    }

    private void FixedUpdate()
    {
        var accelerationJob = new AccelerationJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            Accelerations = _accelerations,
            AvoidanceDistance = AvoidanceDistance,
            SightDistance = SightDistance,
            CohesionFactor = CohesionFactor,
            SeparationFactor = SeparationFactor,
            AlignmentFactor = AlignmentFactor,
        };
        var velocityJob = new VelocityJob()
        {
            Accelerations = _accelerations,
            Positions = _positions,
            Velocities = _velocities,
            AreaSize = AreaSize,
            BorderSightDistance = BorderSightDistance,
            BorderAvoidanceFactor = BorderAvoidanceFactor,
            MaximumVelocity = MaximumVelocity,
            MinimumVelocity = MinimumVelocity,
        };
        var moveJob = new MoveJob()
        {
            Velocities = _velocities,
            Positions = _positions,
        };

        var accelerationJobHandle = accelerationJob.Schedule(_numberOfBoids, 0);
        var velocityJobHandle = velocityJob.Schedule(_numberOfBoids, 0, accelerationJobHandle);
        var moveJobHandle = moveJob.Schedule(_transforms, velocityJobHandle);

        moveJobHandle.Complete();
    }

    private void OnDestroy()
    {
        _transforms.Dispose();
        _accelerations.Dispose();
        _velocities.Dispose();
        _positions.Dispose();
    }

    private Vector2 GetRandomPosition()
    {
        var position = Vector2.zero;

        position.x = Random.Range(0, AreaSize.x + 1);
        position.y = Random.Range(0, AreaSize.y + 1);

        return position;
    }

    private Vector2 GetRandomVelocity()
    {
        var velocity = Vector2.zero;

        velocity.x = Random.Range(MinimumVelocity, MaximumVelocity);
        velocity.y = Random.Range(MinimumVelocity, MaximumVelocity);

        return velocity;
    }
}

public struct Boid
{
    public Vector2 Position;
    public Vector2 Velocity;
}
