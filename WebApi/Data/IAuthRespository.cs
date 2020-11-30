using WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public interface IAuthRespository
    {
       Task<User> Register(User user, string password);
       Task<User> Login(string username, string password);
       Task<bool> UserExist(string username);
    }
}
