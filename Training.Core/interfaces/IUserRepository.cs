using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IUserRepository
    {
        public User GetUserByAccount(string account);
    }
}
