using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Dtos
{
    public class VeterinaryForListDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int VeterinaryClinicId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Task { get; set; }
        public string DiplomaNumber { get; set; }
        public string Url { get; set; }
        public string HealthInfoUrl { get; set; }
    }
}
