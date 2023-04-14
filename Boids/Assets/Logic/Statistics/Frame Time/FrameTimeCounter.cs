using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace Statistics
{
    public class FrameTimeCounter
    {
        private const int BufferSize = 10;

        private Queue<float> _frameBuffer;
        private float _averageFrameTime;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FrameTimeCounter()
            => InitializeBuffer();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            UpdateBuffer();
            Count();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetAverageFrameTime()
            => _averageFrameTime;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeBuffer()
            => _frameBuffer = new(BufferSize);

        private void UpdateBuffer()
        {
            _frameBuffer.Enqueue(Time.unscaledDeltaTime);

            if (_frameBuffer.Count > BufferSize)
                _frameBuffer.Dequeue();
        }

        private void Count()
        {
            var sum = 0f;

            foreach (var frameRenderingTime in _frameBuffer)
                sum += frameRenderingTime;

            _averageFrameTime = sum / _frameBuffer.Count;
        }
    }
}
