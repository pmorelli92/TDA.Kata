using System;
using System.Collections.Generic;
using System.Linq;

using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Exceptions;
using TDA.Kata.Lib.Interfaces;

namespace TDA.Kata.Lib.Handlers
{
    public class SellItemRequest
    {
        public int Quantity { get; set; }

        public string ProductName { get; set; }
    }

    public class SellItemsRequest
    {
        public List<SellItemRequest> Items { get; set; }
    }

    public class OrderCreation
    {
        private IProductCatalog _productCatalog;
        private IOrderRepository _orderRepository;

        public OrderCreation(IOrderRepository orderRepository, IProductCatalog productCatalog)
        {
            _productCatalog = productCatalog;
            _orderRepository = orderRepository;
        }

        public void Handle(SellItemsRequest request)
        {
            var itemList = request.Items?.Select(MapOrderItem)
                                         .ToList();

            var order = new Order(itemList);

            _orderRepository.Save(order);
        }

        public OrderItem MapOrderItem(SellItemRequest item)
        {
            var product = _productCatalog.GetProductByName(item.ProductName);
            return new OrderItem(product, item.Quantity);
        }
    }
}
