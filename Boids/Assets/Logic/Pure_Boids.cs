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
    public int width = 150;
    public int height = 150;

    public float AvoidanceDistance = 20;
    public float SightDistance = 75;

    public float alignmentFactor = 0.005f; // adjust velocity by this %
    public float cohesionFactor = 0.05f; // Adjust by this % of average velocity
    public float separationFactor = 0.05f; // Adjust velocity by this %

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
        var velocityJob = new AccelerationJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            Accelerations = _accelerations,
            AvoidanceDistance = AvoidanceDistance,
            SightDistance = SightDistance,
            alignmentFactor = alignmentFactor,
            cohesionFactor = cohesionFactor,
            separationFactor = separationFactor,
        };
        var boundsJob = new BoundsJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            VelocityLimit = 15,
        };
        var moveJob = new MoveJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            Accelerations = _accelerations,
        };

        var handle = moveJob.Schedule(_transforms,
            boundsJob.Schedule(_numberOfBoids, 0,
            velocityJob.Schedule(_numberOfBoids, 0)));

        //var accelerationHandle = accelerationJob.Schedule(_numberOfEntities, 0, boundsHandle);
        //var moveHandle = moveJob.Schedule(_transformAccessArray, accelerationHandle);

        handle.Complete();
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

        position.x = Random.Range(0, width + 1);
        position.y = Random.Range(0, height + 1);

        return position;
    }

    private Vector2 GetRandomVelocity()
    {
        var velocity = Vector2.zero;

        velocity.x = Random.Range(0, 100) - 5;
        velocity.y = Random.Range(0, 100) - 5;

        return velocity;
    }
}

public struct Boid
{
    public Vector2 Position;
    public Vector2 Velocity;
}

/*
—плоченность Ч необходимо найти центр масс (среднее арифметическое координат всех юнитов) и определить вектор, направленный от текущего юнита в центра масс.
–азделение Ч нужно определить среднее направление от всех ближайших юнитов.
¬ыравнивание скоростей Ч необходимо найти среднюю скорость всех юнитов.
ƒвижение к цели Ч вектор, направленный от юнита в сторону цели.
»збегание преп€тствий Ч совпадает со вторым, за исключением того что направление нужно искать в сторону от ближайших преп€тствий.
*/
