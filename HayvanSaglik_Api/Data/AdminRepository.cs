using HayvanSaglik_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class AdminRepository : IAdminRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Admin> Login(string email, string password)
        {
            var admin = await _applicationDbContext.Admins.FirstOrDefaultAsync(x => x.Email == email);

            if (admin == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, admin.PasswordHash, admin.PasswordSalt))
            {
                return null;
            }

            return admin;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public async Task<Admin> Register(Admin admin, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            admin.PasswordHash = passwordHash;
            admin.PasswordSalt = passwordSalt;

            await _applicationDbContext.Admins.AddAsync(admin);
            await _applicationDbContext.SaveChangesAsync();

            return admin;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> AdminExists(string email)
        {
            if (await _applicationDbContext.Admins.AnyAsync(x => x.Email == email))
            {
                return true;
            }

            return false;
        }

        public Admin GetAdminById(int adminId)
        {
            var admin = _applicationDbContext.Admins.FirstOrDefault(a => a.Id == adminId);
            return admin;
        }
    }
}
