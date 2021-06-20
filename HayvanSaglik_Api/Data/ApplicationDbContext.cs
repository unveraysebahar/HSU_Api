using HayvanSaglik_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Veterinary> Veterinarians { get; set; }
        public DbSet<VeterinaryClinic> VeterinaryClinics { get; set; }
        public DbSet<HealthInformation> HealthInformations { get; set; }
    }
}
