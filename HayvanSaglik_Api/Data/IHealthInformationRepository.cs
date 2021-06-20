using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public interface IHealthInformationRepository
    {
        HealthInformation GetHealthInformationById(int id);
        HealthInformation DeleteHealthInformationById(int healthInformationId);
    }
}
