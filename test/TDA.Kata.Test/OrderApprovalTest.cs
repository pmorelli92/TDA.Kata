using System;
using FluentAssertions;
using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Exceptions;
using TDA.Kata.Lib.Handlers;
using TDA.Kata.Test.Facade;
using Xunit;

namespace TDA.Kata.Test
{
    public class OrderApprovalTest
    {
        private InMemOrderRepository _orderRepository;
        private OrderApproval _orderApproval;

        public OrderApprovalTest()
        {
            _orderRepository = new InMemOrderRepository();
            _orderApproval = new OrderApproval(_orderRepository);
        }

        [Fact]
        public void ApprovedExistingOrder()
        {
            var initialOrder = new Order();
            initialOrder.Status = OrderStatus.CREATED;
            initialOrder.Id = 1;
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = true;

            _orderApproval.Handle(request);

            var savedOrder = _orderRepository.getLastSavedOrder();
            savedOrder.Status.Should().Be(OrderStatus.APPROVED);
        }

        [Fact]
        public void RejectedExistingOrder()
        {
            var initialOrder = new Order();
            initialOrder.Status = OrderStatus.CREATED;
            initialOrder.Id = 1;
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = false;

            _orderApproval.Handle(request);

            var savedOrder = _orderRepository.getLastSavedOrder();
            savedOrder.Status.Should().Be(OrderStatus.REJECTED);
        }
        
        [Fact]
        public void CannotApproveRejectedOrder()
        {
            var initialOrder = new Order();
            initialOrder.Status = OrderStatus.REJECTED;
            initialOrder.Id = 1;
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = true;

            Action act = () => _orderApproval.Handle(request);
            
            act.Should().Throw<RejectedOrderCannotBeApprovedException>();
            _orderRepository.getLastSavedOrder().Should().BeNull();
        }
        
        [Fact]
        public void CannotRejectApprovedOrder() {
            var initialOrder = new Order();
            initialOrder.Status = OrderStatus.APPROVED;
            initialOrder.Id = 1;
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = false;

            Action act = () => _orderApproval.Handle(request);

            act.Should().Throw<ApprovedOrderCannotBeRejectedException>();     
            _orderRepository.getLastSavedOrder().Should().BeNull();
        }
        
        [Fact]
        public void ShippedOrdersCannotBeApproved() {
            var initialOrder = new Order();
            initialOrder.Status = OrderStatus.SHIPPED;
            initialOrder.Id = 1;
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = true;

            Action act = () => _orderApproval.Handle(request);
            
            act.Should().Throw<OrderCannotBeShippedTwiceException>();
            _orderRepository.getLastSavedOrder().Should().BeNull();
        }
        
        [Fact]
        public void ShippedOrdersCannotBeRejected() {
            var initialOrder = new Order();
            initialOrder.Status = OrderStatus.SHIPPED;
            initialOrder.Id = 1;
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = false;

            Action act = () => _orderApproval.Handle(request);

            act.Should().Throw<OrderCannotBeShippedTwiceException>();
            _orderRepository.getLastSavedOrder().Should().BeNull();
        }
    }
}