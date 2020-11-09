using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Exceptions;
using TDA.Kata.Lib.Interfaces;

namespace TDA.Kata.Lib.Handlers
{
    public class OrderShipmentRequest
    {
        public int OrderId { get; set; }
    }

    public class OrderShipment
    {
        private IOrderRepository _orderRepository;
        private IShipmentService _shipmentService;

        public OrderShipment(IOrderRepository orderRepository, IShipmentService shipmentService)
        {
            _orderRepository = orderRepository;
            _shipmentService = shipmentService;
        }

        public void Handle(OrderShipmentRequest request)
        {
            var order = _orderRepository.GetById(request.OrderId);

            if (order.Status == OrderStatus.CREATED || order.Status == OrderStatus.REJECTED)
                throw new OrderCannotBeShippedException();

            if (order.Status == OrderStatus.SHIPPED)
                throw new OrderCannotBeShippedTwiceException();

            _shipmentService.Ship(order);
            order.Status = OrderStatus.SHIPPED;
            _orderRepository.Save(order);
        }
    }
}
