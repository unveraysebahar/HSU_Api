using AutoMapper;
using HayvanSaglik_Api.Dtos;
using HayvanSaglik_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Animal, AnimalForListDto>().ForMember(dest => dest.HealthInfoUrl, opt =>
            {
                opt.MapFrom(src => src.HealthInformations.FirstOrDefault(h => h.IsMain).Url);
            });

            CreateMap<Veterinary, VeterinaryForListDto>().ForMember(dest => dest.HealthInfoUrl, opt =>
            {
                opt.MapFrom(src => src.HealthInformations.FirstOrDefault(h => h.IsMain).Url);
            });

            CreateMap<Admin, AdminForDetailDto>();

            CreateMap<Animal, AnimalForDetailDto>();

            CreateMap<Veterinary, VeterinaryForDetailDto>();

            CreateMap<VeterinaryClinic, VeterinaryClinicForDetailDto>();

            // CreateMap<User, UserForDetailDto>();

            CreateMap<HealthInformationForCreationDto, HealthInformation>();

            CreateMap<HealthInformationForReturnDto, HealthInformation>();
        }
    }
}
