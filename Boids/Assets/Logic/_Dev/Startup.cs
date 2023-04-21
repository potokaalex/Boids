using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.UIElements.CurveField;

namespace BoidSimulation
{
    public class Startup : MonoBehaviour
    {
        public Sprite boidSprite;
        public Material boidMaterial;

        [SerializeField] private SimulationPresetData _settings;
        [SerializeField] private SimulationLoop _simulationLoop;
        [SerializeField] private ControlPanel _controlPanel;

        private DataProvider _dataProvider;

        private void Start()
        {
            _dataProvider = new DataProvider(_settings);
            var simulation = new Simulation(_simulationLoop, _dataProvider, boidSprite, boidMaterial);

            _controlPanel.Initialize(_dataProvider);
        }

        private void OnDestroy()
        {
            if (!enabled)
                return;

            _dataProvider.BoidsData.Dispose();
        }
    }

    public class Simulation
    {
        private SimulationData _simulationData;
        private BoidsData _boidsData;

        public Simulation(SimulationLoop simulationLoop, DataProvider dataProvider, Sprite boidSprite, Material boidMaterial)
        {
            _simulationData = dataProvider.SimulationData;
            _boidsData = dataProvider.BoidsData;

            simulationLoop.OnFixedUpdate += AccelerationUpdate;
            simulationLoop.OnFixedUpdate += VelocityUpdate;
            simulationLoop.OnFixedUpdate += MoveUpdate;
            simulationLoop.OnUpdate += RendererUpdate;

            //
            pb = new MaterialPropertyBlock();

            ri = RenderInfo.FromSprite(boidMaterial, boidSprite);
        }

        private void AccelerationUpdate(float deltaTime)// 20 times per sec
        {
            var accelerationJob = new AccelerationJob()
            {
                Positions = _boidsData.Positions,
                Velocities = _boidsData.Velocities,
                Accelerations = _boidsData.Accelerations,
                AvoidanceDistance = _simulationData.AvoidanceDistance,
                SightDistance = _simulationData.SightDistance,
                CohesionFactor = _simulationData.CohesionFactor,
                SeparationFactor = _simulationData.SeparationFactor,
                AlignmentFactor = _simulationData.AlignmentFactor,
            };

            accelerationJob.Schedule(_boidsData.Transforms.length, 0).Complete();
        }

        private void VelocityUpdate(float deltaTime)// 60 times per sec ?
        {
            var velocityJob = new VelocityJob()
            {
                Accelerations = _boidsData.Accelerations,
                Positions = _boidsData.Positions,
                Velocities = _boidsData.Velocities,
                AreaSize = _simulationData.AreaSize,
                BorderSightDistance = _simulationData.BorderSightDistance,
                BorderAvoidanceFactor = _simulationData.BorderAvoidanceFactor,
                MaximumVelocity = _simulationData.MaximumVelocity,
                MinimumVelocity = _simulationData.MinimumVelocity,
            };

            velocityJob.Schedule(_boidsData.Transforms.length, 0).Complete();
        }

        private void MoveUpdate(float deltaTime)// 60 times per sec
        {
            for (var i = 0; i < _boidsData._activeBoids.Count; i++)
            {
                var velss = _boidsData.Velocities;

                var boid = _boidsData._activeBoids[i];

                boid.Position += velss[i];
                _boidsData.Positions[i] += velss[i];

                _boidsData._activeBoids[i] = boid;
            }

        }

        //
        private static readonly int posDirPropertyId = Shader.PropertyToID("posDirBuffer");
        private const int batchSize = 1023;

        private readonly Vector4[] posDirArr = new Vector4[batchSize];
        private MaterialPropertyBlock pb;

        private RenderInfo ri;

        private void RendererUpdate(float deltaTime)// 60 times per sec
        {
            for (var i = 0; i < batchSize; i++)
            {
                var boid = _boidsData._activeBoids[i];
                //Debug.Log(boid.Position);
                posDirArr[i] = new Vector4(boid.Position.x, boid.Position.y, 0, 0);
            }

            pb.SetVectorArray(posDirPropertyId, posDirArr);
            //ComputeBuffer
            //Graphics.DrawMesh(
            //Graphics.DrawMesh(ri.mesh, Vector3.zero, Quaternion.identity, ri.mat, 0);
            Graphics.DrawMeshInstancedProcedural
                (ri.mesh, 0, ri.mat, new Bounds(Vector3.zero, _simulationData.AreaSize), batchSize, pb, ShadowCastingMode.Off, false);
        }
    }

    public struct Boid
    {
        //public Transform Transform;
        public Vector2 Position;
        public Vector2 Velocity;
    }

    [BurstCompile]
    public struct RendererJob : IJobParallelForTransform
    {
        public NativeArray<Vector2> Positions;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(int index, TransformAccess transform)
        {
        }
    }
}
