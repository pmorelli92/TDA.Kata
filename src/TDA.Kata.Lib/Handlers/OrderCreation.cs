using System;
using System.Collections.Generic;
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
            var order = new Order
            {
                Status = OrderStatus.CREATED,
                Items = new List<OrderItem>(),
                Currency = "EUR",
                Total = 0,
                Tax = 0
            };

            foreach (var item in request.Items)
            {
                var product = _productCatalog.GetProductByName(item.ProductName);

                if (product == null)
                    throw new UnknownProductException();

                var unitaryTax = Math.Round(product.Price / 100 * product.Category.TaxPercentage, 2);
                var taxAmount = Math.Round(unitaryTax * item.Quantity);

                var unitaryTaxedAmount = product.Price + unitaryTax;
                var taxedAmount = Math.Round(unitaryTaxedAmount * item.Quantity);

                var orderItem = new OrderItem
                {
                    Product = product,
                    Quantity = item.Quantity,
                    Tax = taxAmount,
                    TaxedAmount = taxedAmount
                };

                order.Items.Add(orderItem);
                order.Total += taxedAmount;
                order.Tax += taxAmount;
            }

            _orderRepository.Save(order);
        }
    }
}
