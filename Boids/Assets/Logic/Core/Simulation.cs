using Unity.Jobs;
using UnityEngine.Jobs;
using BoidSimulation.Data;

namespace BoidSimulation
{
    public class Simulation
    {
        private SimulationData _simulationData;
        private BoidsRenderer _spriteRenderer;

        public Simulation(SimulationLoop simulationLoop, SimulationData simulationData)
        {
            _simulationData = simulationData;
            _spriteRenderer = new(_simulationData.BoidsData.Sprite, _simulationData.BoidsData.Material);

            simulationLoop.OnFixedUpdate += AccelerationUpdate;
            simulationLoop.OnFixedUpdate += VelocityUpdate;
            simulationLoop.OnFixedUpdate += MoveUpdate;
            simulationLoop.OnGraphicsUpdate += RendererUpdate;
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
                Rotations = _simulationData.BoidsData.Rotations
            };

            moveJob.Schedule(_simulationData.BoidsData.GetInstanceCount(), 0).Complete();
        }

        private void RendererUpdate(float deltaTime)
        {
            _spriteRenderer.Render(
                _simulationData.BoidsData.Positions, _simulationData.BoidsData.Rotations, _simulationData.BoidsData.GetInstanceCount());
        }
    }
}
