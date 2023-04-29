namespace BoidSimulation.UI
{
    public class PauseButton : ButtonBase
    {
        private protected override void OnClick()
            => ControlPanel.SimulationPause();
    }
}
