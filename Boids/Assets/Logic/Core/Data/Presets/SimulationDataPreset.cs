using UnityEngine;
using System;

namespace BoidSimulation.Data
{
    [Serializable]
    public class SimulationDataPreset
    {
        public Vector2 AreaSize;
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
