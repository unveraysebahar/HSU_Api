using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class HealthInformationRepository : IHealthInformationRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public HealthInformationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public HealthInformation DeleteHealthInformationById(int healthInformationId)
        {
            var healthInformation = _applicationDbContext.HealthInformations.FirstOrDefault(a => a.Id == healthInformationId);
            _applicationDbContext.Remove(healthInformation);
            return healthInformation;
        }

        public HealthInformation GetHealthInformationById(int id)
        {
            var healthInformation = _applicationDbContext.HealthInformations.FirstOrDefault(h => h.Id == id);
            return healthInformation;
        }
    }
}
