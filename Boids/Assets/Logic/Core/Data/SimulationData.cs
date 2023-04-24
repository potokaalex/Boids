using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace BoidSimulation.Data
{
    public class SimulationData
    {
        public BoidsData BoidsData;

        public float2 AreaSize;

        public float MaximumVelocity;
        public float MinimumVelocity;

        public float BorderAvoidanceFactor;
        public float AvoidanceDistance;

        public float SightDistance;
        public float BorderSightDistance;

        public float CohesionFactor;
        public float SeparationFactor;
        public float AlignmentFactor;

        public bool IsTracePaths;

        private SimulationDataPreset _simulationDataPreset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SimulationData(SimulationDataPreset simulationDataPreset, BoidsDataPreset boidsDataPreset)
        {
            Initialize(simulationDataPreset);

            _simulationDataPreset = simulationDataPreset;
            BoidsData = new(new(AreaSize, MaximumVelocity, MinimumVelocity), boidsDataPreset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Initialize(_simulationDataPreset);
            BoidsData.Reset();
        }

        private void Initialize(SimulationDataPreset simulationDataPreset)
        {
            AreaSize = new(Screen.width, Screen.height);

            MaximumVelocity = simulationDataPreset.MaximumVelocity;
            MinimumVelocity = simulationDataPreset.MinimumVelocity;

            BorderAvoidanceFactor = simulationDataPreset.BorderAvoidanceFactor;
            AvoidanceDistance = simulationDataPreset.AvoidanceDistance;

            SightDistance = simulationDataPreset.SightDistance;
            BorderSightDistance = simulationDataPreset.BorderSightDistance;

            CohesionFactor = simulationDataPreset.CohesionFactor;
            SeparationFactor = simulationDataPreset.SeparationFactor;
            AlignmentFactor = simulationDataPreset.AlignmentFactor;

            IsTracePaths = false;
        }
    }
}
