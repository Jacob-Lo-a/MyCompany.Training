using Microsoft.EntityFrameworkCore.Storage;
using Training.Core;
using Training.Core.interfaces;
using Training.Core.Models;
namespace Training.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreDbContext _bookStoreDbContext;

        public OrderRepository(BookStoreDbContext bookStoreDbContext) 
        {
            _bookStoreDbContext = bookStoreDbContext;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _bookStoreDbContext.Database.BeginTransactionAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _bookStoreDbContext.Orders.AddAsync(order);
        }
        
        public async Task SaveChangesAsync()
        {
            await _bookStoreDbContext.SaveChangesAsync();
        }
    }
}
