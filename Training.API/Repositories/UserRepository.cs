using Training.Core.interfaces;
using Training.Core.Models;

namespace Training.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreDbContext _dbContext;

        public UserRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User? GetUserByAccount(string account)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Account == account);
        }
    }
}
