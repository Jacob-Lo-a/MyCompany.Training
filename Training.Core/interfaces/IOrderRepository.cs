using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetAllWithUserAsync();
        Task<List<Order>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}
