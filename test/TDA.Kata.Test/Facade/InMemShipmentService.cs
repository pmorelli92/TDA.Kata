using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Interfaces;

namespace TDA.Kata.Test.Facade
{
    public class InMemShipmentService : IShipmentService
    {
        private Order _lastShippedOrder;

        internal Order getLastShippedOrder()
            => _lastShippedOrder;

        public void Ship(Order order)
            => _lastShippedOrder = order;
    }
}
