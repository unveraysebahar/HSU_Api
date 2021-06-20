using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Dtos
{
    public class VeterinaryClinicForRegisterDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime LicenseDate { get; set; }
        public string LicenseNumber { get; set; }
        public string PaymentDepartmentNumber { get; set; }
        public string Password { get; set; }
    }
}
