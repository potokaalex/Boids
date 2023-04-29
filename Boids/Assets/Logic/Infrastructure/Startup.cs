using BoidSimulation.Data;
using UnityEngine;

namespace BoidSimulation
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private SimulationDataPreset _simulationSettings;
        [SerializeField] private BoidsDataPreset _boidsSettings;
        [SerializeField] private SimulationLoop _simulationLoop;
        [SerializeField] private ControlPanel _controlPanel;

        private SimulationData _simulationData;

        private void Start()
        {
            _simulationData = new(_simulationSettings, _boidsSettings);

            _controlPanel.Initialize(_simulationLoop,_simulationData);

            new Simulation(_simulationLoop, _simulationData);
        }

        private void OnDestroy()
        {
            if (enabled)
                _simulationData.BoidsData.Dispose();
        }
    }
}
