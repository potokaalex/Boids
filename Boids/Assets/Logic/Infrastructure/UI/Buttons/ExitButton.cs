using UnityEditor;

namespace BoidSimulation.UI
{
    public class ExitButton : ButtonBase
    {
        private protected override void OnClick()
#if UNITY_EDITOR
            => EditorApplication.isPlaying = false;
#else
            => Application.Quit();
#endif
    }
}
