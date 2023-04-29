using System;
using UnityEngine;

namespace BoidSimulation
{
    public class SimulationLoop : MonoBehaviour
    {
        public event Action<float> OnPhysicsUpdate;
        public event Action<float> OnGraphicsUpdate;
        public event Action OnDispose;

        public bool IsPause { get; set; }

        private void Update()
            => OnGraphicsUpdate?.Invoke(Time.deltaTime);

        private void FixedUpdate()
        {
            if (!IsPause)
                OnPhysicsUpdate?.Invoke(Time.fixedDeltaTime);
        }

        private void OnDisable()
            => OnDispose?.Invoke();
    }
}
