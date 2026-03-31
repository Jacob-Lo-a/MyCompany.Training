using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IUserService
    {
        User ValidateUser(string account, string password);
        
    }

}
