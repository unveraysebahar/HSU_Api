using AutoMapper;
using HayvanSaglik_Api.Data;
using HayvanSaglik_Api.Dtos;
using HayvanSaglik_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private IEntityRepository _entityRepository;
        private IAnimalRepository _animalRepository;
        private IMapper _mapper;

        public AnimalsController(IEntityRepository entityRepository, IAnimalRepository animalRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _animalRepository = animalRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAnimals()
        {
            // var animals = _entityRepository.GetAnimals().Select(a => a.Name);

            /**
            var animals = _entityRepository.GetAnimals().Select(a => new AnimalForListDto
            {
                Id = a.Id,
                Name = a.Name,
                Gender = a.Gender,
                Type = a.Type,
                Breed = a.Breed,
                HealthInfoUrl = a.HealthInformations.FirstOrDefault(h => h.IsMain == true).Url
            }).ToList();
            **/

            var animals = _animalRepository.GetAnimals();
            var animalsToReturn = _mapper.Map<List<AnimalForListDto>>(animals);

            return Ok(animalsToReturn);
        }

        [HttpPost]
        [Route("add")] // api/animals/add
        public ActionResult Add([FromBody] Animal animal)
        {
            _entityRepository.Add(animal);
            _entityRepository.SaveAll();

            return Ok(animal);
        }

        [HttpGet]
        [Route("detail")] // api/animals/detail/?animalId=1
        public ActionResult GetAnimalById(int animalId)
        {
            var animal = _animalRepository.GetAnimalById(animalId);
            var animalToReturn = _mapper.Map<AnimalForDetailDto>(animal);

            return Ok(animalToReturn);
        }

        [HttpGet]
        [Route("healthInformations")] // api/animals/healthInformations/?animalId=1
        public ActionResult GetHealthInformationsByAnimal(int animalId)
        {
            var healthInformations = _animalRepository.GetHealthInformationsByAnimal(animalId);
            return Ok(healthInformations);
        }

        [HttpDelete]
        [Route("delete")] // api/animals/delete/?animalId=1
        public ActionResult Delete(int animalId)
        {
            var animal = _animalRepository.DeleteAnimalById(animalId);
            _entityRepository.SaveAll();
            return Ok(animal);
        }

        [HttpPut("updateAnimal")] // api/animals/updateAnimal/?animalId=1
        public async Task<ActionResult<Animal>> UpdateAnimal(int animalId, [FromBody] Animal animal)
        {
            try
            {
                if (animalId != animal.Id)
                {
                    return BadRequest("Animal Id mismatch");
                }

                var animalToUpdate = await _animalRepository.GetAnimal(animalId);

                if (animalToUpdate == null)
                {
                    return NotFound($"Animal with Id = {animalId} not found");
                }

                return await _animalRepository.UpdateAnimal(animal);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }
    }
}
