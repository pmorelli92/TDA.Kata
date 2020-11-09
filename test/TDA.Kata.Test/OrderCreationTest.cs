using System;
using System.Collections.Generic;
using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Handlers;
using TDA.Kata.Lib.Interfaces;
using TDA.Kata.Test.Facade;
using Xunit;
using FluentAssertions;
using System.Linq;
using TDA.Kata.Lib.Exceptions;

namespace TDA.Kata.Test
{
    public class OrderCreationTest
    {
        private InMemOrderRepository _orderRepository;
        private InMemProductCatalog _productCatalog;

        public OrderCreationTest()
        {
            var foodCategory = new Category { Name = "food", TaxPercentage = 10 };

            _productCatalog = new InMemProductCatalog(new List<Product> {
                new Product { Name = "salad", Price = 3.56M, Category = foodCategory },
                new Product { Name = "tomato", Price = 4.65M, Category = foodCategory },
            });

            _orderRepository = new InMemOrderRepository();
        }

        [Fact]
        public void SellMultipleItems()
        {
            // Setup
            var handler = new OrderCreation(_orderRepository, _productCatalog);
            var sellItemsRequest = new SellItemsRequest
            {
                Items = new List<SellItemRequest> {
                    new SellItemRequest{ ProductName = "salad", Quantity = 2 },
                    new SellItemRequest{ ProductName = "tomato", Quantity = 3 },
                }
            };

            // Exercise
            handler.Handle(sellItemsRequest);

            // Verify
            var actual = _orderRepository.getLastSavedOrder();

            actual.Total.Should().Be(23.20M);
            actual.Tax.Should().Be(2.13M);
            actual.Currency.Should().Be("EUR");
            actual.Items.Count.Should().Be(2);
            actual.Items.Should().HaveCount(2);
            actual.Items.First().Product.Name.Should().Be("salad");
            actual.Items.First().Product.Price.Should().Be(3.56M);
            actual.Items.First().Quantity.Should().Be(2);
            actual.Items.First().TaxedAmount.Should().Be(7.84M);
            actual.Items.First().Tax.Should().Be(0.72M);
            actual.Items.Last().Product.Name.Should().Be("tomato");
            actual.Items.Last().Product.Price.Should().Be(4.65M);
            actual.Items.Last().Quantity.Should().Be(3);
            actual.Items.Last().TaxedAmount.Should().Be(15.36M);
            actual.Items.Last().Tax.Should().Be(1.41M);
        }

        [Fact]
        public void UnknownProduct()
        {
            // Setup
            var handler = new OrderCreation(_orderRepository, _productCatalog);
            var sellItemsRequest = new SellItemsRequest
            {
                Items = new List<SellItemRequest> {
                    new SellItemRequest{ ProductName = "hamburger", Quantity = 1 },
                }
            };

            // Exercise & Verify
            Action act = () => handler.Handle(sellItemsRequest);
            act.Should().ThrowExactly<UnknownProductException>();
        }
    }
}
