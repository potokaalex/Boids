namespace BoidSimulation.UI
{
    public class AvoidanceDistanceSlider : SettingsSliderBase
    {
        private protected override void OnValueChanged(float value)
        {
            base.OnValueChanged(value);
            ControlPanel.SetAvoidanceDistance(value);
        }
    }
}
