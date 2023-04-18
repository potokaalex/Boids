using UnityEngine;

namespace BoidSimulation
{
    public class SimulationData
    {
        public Vector2 AreaSize;

        public float MaximumVelocity;
        public float MinimumVelocity;

        public float AvoidanceDistance;
        public float SightDistance;

        public float BorderSightDistance;
        public float BorderAvoidanceFactor;

        public float CohesionFactor;
        public float SeparationFactor;
        public float AlignmentFactor;

        public bool IsTracePaths;
    }
}
