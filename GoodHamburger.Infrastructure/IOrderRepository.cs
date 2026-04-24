using GoodHamburger.Domain;

namespace GoodHamburger.Infrastructure;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetAllAsync();
    Task<Order> AddAsync(Order order);
    Task<Order?> UpdateAsync(Order order);
    Task DeleteAsync(int id);
}
