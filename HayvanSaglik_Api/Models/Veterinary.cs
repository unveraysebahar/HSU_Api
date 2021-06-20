using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Models
{
    public class Veterinary
    {
        public Veterinary()
        {
            HealthInformations = new List<HealthInformation>();
        }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int VeterinaryClinicId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Task { get; set; }
        public string DiplomaNumber { get; set; }
        public Role Role { get; set; }
        public VeterinaryClinic VeterinaryClinic { get; set; }
        public List<HealthInformation> HealthInformations { get; set; }
    }
}
