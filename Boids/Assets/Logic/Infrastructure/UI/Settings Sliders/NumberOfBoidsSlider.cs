namespace BoidSimulation.UI
{
    public class NumberOfBoidsSlider : SettingsSliderBase
    {
        private protected override void OnValueChanged(float value)
        {
            base.OnValueChanged(value);
            ControlPanel.SetNumberOfBoids((int)value);
        }
    }
}
