using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HayvanSaglik_Api.Data;
using HayvanSaglik_Api.Dtos;
using HayvanSaglik_Api.Helpers;
using HayvanSaglik_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Controllers
{
    // [Route("api/animals/{animalId}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthInformationsController : ControllerBase
    {
        private IEntityRepository _entityRepository;
        private IAnimalRepository _animalRepository;
        private IHealthInformationRepository _healthInformationRepository;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public HealthInformationsController(IEntityRepository entityRepository, IAnimalRepository animalRepository, IHealthInformationRepository healthInformationRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _entityRepository = entityRepository;
            _animalRepository = animalRepository;
            _healthInformationRepository = healthInformationRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        public ActionResult AddHealthInformationForAnimal(int animalId, [FromBody] HealthInformation healthInformation)
        {
            var animal = _animalRepository.GetAnimalById(animalId);

            _entityRepository.Add(healthInformation);
            _entityRepository.SaveAll();

            return Ok(healthInformation);
        }

        [HttpGet("{id}", Name = "GetHealthInformation")]
        public ActionResult GetHealthInformation(int id)
        {
            var healthInformationFromDb = _healthInformationRepository.GetHealthInformationById(id);
            var healthInformation = _mapper.Map<HealthInformationForReturnDto>(healthInformationFromDb);

            return Ok(healthInformation);
        }

        [HttpDelete]
        [Route("delete")] // api/healthInformations/delete/?healthInformationId=1
        public ActionResult Delete(int healthInformationId)
        {
            var healthInformation = _healthInformationRepository.DeleteHealthInformationById(healthInformationId);
            _entityRepository.SaveAll();
            return Ok(healthInformation);
        }

        [HttpPost]
        [Route("add")] // api/healthInformations/add
        public ActionResult Add([FromBody] HealthInformation healthInformation)
        {
            _entityRepository.Add(healthInformation);
            _entityRepository.SaveAll();

            return Ok(healthInformation);
        }
    }
}
