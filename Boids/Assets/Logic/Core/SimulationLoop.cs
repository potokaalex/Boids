using System;
using UnityEngine;

namespace BoidSimulation
{
    public class SimulationLoop : MonoBehaviour
    {
        private const int PhysicsUpdateFactor = 20;//1 physics update call for 20 FixedUpdate calls

        public event Action<float> OnPhysicsUpdate;
        public event Action<float> OnGraphicsUpdate;

        public event Action<float> OnFixedUpdate;//


        private int _fixedUpdateCounter;

        private void Update()
            => OnGraphicsUpdate?.Invoke(Time.deltaTime);

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke(Time.fixedDeltaTime);

            if (_fixedUpdateCounter < PhysicsUpdateFactor)
            {
                _fixedUpdateCounter++;
                return;
            }
            else
                _fixedUpdateCounter = 0;

            OnPhysicsUpdate?.Invoke(Time.fixedDeltaTime * PhysicsUpdateFactor);
        }
    }
}
