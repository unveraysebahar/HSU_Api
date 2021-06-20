using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Models
{
    public class User : AccountBase
    {
        public User()
        {
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        // public string Email { get; set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
