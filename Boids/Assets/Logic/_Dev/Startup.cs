using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;

namespace BoidSimulation
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private SimulationPresetData _settings;
        [SerializeField] private SimulationLoop _simulationLoop;
        [SerializeField] private ControlPanel _controlPanel;

        private DataProvider _dataProvider; 

        private void Start()
        {
            _dataProvider = new DataProvider(_settings);
            var simulation = new Simulation(_simulationLoop, _dataProvider);

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

        public Simulation(SimulationLoop simulationLoop, DataProvider dataProvider)
        {
            _simulationData = dataProvider.SimulationData;
            _boidsData = dataProvider.BoidsData;

            simulationLoop.OnFixedUpdate += AccelerationUpdate;
            simulationLoop.OnFixedUpdate += VelocityUpdate;
            simulationLoop.OnFixedUpdate += MoveUpdate;
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
            var moveJob = new MoveJob()
            {
                Velocities = _boidsData.Velocities,
                Positions = _boidsData.Positions,
            };

            moveJob.Schedule(_boidsData.Transforms).Complete();
        }
    }

    public struct Boid
    {
        public Transform Transform;
        public Vector2 Velocity;
    }
}
