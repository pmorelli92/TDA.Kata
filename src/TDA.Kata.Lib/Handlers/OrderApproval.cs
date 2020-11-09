using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Exceptions;
using TDA.Kata.Lib.Interfaces;

namespace TDA.Kata.Lib.Handlers
{
    public class OrderApprovalRequest
    {
        public int OrderId { get; set; }

        public bool Approved { get; set; }
    }

    public class OrderApproval
    {
        private IOrderRepository _orderRepository;
        public OrderApproval(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Handle(OrderApprovalRequest request)
        {
            var order = _orderRepository.GetById(request.OrderId);

            if (order.Status == OrderStatus.SHIPPED)
                throw new OrderCannotBeShippedTwiceException();

            if (request.Approved && order.Status == OrderStatus.REJECTED)
                throw new RejectedOrderCannotBeApprovedException();

            if (!request.Approved && order.Status == OrderStatus.APPROVED)
                throw new ApprovedOrderCannotBeRejectedException();

            order.Status = request.Approved ? OrderStatus.APPROVED : OrderStatus.REJECTED;
            _orderRepository.Save(order);
        }
    }
}
