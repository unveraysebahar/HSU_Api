using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Dtos
{
    public class AnimalForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsNeutered { get; set; }
        public string Url { get; set; }
        public string HealthInfoUrl { get; set; }
    }
}
