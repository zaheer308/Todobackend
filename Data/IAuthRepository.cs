using System;
using System.Threading.Tasks;
using TodoBackend.Models;


namespace TodoBackend.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(string user, string password); 
         Task<Object> Login(string username, string pasword);
         Task<bool> UserExists(string username);
    }
}