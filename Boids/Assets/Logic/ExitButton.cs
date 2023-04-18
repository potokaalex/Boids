using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

namespace BoidSimulation
{
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private Button Button;

        private void Awake()
            => Button.onClick.AddListener(ApplicationExit);

        private void ApplicationExit()
#if UNITY_EDITOR
            => EditorApplication.isPlaying = false;
#else
            => Application.Quit();
#endif
    }
}