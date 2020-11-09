using TDA.Kata.Lib.Domain;

namespace TDA.Kata.Lib.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order order);
        Order GetById(int orderId);
    }
}
