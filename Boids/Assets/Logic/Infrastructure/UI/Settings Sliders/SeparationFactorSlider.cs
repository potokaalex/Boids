namespace BoidSimulation.UI
{
    public class SeparationFactorSlider : SettingsSliderBase
    {
        private protected override void OnValueChanged(float value)
        {
            base.OnValueChanged(value);
            ControlPanel.SetSeparationFactor(value);
        }
    }
}
