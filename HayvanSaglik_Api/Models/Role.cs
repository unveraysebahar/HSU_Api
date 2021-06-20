using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Models
{
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
            Veterinarians = new List<Veterinary>();
            VeterinaryClinics = new List<VeterinaryClinic>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public List<User> Users { get; set; }
        public List<Veterinary> Veterinarians { get; set; }
        public List<VeterinaryClinic> VeterinaryClinics { get; set; }
    }
}
