using HayvanSaglik_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class VeterinaryRepository : IVeterinaryRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public VeterinaryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Veterinary DeleteVeterinaryById(int veterinaryId)
        {
            var veterinary = _applicationDbContext.Veterinarians.Include(a => a.HealthInformations).FirstOrDefault(a => a.Id == veterinaryId);
            _applicationDbContext.Remove(veterinary);
            return veterinary;
        }

        public List<HealthInformation> GetHealthInformationsByVeterinary(int veterinaryId)
        {
            var healthInformations = _applicationDbContext.HealthInformations.Where(h => h.VeterinaryId == veterinaryId).ToList();
            return healthInformations;
        }

        public List<Veterinary> GetVeterinarians()
        {
            var veterinarians = _applicationDbContext.Veterinarians.Include(v => v.HealthInformations).ToList();
            return veterinarians;
        }

        public Veterinary GetVeterinaryById(int veterinaryId)
        {
            var veterinary = _applicationDbContext.Veterinarians.Include(v => v.HealthInformations).FirstOrDefault(a => a.Id == veterinaryId);
            return veterinary;
        }
    }
}
