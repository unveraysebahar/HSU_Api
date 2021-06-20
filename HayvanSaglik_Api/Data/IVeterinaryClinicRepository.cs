using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public interface IVeterinaryClinicRepository
    {
        List<VeterinaryClinic> GetVeterinaryClinics();
        Task<VeterinaryClinic> Login(string email, string password);
        Task<VeterinaryClinic> Register(VeterinaryClinic veterinaryClinic, string password);
        Task<bool> VeterinaryClinicExists(string email);
        Task<VeterinaryClinic> GetVeterinaryClinic(int veterinaryClinicId);
        VeterinaryClinic DeleteVeterinaryClinicById(int veterinaryClinicId);
        Task<VeterinaryClinic> UpdateVeterinaryClinic(VeterinaryClinic veterinaryClinic);
        List<Veterinary> GetVeterinariansByVeterinaryClinic(int veterinaryClinicId);
        VeterinaryClinic GetVeterinaryClinicById(int veterinaryClinicId);
    }
}
