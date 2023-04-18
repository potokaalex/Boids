using UnityEngine.UI;
using UnityEngine;

namespace BoidSimulation
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private SettingsSlider CohesionFactor;
        [SerializeField] private SettingsSlider SeparationFactor;
        [SerializeField] private SettingsSlider AlignmentFactor;

        [SerializeField] private SettingsSlider AvoidanceDistance;
        [SerializeField] private SettingsSlider SightDistance;
        [SerializeField] private SettingsSlider NumberOfBoids;

        [SerializeField] private Button Reset;
        [SerializeField] private Button TracePaths;

        private SimulationData _simulationData;
        private BoidsData _boidsData;
        private DataProvider _dataProvider;

        public void Initialize(DataProvider dataProvider)
        {
            _simulationData = dataProvider.SimulationData;
            _boidsData = dataProvider.BoidsData;
            _dataProvider = dataProvider;

            CohesionFactor.ChangeValue(_simulationData.CohesionFactor);
            SeparationFactor.ChangeValue(_simulationData.SeparationFactor);
            AlignmentFactor.ChangeValue(_simulationData.AlignmentFactor);
            AvoidanceDistance.ChangeValue(_simulationData.AvoidanceDistance);
            SightDistance.ChangeValue(_simulationData.SightDistance);
            NumberOfBoids.ChangeValue(_boidsData.Transforms.length);

            CohesionFactor.OnValueChanged += SetCohesionFactor;
            SeparationFactor.OnValueChanged += SetSeparationFactor;
            AlignmentFactor.OnValueChanged += SetAlignmentFactor;
            AvoidanceDistance.OnValueChanged += SetAvoidanceDistance;
            SightDistance.OnValueChanged += SetSightDistance;
            NumberOfBoids.OnValueChanged += ChangeNumberOfBoids;

            Reset.onClick.AddListener(SimulationReset);
            TracePaths.onClick.AddListener(TracePathsToggle);
        }

        private void SetCohesionFactor(float value)
            => _simulationData.CohesionFactor = value;

        private void SetSeparationFactor(float value)
            => _simulationData.SeparationFactor = value;

        private void SetAlignmentFactor(float value)
            => _simulationData.AlignmentFactor = value;

        private void SetAvoidanceDistance(float value)
            => _simulationData.AvoidanceDistance = value;

        private void SetSightDistance(float value)
            => _simulationData.SightDistance = value;

        private void ChangeNumberOfBoids(float newValue)
            => _boidsData.ChangeNumberOfBoids((int)newValue);

        private void SimulationReset()
            => _dataProvider.Reset();

        private void TracePathsToggle()
            => _simulationData.IsTracePaths = !_simulationData.IsTracePaths;
    }
}
