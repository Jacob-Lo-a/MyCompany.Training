using Training.Core.interfaces;
using Training.Core.Models;


namespace Training.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
       

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public User ValidateUser(string account, string password)
        {

            var user = _userRepository.GetUserByAccount(account);
            
            if (user == null || user.PasswordHash != password)
            {
                throw new UnauthorizedAccessException("帳號或密碼錯誤");
            }
            
            return user;
        }

       
        

    }
}
