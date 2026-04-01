using Microsoft.EntityFrameworkCore;
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


        
        public async Task AddAsync(Order order)
        {

            
            await using (var transaction = await _bookStoreDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _bookStoreDbContext.Orders.AddAsync(order);
                    await _bookStoreDbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            
            

        }
        
        public async Task<List<Order>> GetAllWithUserAsync()
        {
           
            return await _bookStoreDbContext.Orders
                        .Include("User")
                        .ToListAsync();
        }
    }
}
