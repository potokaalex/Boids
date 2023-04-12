using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.UIElements;

public class Pure
{
    private Boid[] _boids;
    private GameObject _boidPrefab;
    private int _numberOfBoids;
    private int visualRange = 75;
    private int width = 150;
    private int height = 150;

    void Init()
    {
        _boids = new Boid[_numberOfBoids];

        for (var i = 0; i < _numberOfBoids; i++)
        {
            _boids[i] = new();
            _boids[i].Position = UnityEngine.Object.Instantiate(_boidPrefab).transform.position;
            //_boids[i].History = new(HistorySize);

            InitPosition(_boids[i]);
            InitVelocity(_boids[i]);
        }
    }

    private void InitPosition(Boid boid)
    {
        var pos = Vector2.zero;

        pos.x = UnityEngine.Random.Range(0, width + 1);
        pos.y = UnityEngine.Random.Range(0, height + 1);

        boid.Position = pos;
    }

    private void InitVelocity(Boid boid)
    {
        boid.Velocity.x = UnityEngine.Random.Range(0, 100) - 5;
        boid.Velocity.y = UnityEngine.Random.Range(0, 100) - 5;
    }

    void Update()
    {
        foreach (var boid in _boids)
        {
            //velocity
            FlyTowardsCenter(boid);
            AvoidOthers(boid);
            MatchVelocity(boid);

            //limit
            LimitSpeed(boid);
            KeepWithinBounds(boid);

            //move
            UpdateHistory(boid);
            UpdatePosition(boid);
        }
    }

    private void FlyTowardsCenter(Boid boid)
    {
        var centeringFactor = 0.005f; // adjust velocity by this %
        var centerPosition = Vector2.zero;
        var numNeighbors = 0;

        foreach (var otherBoid in _boids)
        {
            if (GetDistance(boid, otherBoid) < visualRange)
            {
                centerPosition += otherBoid.Position;
                numNeighbors += 1;
            }
        }

        if (numNeighbors != 0)
        {
            centerPosition /= numNeighbors;
            boid.Velocity += (centerPosition - boid.Position) * centeringFactor;
        }
    }

    private void AvoidOthers(Boid boid)
    {
        var minDistance = 20; // The distance to stay away from other boids
        var avoidFactor = 0.05f; // Adjust velocity by this %
        var movePos = Vector2.zero;
        //var moveY = 0;

        foreach (var otherBoid in _boids)
        {
            //if (otherBoid.ID != boid.ID)
            {
                if (Vector2.Distance(boid.Position, otherBoid.Position) < minDistance)
                {
                    movePos += (Vector2)(boid.Position - otherBoid.Position);
                }
            }
        }

        boid.Velocity += movePos * avoidFactor;
    }

    private void MatchVelocity(Boid boid)
    {
        var matchingFactor = 0.05f; // Adjust by this % of average velocity

        var avgVelocity = Vector2.zero;
        var numNeighbors = 0;

        foreach (var otherBoid in _boids)
        {
            if (Vector2.Distance(boid.Position, otherBoid.Position) < visualRange)
            {
                avgVelocity += otherBoid.Velocity;
                numNeighbors += 1;
            }
        }

        if (numNeighbors != 0)
        {
            avgVelocity /= numNeighbors;
            boid.Velocity += (avgVelocity - boid.Velocity) * matchingFactor;
        }
    }

    private void LimitSpeed(Boid boid)
    {
        var speedLimit = 15;
        var speed = boid.Velocity.magnitude;// Math.sqrt(boid.dx * boid.dx + boid.dy * boid.dy);

        if (speed > speedLimit)
        {
            boid.Velocity = boid.Velocity / speed * speedLimit;
            //boid.Velocity *= speedLimit / speed ;
        }
    }

    private void KeepWithinBounds(Boid boid)
    {
        var margin = 200;
        var turnFactor = 1;
        var pos = boid.Position;

        if (pos.x < margin)
        {
            boid.Velocity.x += turnFactor;
        }
        if (pos.x > (width - margin))
        {
            boid.Velocity.x -= turnFactor;
        }
        if (pos.y < margin)
        {
            boid.Velocity.y += turnFactor;
        }
        if (pos.y > (height - margin))
        {
            boid.Velocity.y -= turnFactor;
        }
    }

    private void UpdatePosition(Boid boid)
    {
        //_boids[boid.ID].
        //boid.Transform.Translate(boid.Velocity);
    }

    private void UpdateHistory(Boid boid)
    {
        //boid.History.Enqueue(boid.Transform.position);
    }

    private float GetDistance(Boid a, Boid b)
    {
        return Vector2.Distance(a.Position, b.Position);
    }
}
