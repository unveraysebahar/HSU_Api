using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public interface IVeterinaryRepository
    {
        List<Veterinary> GetVeterinarians();
        Veterinary GetVeterinaryById(int veterinaryId);
        Veterinary DeleteVeterinaryById(int veterinaryId);
        List<HealthInformation> GetHealthInformationsByVeterinary(int veterinaryId);
    }
}
