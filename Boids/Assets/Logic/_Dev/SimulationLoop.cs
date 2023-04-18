using System;
using UnityEngine;

namespace BoidSimulation
{
    public class SimulationLoop : MonoBehaviour
    {
        //public event Action<float> OnSimulationUpdate;
        public event Action<float> OnFixedUpdate;
        public event Action<float> OnUpdate;

        private void Update()
            => OnUpdate?.Invoke(Time.deltaTime);

        private void FixedUpdate()
            => OnFixedUpdate?.Invoke(Time.fixedDeltaTime);
    }
}
