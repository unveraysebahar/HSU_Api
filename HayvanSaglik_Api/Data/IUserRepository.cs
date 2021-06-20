using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public interface IUserRepository
    {
        Task<User> Login(string email, string password);
        Task<User> Register(User user, string password);
        Task<bool> UserExists(string email);
        List<Animal> GetAnimalsByUser(int userId);
        User GetUserById(int userId);
        Task<User> GetUser(int userId);
        Task<User> UpdateUser(User user);
    }
}
