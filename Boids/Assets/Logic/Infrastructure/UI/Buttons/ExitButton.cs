using UnityEditor;

namespace BoidSimulation.UI
{
    public class ExitButton : ButtonBase
    {
        private void Awake()
            => Initialize(null);

        private protected override void OnClick()
#if UNITY_EDITOR
            => EditorApplication.isPlaying = false;
#else
            => UnityEngine.Application.Quit();
#endif
    }
}
