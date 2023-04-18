﻿using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using TMPro;

namespace Statistics
{
    public class FrameStatisticsDisplay : MonoBehaviour
    {
        private const int DisplayFrequency = 10; //1 displaying call for 10 FixedUpdate calls

        [SerializeField] private TextMeshProUGUI _averageFrameTime;
        [SerializeField] private TextMeshProUGUI _averageFPS;

        private FrameTimeCounter _counter = new();
        private int _fixedUpdateCounter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Update()
            => _counter.Update();

        private void FixedUpdate()
        {
            if (_fixedUpdateCounter < DisplayFrequency)
            {
                _fixedUpdateCounter++;
                return;
            }
            else
                _fixedUpdateCounter = 0;

            DisplayFrameTime(_averageFrameTime, _counter.GetAverageFrameTime());
            DisplayFPS(_averageFPS, 1f / _counter.GetAverageFrameTime());
        }

        private void DisplayFPS(TextMeshProUGUI text, float value)
            => text.text = $"FPS: {Math.Round(value, 1)}";

        private void DisplayFrameTime(TextMeshProUGUI text, float value)
            => text.text = $"FrameTime: {Math.Round(value * 1000, 1)} ms";
    }
}