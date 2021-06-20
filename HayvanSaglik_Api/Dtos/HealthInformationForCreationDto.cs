using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Dtos
{
    public class HealthInformationForCreationDto
    {
        public HealthInformationForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
