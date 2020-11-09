using System.Collections.Generic;
using TDA.Kata.Lib.Domain;
using TDA.Kata.Lib.Interfaces;
using System.Linq;

namespace TDA.Kata.Test.Facade
{
    public class InMemProductCatalog : IProductCatalog
    {
        private List<Product> _products = new List<Product>();

        public InMemProductCatalog(List<Product> products)
        {
            _products = products;
        }

        public Product GetProductByName(string name)
            => _products.FirstOrDefault(e => e.Name == name);
    }
}
