using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Models
{
    public class HealthInformation
    {
        public HealthInformation()
        {
            IsMain = true;
            DateAdded = DateTime.Now;
            PublicId = "1";
        }
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public int VeterinaryId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }
        public Animal Animal { get; set; }
        public Veterinary Veterinary { get; set; }
    }
}
