namespace BoidSimulation.UI
{
    public class CohesionFactorSlider : SettingsSliderBase
    {
        private protected override void OnValueChanged(float value)
        {
            base.OnValueChanged(value);
            ControlPanel.SetCohesionFactor(value);
        }
    }
}
