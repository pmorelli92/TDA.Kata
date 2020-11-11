using System.Collections.Generic;
using System.Linq;
using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Interfaces;

namespace TDA.Kata.Test.Facade
{
    public class InMemOrderRepository : IOrderRepository
    {
        private Order _lastSavedOrder;
        private readonly List<Order> _orders = new List<Order>();

        internal Order getLastSavedOrder()
            => _lastSavedOrder;

        public Order GetById(int orderId)
            => _orders.FirstOrDefault(e => e.Id == orderId);

        public void Save(Order order)
        {
            _lastSavedOrder = order;
            _orders.Add(order);
        }

        public void AddOrder(Order order) 
            => _orders.Add(order);
    }
}
