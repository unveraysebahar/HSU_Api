using HayvanSaglik_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class VeterinaryClinicRepository : IVeterinaryClinicRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public VeterinaryClinicRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<VeterinaryClinic> Login(string email, string password)
        {
            var veterinaryClinic = await _applicationDbContext.VeterinaryClinics.FirstOrDefaultAsync(x => x.Email == email);

            if (veterinaryClinic == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, veterinaryClinic.PasswordHash, veterinaryClinic.PasswordSalt))
            {
                return null;
            }

            return veterinaryClinic;
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

        public async Task<VeterinaryClinic> Register(VeterinaryClinic veterinaryClinic, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            veterinaryClinic.PasswordHash = passwordHash;
            veterinaryClinic.PasswordSalt = passwordSalt;

            await _applicationDbContext.VeterinaryClinics.AddAsync(veterinaryClinic);
            await _applicationDbContext.SaveChangesAsync();

            return veterinaryClinic;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // Hmacsha512 algorithm
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> VeterinaryClinicExists(string email)
        {
            if (await _applicationDbContext.VeterinaryClinics.AnyAsync(x => x.Email == email))
            {
                return true;
            }

            return false;
        }

        public async Task<VeterinaryClinic> GetVeterinaryClinic(int veterinaryClinicId)
        {
            return await _applicationDbContext.VeterinaryClinics.FirstOrDefaultAsync(x => x.Id == veterinaryClinicId);
        }

        public async Task<VeterinaryClinic> UpdateVeterinaryClinic(VeterinaryClinic veterinaryClinic)
        {
            var result = await _applicationDbContext.VeterinaryClinics.FirstOrDefaultAsync(x => x.Id == veterinaryClinic.Id);

            if(result != null)
            {
                result.RoleId = veterinaryClinic.RoleId;
                /**
                result.Name = veterinaryClinic.Name;
                result.Address = veterinaryClinic.Address;
                result.PhoneNumber = veterinaryClinic.PhoneNumber;
                result.Email = veterinaryClinic.Email;
                result.LicenseDate = veterinaryClinic.LicenseDate;
                result.LicenseNumber = veterinaryClinic.LicenseNumber;
                result.PaymentDepartmentNumber = veterinaryClinic.PaymentDepartmentNumber;
                result.PasswordHash = veterinaryClinic.PasswordHash;
                result.PasswordSalt = veterinaryClinic.PasswordSalt;
                **/

                await _applicationDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public List<Veterinary> GetVeterinariansByVeterinaryClinic(int veterinaryClinicId)
        {
            var veterinarians = _applicationDbContext.Veterinarians.Where(h => h.VeterinaryClinicId == veterinaryClinicId).ToList();
            return veterinarians;
        }

        public List<VeterinaryClinic> GetVeterinaryClinics()
        {
            var veterinaryClinics = _applicationDbContext.VeterinaryClinics.ToList();
            return veterinaryClinics;
        }

        public VeterinaryClinic GetVeterinaryClinicById(int veterinaryClinicId)
        {
            var veterinaryClinic = _applicationDbContext.VeterinaryClinics.FirstOrDefault(a => a.Id == veterinaryClinicId);
            return veterinaryClinic;
        }

        public VeterinaryClinic DeleteVeterinaryClinicById(int veterinaryClinicId)
        {
            var veterinaryClinic = _applicationDbContext.VeterinaryClinics.FirstOrDefault(a => a.Id == veterinaryClinicId);
            _applicationDbContext.Remove(veterinaryClinic);
            return veterinaryClinic;
        }
    }
}
