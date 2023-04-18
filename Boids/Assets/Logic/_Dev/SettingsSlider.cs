using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace BoidSimulation
{
    public class SettingsSlider : MonoBehaviour
    {
        [SerializeField] private TMP_InputField CurrentValue;
        [SerializeField] private Slider Slider;

        public event Action<float> OnValueChanged;

        private void Awake()
        {
            Slider.onValueChanged.AddListener(OnValueChangedInvoke);
            CurrentValue.onEndEdit.AddListener((str) => OnValueChangedInvoke(float.Parse(str)));
        }

        public void ChangeValue(float newValue)
            => Slider.value = newValue;

        private void OnValueChangedInvoke(float value)
        {
            CurrentValue.SetTextWithoutNotify(Math.Round(value, 1).ToString());
            Slider.SetValueWithoutNotify(value);
            OnValueChanged?.Invoke(value);
        }
    }
}
