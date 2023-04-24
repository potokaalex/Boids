namespace BoidSimulation.UI
{
    public class ResetButton : ButtonBase
    {
        private protected override void OnClick()
            => ControlPanel.SimulationReset();
    }
}
