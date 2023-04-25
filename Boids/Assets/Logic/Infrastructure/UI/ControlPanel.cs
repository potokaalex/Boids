using BoidSimulation.Data;
using BoidSimulation.UI;
using UnityEngine;

namespace BoidSimulation
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private CohesionFactorSlider CohesionFactor;
        [SerializeField] private SeparationFactorSlider SeparationFactor;
        [SerializeField] private AlignmentFactorSlider AlignmentFactor;
        [SerializeField] private AvoidanceDistanceSlider AvoidanceDistance;
        [SerializeField] private SightDistanceSlider SightDistance;
        [SerializeField] private NumberOfBoidsSlider NumberOfBoids;

        [SerializeField] private ResetButton Reset;
        [SerializeField] private TracePathsToggle TracePaths;

        private SimulationData _simulationData;

        public void Initialize(SimulationData simulationData)
        {
            _simulationData = simulationData;

            InitializeSliders();

            Reset.Initialize(this);
            TracePaths.Initialize(this);
        }

        public void SetCohesionFactor(float value)
            => _simulationData.CohesionFactor = value;

        public void SetSeparationFactor(float value)
            => _simulationData.SeparationFactor = value;

        public void SetAlignmentFactor(float value)
            => _simulationData.AlignmentFactor = value;

        public void SetAvoidanceDistance(float value)
            => _simulationData.AvoidanceDistance = value;

        public void SetSightDistance(float value)
            => _simulationData.SightDistance = value;

        public void SetNumberOfBoids(int value)
            => _simulationData.BoidsData.SetInstanceCount(value);

        public void SimulationReset()
        {
            _simulationData.Reset();
            InitializeSliders();
        }

        public void TracePathsToggle()
            => _simulationData.IsTracePaths = !_simulationData.IsTracePaths;

        private void InitializeSliders()
        {
            CohesionFactor.Initialize(this, _simulationData.CohesionFactor);
            SeparationFactor.Initialize(this, _simulationData.SeparationFactor);
            AlignmentFactor.Initialize(this, _simulationData.AlignmentFactor);
            AvoidanceDistance.Initialize(this, _simulationData.AvoidanceDistance);
            SightDistance.Initialize(this, _simulationData.SightDistance);
            NumberOfBoids.Initialize(this, _simulationData.BoidsData.GetInstanceCount());
        }
    }
}
