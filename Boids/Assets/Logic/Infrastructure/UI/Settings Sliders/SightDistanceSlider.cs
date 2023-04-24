namespace BoidSimulation.UI
{
    public class SightDistanceSlider : SettingsSliderBase
    {
        private protected override void OnValueChanged(float value)
        {
            base.OnValueChanged(value);
            ControlPanel.SetSightDistance(value);
        }
    }
}
