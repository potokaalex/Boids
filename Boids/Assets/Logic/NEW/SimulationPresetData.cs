using System;
using UnityEngine;

namespace BoidSimulation
{
    [Serializable]
    public class SimulationPresetData
    {
        public GameObject BoidPrefab;

        public float BorderSightDistance;
        public float BorderAvoidanceFactor;

        public float AvoidanceDistance;
        public float SightDistance;

        public float MaximumVelocity;
        public float MinimumVelocity;

        public float CohesionFactor;
        public float SeparationFactor;
        public float AlignmentFactor;

        public int NumberOfBoids;
    }
}
