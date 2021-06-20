using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Models
{
    public class Admin : AccountBase
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        // public string Email { get; set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }
    }
}
