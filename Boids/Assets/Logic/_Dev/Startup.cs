using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
        private BoidsDataProvider _boidsData;

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

            accelerationJob.Schedule(_boidsData.GetInstanceCount(), 0).Complete();
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

            velocityJob.Schedule(_boidsData.GetInstanceCount(), 0).Complete();
        }

        private void MoveUpdate(float deltaTime)// 60 times per sec
        {
            var moveJob = new MoveJob()
            {
                Velocities = _boidsData.Velocities,
                Positions = _boidsData.Positions
            };

            moveJob.Schedule(_boidsData.GetInstanceCount(), 0).Complete();
        }

        private static readonly int posDirPropertyId = Shader.PropertyToID("posDirBuffer");
        private const int batchSize = 1023;

        private readonly Vector4[] posDirArr = new Vector4[batchSize];
        private MaterialPropertyBlock pb;

        private RenderInfo ri;

        private void RendererUpdate(float deltaTime)// 60 times per sec
        {
            for (int done = 0; done < _boidsData.GetInstanceCount(); done += batchSize)
            {
                int run = Mathf.Min(_boidsData.GetInstanceCount() - done, batchSize);

                for (var i = 0; i < run; i++)
                {
                    var position = _boidsData.Positions[done + i];

                    posDirArr[i] = new Vector4(position.x, position.y, 0, 0);
                }

                pb.SetVectorArray(posDirPropertyId, posDirArr);
                //Debug.Log("DRAW !");
                Graphics.DrawMeshInstancedProcedural
                    (ri.mesh, 0, ri.mat, new Bounds(Vector3.zero, _simulationData.AreaSize), run, pb, ShadowCastingMode.Off, false);
            }
        }
    }

    [BurstCompile]
    public struct RendererJob : IJobParallelFor
    {
        public NativeArray<Vector2> Positions;

        public void Execute(int index)
        {

        }
    }
}
