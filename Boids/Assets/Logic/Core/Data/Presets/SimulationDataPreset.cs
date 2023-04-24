using System;

namespace BoidSimulation.Data
{
    [Serializable]
    public class SimulationDataPreset
    {
        public float BorderSightDistance;
        public float BorderAvoidanceFactor;

        public float AvoidanceDistance;
        public float SightDistance;

        public float MaximumVelocity;
        public float MinimumVelocity;

        public float CohesionFactor;
        public float SeparationFactor;
        public float AlignmentFactor;
    }
}
