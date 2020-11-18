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
        private readonly IOrderRepository _orderRepository;
        public OrderApproval(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Handle(OrderApprovalRequest request)
        {
            var order = _orderRepository.GetById(request.OrderId);

            _orderRepository.Save(request.Approved
                                    ? order.Approve()
                                    : order.Reject());
        }
    }
}
