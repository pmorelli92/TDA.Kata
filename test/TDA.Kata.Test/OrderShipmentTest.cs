using System;
using FluentAssertions;
using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Exceptions;
using TDA.Kata.Lib.Handlers;
using TDA.Kata.Test.Facade;
using Xunit;

namespace TDA.Kata.Test
{
    public class OrderShipmentTest
    {
        private InMemOrderRepository _orderRepository;
        private InMemShipmentService _inMemShipmentService;
        private OrderShipment _orderShipment;
        
        public OrderShipmentTest()
        {
            _orderRepository = new InMemOrderRepository();
            _inMemShipmentService = new InMemShipmentService();
            
            _orderShipment = new OrderShipment(_orderRepository, _inMemShipmentService);
        }

        [Fact]
        public void ShipApprovedOrder()
        {
            var initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.APPROVED;
            _orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            _orderShipment.Handle(request);

            _orderRepository.getLastSavedOrder().Status.Should().Be(OrderStatus.SHIPPED);
            _inMemShipmentService.getLastShippedOrder().Should().Be(initialOrder);
        } 
        
        [Fact]
        public void CreatedOrdersCannotBeShipped()  {
            var initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.CREATED;
            _orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            Action act = () => _orderShipment.Handle(request);

            act.Should().Throw<OrderCannotBeShippedException>();
            _orderRepository.getLastSavedOrder().Should().BeNull();
            _inMemShipmentService.getLastShippedOrder().Should().BeNull();
        }

        [Fact]
        public void RejectedOrdersCannotBeShipped()  {
            var initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.REJECTED;
            _orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            Action act = () => _orderShipment.Handle(request);

            act.Should().Throw<OrderCannotBeShippedException>();
            _orderRepository.getLastSavedOrder().Should().BeNull();
            _inMemShipmentService.getLastShippedOrder().Should().BeNull();
        }

        [Fact]
        public void ShippedOrdersCannotBeShippedAgain()  {
            var initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.SHIPPED;
            _orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            Action act  = () => _orderShipment.Handle(request);
            
            act.Should().Throw<OrderCannotBeShippedTwiceException>();
            _orderRepository.getLastSavedOrder().Should().BeNull();
            _inMemShipmentService.getLastShippedOrder().Should().BeNull();
        }
    }
}