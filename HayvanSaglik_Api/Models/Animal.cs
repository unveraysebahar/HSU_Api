using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Models
{
    public class Animal
    {
        public Animal()
        {
            HealthInformations = new List<HealthInformation>();
            DateAdded = DateTime.Now;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsNeutered { get; set; }
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
        public List<HealthInformation> HealthInformations { get; set; }
    }
}
