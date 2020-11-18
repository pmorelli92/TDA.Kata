using System;

namespace TDA.Kata.Lib.Domain
{
    public class Product
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }

        public decimal GetUnitaryTax()
            => Math.Round(Price / 100 * Category.TaxPercentage, 2, MidpointRounding.AwayFromZero);
    }
}
