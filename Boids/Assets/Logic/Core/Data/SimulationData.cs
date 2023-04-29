using Unity.Mathematics;

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

        private SimulationDataPreset _simulationDataPreset;

        public SimulationData(SimulationDataPreset simulationDataPreset, BoidsDataPreset boidsDataPreset)
        {
            Initialize(simulationDataPreset);

            _simulationDataPreset = simulationDataPreset;
            BoidsData = new(new(AreaSize, MaximumVelocity, MinimumVelocity), boidsDataPreset);
        }

        public void Reset()
        {
            Initialize(_simulationDataPreset);
            BoidsData.Reset();
        }

        private void Initialize(SimulationDataPreset simulationDataPreset)
        {
            AreaSize = simulationDataPreset.AreaSize;
            MaximumVelocity = simulationDataPreset.MaximumVelocity;
            MinimumVelocity = simulationDataPreset.MinimumVelocity;

            BorderAvoidanceFactor = simulationDataPreset.BorderAvoidanceFactor;
            AvoidanceDistance = simulationDataPreset.AvoidanceDistance;

            SightDistance = simulationDataPreset.SightDistance;
            BorderSightDistance = simulationDataPreset.BorderSightDistance;

            CohesionFactor = simulationDataPreset.CohesionFactor;
            SeparationFactor = simulationDataPreset.SeparationFactor;
            AlignmentFactor = simulationDataPreset.AlignmentFactor;
        }
    }
}
