using System.Runtime.CompilerServices;
using UnityEngine;

namespace BoidSimulation
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _zoomStep;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;

        private void Update()
        {
            ZoomUpdate(Input.mouseScrollDelta.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ZoomUpdate(float scrollDelta)
        {
            if (scrollDelta == 0)
                return;

            var size = _camera.orthographicSize - Input.mouseScrollDelta.y * _zoomStep;

            _camera.orthographicSize = Mathf.Clamp(size, _minSize, _maxSize);
        }
    }
}
