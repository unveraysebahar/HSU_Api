using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public interface IAdminRepository
    {
        Task<Admin> Login(string email, string password);
        Task<Admin> Register(Admin admin, string password);
        Task<bool> AdminExists(string email);
        Admin GetAdminById(int adminId);
    }
}
