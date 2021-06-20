using AutoMapper;
using HayvanSaglik_Api.Data;
using HayvanSaglik_Api.Dtos;
using HayvanSaglik_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinaryClinicsController : ControllerBase
    {
        IVeterinaryClinicRepository _veterinaryClinicRepository;
        IEntityRepository _entityRepository;
        IConfiguration _configuration;
        private IMapper _mapper;

        public VeterinaryClinicsController(IVeterinaryClinicRepository veterinaryClinicRepository, IConfiguration configuration, IMapper mapper, IEntityRepository entityRepository)
        {
            _veterinaryClinicRepository = veterinaryClinicRepository;
            _configuration = configuration;
            _mapper = mapper;
            _entityRepository = entityRepository;
        }

        [HttpGet]
        public ActionResult GetVeterinaryClinics()
        {
            var veterinaryClinics = _veterinaryClinicRepository.GetVeterinaryClinics();
            var veterinaryClinicsToReturn = _mapper.Map<List<VeterinaryClinicForListDto>>(veterinaryClinics);

            return Ok(veterinaryClinicsToReturn);
        }

        [HttpPost("login")] // api/veterinaryClinics/login
        public async Task<ActionResult> Login([FromBody] VeterinaryClinicForLoginDto veterinaryClinicForLoginDto)
        {
            var veterinaryClinic = await _veterinaryClinicRepository.Login(veterinaryClinicForLoginDto.Email, veterinaryClinicForLoginDto.Password);

            if (veterinaryClinic == null)
            {
                return Unauthorized();
            }

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, veterinaryClinic.Id.ToString()),
                    new Claim(ClaimTypes.Email, veterinaryClinic.Email)
                }),
                Expires = DateTime.Now.AddDays(100), // How many days will the token be valid?
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        [HttpPost("register")] // api/veterinaryClinics/register
        public async Task<IActionResult> Register([FromBody] VeterinaryClinicForRegisterDto veterinaryClinicForRegisterDto)
        {
            if (await _veterinaryClinicRepository.VeterinaryClinicExists(veterinaryClinicForRegisterDto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var veterinaryClinicToCreate = new VeterinaryClinic
            {
                RoleId = 5,
                Name = veterinaryClinicForRegisterDto.Name,
                Address = veterinaryClinicForRegisterDto.Address,
                PhoneNumber = veterinaryClinicForRegisterDto.PhoneNumber,
                Email = veterinaryClinicForRegisterDto.Email,
                LicenseDate = veterinaryClinicForRegisterDto.LicenseDate,
                LicenseNumber = veterinaryClinicForRegisterDto.LicenseNumber,
                PaymentDepartmentNumber = veterinaryClinicForRegisterDto.PaymentDepartmentNumber,
            };
            var createdVeterinaryClinic = await _veterinaryClinicRepository.Register(veterinaryClinicToCreate, veterinaryClinicForRegisterDto.Password);

            return StatusCode(201); // Created success
        }

        [HttpGet("veterinarians")] // api/veterinaryClinics/veterinarians/?veterinaryClinicId=1
        public ActionResult GetAnimalsByUser(int veterinaryClinicId)
        {
            var veterinarians = _veterinaryClinicRepository.GetVeterinariansByVeterinaryClinic(veterinaryClinicId);

            return Ok(veterinarians);
        }

        [HttpGet]
        [Route("detail")] // api/veterinaryClinics/detail/?veterinaryClinicId=1
        public ActionResult GetVeterinaryClinicById(int veterinaryClinicId)
        {
            var veterinaryClinic = _veterinaryClinicRepository.GetVeterinaryClinicById(veterinaryClinicId);
            var veterinaryClinicToReturn = _mapper.Map<VeterinaryClinicForDetailDto>(veterinaryClinic);

            return Ok(veterinaryClinicToReturn);
        }

        [HttpDelete]
        [Route("delete")] // api/veterinaryClinics/delete/?veterinaryClinicId=1
        public ActionResult Delete(int veterinaryClinicId)
        {
            var veterinaryClinic = _veterinaryClinicRepository.DeleteVeterinaryClinicById(veterinaryClinicId);
            _entityRepository.SaveAll();
            return Ok(veterinaryClinic);
        }
    }
}
