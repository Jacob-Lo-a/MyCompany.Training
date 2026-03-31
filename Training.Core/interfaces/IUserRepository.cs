using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IUserRepository
    {
        public User GetUserByAccount(string account);
    }
}
