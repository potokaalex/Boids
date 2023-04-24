using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace BoidSimulation.UI
{
    public abstract class SettingsSliderBase : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _currentValue;
        [SerializeField] private Slider _slider;

        private protected ControlPanel ControlPanel;

        public void Initialize(ControlPanel controlPanel, float value)
        {
            ControlPanel = controlPanel;

            OnValueChanged(value);

            _slider.onValueChanged.AddListener(OnValueChanged);
            _currentValue.onEndEdit.AddListener((str) => OnValueChanged(float.Parse(str)));
        }

        private protected virtual void OnValueChanged(float value)
        {
            _currentValue.SetTextWithoutNotify(Math.Round(value, 1).ToString());
            _slider.SetValueWithoutNotify(value);
        }
    }
}
