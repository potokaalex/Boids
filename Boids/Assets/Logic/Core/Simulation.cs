using Unity.Jobs;
using UnityEngine.Jobs;
using BoidSimulation.Data;
using System;
using TMPro;

namespace BoidSimulation
{
    public class Simulation : IDisposable
    {
        private SimulationLoop _simulationLoop;
        private SimulationData _simulationData;
        private BoidsRenderer _spriteRenderer;
        private PathsRenderer _pathsRenderer;

        public Simulation(SimulationLoop simulationLoop, SimulationData simulationData)
        {
            _simulationLoop = simulationLoop;
            _simulationData = simulationData;
            _spriteRenderer = new(_simulationData.BoidsData.BoidSprite, _simulationData.BoidsData.BoidMaterial);
            _pathsRenderer = new(simulationData.BoidsData.PathsMaterial);

            simulationLoop.OnFixedUpdate += AccelerationUpdate;
            simulationLoop.OnFixedUpdate += VelocityUpdate;
            simulationLoop.OnFixedUpdate += MoveUpdate;
            simulationLoop.OnGraphicsUpdate += RendererUpdate;
            simulationLoop.OnDispose += Dispose;
        }

        private void AccelerationUpdate(float deltaTime)
        {
            var accelerationJob = new AccelerationJob()
            {
                Positions = _simulationData.BoidsData.Positions,
                Velocities = _simulationData.BoidsData.Velocities,
                Accelerations = _simulationData.BoidsData.Accelerations,
                AvoidanceDistance = _simulationData.AvoidanceDistance,
                SightDistance = _simulationData.SightDistance,
                CohesionFactor = _simulationData.CohesionFactor,
                SeparationFactor = _simulationData.SeparationFactor,
                AlignmentFactor = _simulationData.AlignmentFactor,
            };

            accelerationJob.Schedule(_simulationData.BoidsData.GetInstanceCount(), 0).Complete();
        }

        private void VelocityUpdate(float deltaTime)
        {
            var velocityJob = new VelocityJob()
            {
                Accelerations = _simulationData.BoidsData.Accelerations,
                Positions = _simulationData.BoidsData.Positions,
                Velocities = _simulationData.BoidsData.Velocities,
                AreaSize = _simulationData.AreaSize,
                BorderSightDistance = _simulationData.BorderSightDistance,
                BorderAvoidanceFactor = _simulationData.BorderAvoidanceFactor,
                MaximumVelocity = _simulationData.MaximumVelocity,
                MinimumVelocity = _simulationData.MinimumVelocity,
            };

            velocityJob.Schedule(_simulationData.BoidsData.GetInstanceCount(), 0).Complete();
        }

        private void MoveUpdate(float deltaTime)
        {
            var moveJob = new MoveJob()
            {
                Velocities = _simulationData.BoidsData.Velocities,
                Positions = _simulationData.BoidsData.Positions,
                Rotations = _simulationData.BoidsData.Rotations,
                BoidsHistory = _simulationData.BoidsData.BoidsHistory
            };

            moveJob.Schedule(_simulationData.BoidsData.GetInstanceCount(), 0).Complete();
        }

        private void RendererUpdate(float deltaTime)
        {
            //UnityEngine.Debug.Log(_simulationData.BoidsData.BoidsHistory.GetPositions(0).GetCurrentLength());

            //if (_simulationData.IsTracePaths)
            _pathsRenderer.Render(_simulationData.BoidsData.BoidsHistory);

            _spriteRenderer.Render(
                _simulationData.BoidsData.Positions, _simulationData.BoidsData.Rotations, _simulationData.BoidsData.GetInstanceCount());
        }

        public void Dispose()
        {
            _simulationLoop.OnFixedUpdate -= AccelerationUpdate;
            _simulationLoop.OnFixedUpdate -= VelocityUpdate;
            _simulationLoop.OnFixedUpdate -= MoveUpdate;
            _simulationLoop.OnGraphicsUpdate -= RendererUpdate;
            _simulationLoop.OnDispose -= Dispose;

            _pathsRenderer.Dispose();
        }
    }
}
