using System;

using TDA.Kata.Lib.Exceptions;

namespace TDA.Kata.Lib.Domain
{
    public class OrderItem
    {
        public Product Product { get; }
        public int Quantity { get; }
        public decimal TaxedAmount { get; }
        public decimal Tax { get; }

        public OrderItem(Product product, int quantity)
        {
            Product = product ?? throw new UnknownProductException();

            Quantity = quantity;
            var unitaryTax = Product.GetUnitaryTax();
            Tax = CalculateTaxAmount(unitaryTax);
            TaxedAmount = CalculateTaxedAmount(unitaryTax);
        }

        private decimal CalculateTaxAmount(decimal unitaryTax)
            => Math.Round(unitaryTax * Quantity, 2, MidpointRounding.AwayFromZero);

        private decimal CalculateTaxedAmount(decimal unitaryTax)
        {
            var unitaryTaxedAmount = Product.Price + unitaryTax;
            return Math.Round(unitaryTaxedAmount * Quantity, 2, MidpointRounding.AwayFromZero);
        }

    }
}
