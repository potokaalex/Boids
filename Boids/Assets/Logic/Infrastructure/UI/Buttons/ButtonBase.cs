using UnityEngine.UI;
using UnityEngine;

namespace BoidSimulation.UI
{
    public abstract class ButtonBase : MonoBehaviour
    {
        [SerializeField] private Button _selectableButton;

        private protected ControlPanel ControlPanel;

        public void Initialize(ControlPanel controlPanel)
        {
            ControlPanel = controlPanel;

            _selectableButton.onClick.AddListener(OnClick);
        }

        private protected abstract void OnClick();
    }
}
