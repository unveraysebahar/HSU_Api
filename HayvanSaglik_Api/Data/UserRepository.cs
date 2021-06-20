using HayvanSaglik_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) // Hmacsha512 algorithm
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

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _applicationDbContext.Users.AddAsync(user);
            await _applicationDbContext.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // Hmacsha512 algorithm
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _applicationDbContext.Users.AnyAsync(x => x.Email == email))
            {
                return true;
            }

            return false;
        }

        public List<Animal> GetAnimalsByUser(int userId)
        {
            var animals = _applicationDbContext.Animals.Where(a => a.UserId == userId).ToList();
            return animals;
        }

        public User GetUserById(int userId)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(a => a.Id == userId);
            return user;
        }

        public async Task<User> GetUser(int userId)
        {
            return await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> UpdateUser(User user)
        {
            var result = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (result != null)
            {
                result.FullName = user.FullName;
                result.PhoneNumber = user.PhoneNumber;
                result.Email = user.Email;

                await _applicationDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
