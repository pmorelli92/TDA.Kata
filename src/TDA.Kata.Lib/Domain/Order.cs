using System.Collections.Generic;
using System.Linq;

using TDA.Kata.Lib.Exceptions;

namespace TDA.Kata.Lib.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; }
        public string Currency { get; }
        public List<OrderItem> Items { get; }
        public decimal Tax { get; }
        public OrderStatus Status { get; set; }

        public Order(List<OrderItem> items)
        {
            if (items == null)
                Items = new List<OrderItem>();

            Status = OrderStatus.CREATED;
            Items = items;
            Currency = "EUR";
            Tax = Items.Sum(e => e.Tax);
            Total = Items.Sum(e => e.TaxedAmount);
        }

        public Order(OrderStatus status, List<OrderItem> items, int id, string currency)
        {
            Id = id;
            Status = status;
            Items = items;
            Currency = "EUR";
            Tax = Items.Sum(e => e.Tax);
            Total = Items.Sum(e => e.TaxedAmount);
        }

        public Order()
        {
            Status = OrderStatus.CREATED;
            Items = new List<OrderItem>();
            Currency = "EUR";
        }

        public Order Approve()
        {
            if (Status == OrderStatus.SHIPPED)
                throw new OrderCannotBeShippedTwiceException();

            if (Status == OrderStatus.REJECTED)
                throw new RejectedOrderCannotBeApprovedException();

            return new Order(OrderStatus.APPROVED, Items, Id, Currency);
        }

        public Order Reject()
        {
            if (Status == OrderStatus.SHIPPED)
                throw new OrderCannotBeShippedTwiceException();

            if (Status == OrderStatus.APPROVED)
                throw new ApprovedOrderCannotBeRejectedException();

            return new Order(OrderStatus.REJECTED, Items, Id, Currency);
        }
    }
}
