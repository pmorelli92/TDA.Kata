using TDA.Kata.Lib.Domain;

namespace TDA.Kata.Lib.Interfaces
{
    public interface IProductCatalog
    {
        Product GetProductByName(string name);
    }
}
