namespace BoidSimulation.UI
{
    public class AlignmentFactorSlider : SettingsSliderBase
    {
        private protected override void OnValueChanged(float value)
        {
            base.OnValueChanged(value);
            ControlPanel.SetAlignmentFactor(value);
        }
    }
}
