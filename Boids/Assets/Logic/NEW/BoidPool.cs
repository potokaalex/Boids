using System.Collections.Generic;

namespace BoidSimulation
{
    public class BoidPool
    {
        private Stack<Boid> _reserve;
        private BoidFactory _factory;

        public BoidPool(BoidFactory factory, int initialQuantity)
        {
            _reserve = new(initialQuantity);
            _factory = factory;
        }

        public Boid Rent()
        {
            if (_reserve.Count < 1)
                return _factory.Create();

            _reserve.Peek().Transform.gameObject.SetActive(true);

            return _reserve.Pop();
        }

        public void Return(Boid boid)
        {
            boid.Transform.gameObject.SetActive(false);

            _reserve.Push(boid);
        }
    }
}
