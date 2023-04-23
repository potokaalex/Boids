using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
//using static UnityEditor.UIElements.CurveField;

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

        private BoidsRenderer _spriteRenderer;

        public Simulation(SimulationLoop simulationLoop, DataProvider dataProvider, Sprite boidSprite, Material boidMaterial)
        {
            _simulationData = dataProvider.SimulationData;
            _boidsData = dataProvider.BoidsData;

            simulationLoop.OnFixedUpdate += AccelerationUpdate;
            simulationLoop.OnFixedUpdate += VelocityUpdate;
            simulationLoop.OnFixedUpdate += MoveUpdate;
            simulationLoop.OnUpdate += RendererUpdate;

            //

            _spriteRenderer = new(boidSprite, boidMaterial);
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



        private void RendererUpdate(float deltaTime)
            => _spriteRenderer.Render(_boidsData.Positions, _boidsData.Positions, _boidsData.GetInstanceCount());
    }

    public class BoidsRenderer
    {
        private const string ShaderPropertyName = "posDirBuffer";
        private const int BatchSize = 1023;

        private readonly int _propertyId;
        private MaterialPropertyBlock _propertyBlock;
        private Material _material;
        private Mesh _mesh;

        private Vector4[] posDirBuffer;
        private float2[] positionsBuffer;
        private float2[] directionsBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsRenderer(Sprite sprite, Material material)
        {
            _propertyId = Shader.PropertyToID(ShaderPropertyName);
            _propertyBlock = new();
            _material = material;
            _mesh = GetMesh(sprite);

            posDirBuffer = new Vector4[BatchSize];
            positionsBuffer = Array.Empty<float2>();
            directionsBuffer = Array.Empty<float2>();
        }

        public void Render(NativeArray<float2> positions, NativeArray<float2> directions, int count)
        {
            Copy(positions, ref positionsBuffer);
            Copy(directions, ref directionsBuffer);

            Render(positionsBuffer, directionsBuffer, count);

            void Copy(NativeArray<float2> src, ref float2[] dst)
            {
                if (dst.Length != src.Length)
                    dst = new float2[count];

                src.CopyTo(dst);
            }
        }

        public void Render(float2[] positions, float2[] rotations, int count)
        {
            for (var done = 0; done < count; done += BatchSize)
            {
                var run = math.min(count - done, BatchSize);

                for (var i = 0; i < run; i++)
                {
                    var position = positions[i + done];
                    var rotation = rotations[i + done];

                    posDirBuffer[i] = new(rotation.x, position.y, rotation.x, rotation.y);
                }

                _propertyBlock.SetVectorArray(_propertyId, posDirBuffer);

                Graphics.DrawMeshInstancedProcedural
                    (_mesh, 0, _material, new Bounds(Vector3.zero, Vector3.one * float.MaxValue), run, _propertyBlock, ShadowCastingMode.Off, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Mesh GetMesh(Sprite sprite)
        {
            return new()
            {
                vertices = sprite.vertices.Select(v => (Vector3)v).ToArray(),
                triangles = sprite.triangles.Select(t => (int)t).ToArray(),
                uv = sprite.uv
            };
        }
    }
}
