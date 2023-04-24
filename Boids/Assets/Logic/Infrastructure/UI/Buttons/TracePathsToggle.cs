namespace BoidSimulation.UI
{
    public class TracePathsToggle : ButtonBase
    {
        private protected override void OnClick()
            => ControlPanel.TracePathsToggle();
    }
}
