using UnityEngine;

namespace BoidSimulation
{
    public class DataProvider
    {
        public SimulationData SimulationData;
        public BoidsData BoidsData;

        public DataProvider(SimulationPresetData presetData)
        {
            SimulationData = new()
            {
                AreaSize = new(Screen.width, Screen.height),
                MaximumVelocity = presetData.MaximumVelocity,
                MinimumVelocity = presetData.MinimumVelocity,
                AvoidanceDistance = presetData.AvoidanceDistance,
                SightDistance = presetData.SightDistance,
                BorderSightDistance = presetData.BorderSightDistance,
                BorderAvoidanceFactor = presetData.BorderAvoidanceFactor,
                CohesionFactor = presetData.CohesionFactor,
                SeparationFactor = presetData.SeparationFactor,
                AlignmentFactor = presetData.AlignmentFactor
            };

            var factory = new BoidFactory
                (presetData.BoidPrefab, SimulationData.AreaSize, SimulationData.MinimumVelocity, SimulationData.MaximumVelocity);

            BoidsData = new(new(factory, presetData.NumberOfBoids), presetData.NumberOfBoids);
        }

        public void Reset()
        {

        }
    }
}
