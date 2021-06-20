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
    public class VeterinariansController : ControllerBase
    {
        private IEntityRepository _entityRepository;
        private IVeterinaryRepository _veterinaryRepository;
        private IMapper _mapper;

        public VeterinariansController(IEntityRepository entityRepository, IVeterinaryRepository veterinaryRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _veterinaryRepository = veterinaryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetVeterinarians()
        {
            var veterinarians = _veterinaryRepository.GetVeterinarians();
            var veterinariansToReturn = _mapper.Map<List<VeterinaryForListDto>>(veterinarians);

            return Ok(veterinariansToReturn);
        }

        [HttpPost]
        [Route("add")] // api/veterinarians/add
        public ActionResult Add([FromBody] Veterinary veterinary)
        {
            _entityRepository.Add(veterinary);
            _entityRepository.SaveAll();

            return Ok(veterinary);
        }

        [HttpGet]
        [Route("detail")] // api/veterinarians/detail/?veterinaryId=1
        public ActionResult GetVeterinaryById(int veterinaryId)
        {
            var veterinary = _veterinaryRepository.GetVeterinaryById(veterinaryId);
            var veterinaryToReturn = _mapper.Map<VeterinaryForDetailDto>(veterinary);

            return Ok(veterinaryToReturn);
        }

        [HttpGet]
        [Route("healthInformations")] // api/veterinarians/healthInformations/?veterinaryId=1
        public ActionResult GetInformationByVeterinary(int veterinaryId)
        {
            var healthInformation = _veterinaryRepository.GetHealthInformationsByVeterinary(veterinaryId);
            return Ok(healthInformation);
        }

        [HttpDelete]
        [Route("delete")] // api/veterinarians/delete/?veterinaryId=1
        public ActionResult Delete(int veterinaryId)
        {
            var veterinary = _veterinaryRepository.DeleteVeterinaryById(veterinaryId);
            _entityRepository.SaveAll();
            return Ok(veterinary);
        }
    }
}
