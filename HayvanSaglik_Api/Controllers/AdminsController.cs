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
    public class AdminsController : ControllerBase
    {
        IAdminRepository _adminRepository;
        IConfiguration _configuration;
        private IMapper _mapper;

        IVeterinaryClinicRepository _veterinaryClinicRepository;

        public AdminsController(IAdminRepository adminRepository, IConfiguration configuration, IMapper mapper, IVeterinaryClinicRepository veterinaryClinicRepository)
        {
            _adminRepository = adminRepository;
            _configuration = configuration;
            _mapper = mapper;
            _veterinaryClinicRepository = veterinaryClinicRepository;
        }

        [HttpPost("login")] // api/admins/login
        public async Task<ActionResult> Login([FromBody] AdminForLoginDto adminForLoginDto)
        {
            var admin = await _adminRepository.Login(adminForLoginDto.Email, adminForLoginDto.Password);

            if (admin == null)
            {
                return Unauthorized();
            }

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.Email, admin.Email)
                }),
                Expires = DateTime.Now.AddDays(100), // How many days will the token be valid?
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        [HttpPost("register")] // api/admins/register
        public async Task<IActionResult> Register([FromBody] AdminForRegisterDto adminForRegisterDto)
        {
            if (await _adminRepository.AdminExists(adminForRegisterDto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminToCreate = new Admin
            {
                RoleId = 1,
                Email = adminForRegisterDto.Email,
            };
            var createdAdmin = await _adminRepository.Register(adminToCreate, adminForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPut("updateVeterinaryClinicRole")] // api/admins/updateVeterinaryClinicRole/?veterinaryClinicId=1
        public async Task<ActionResult<VeterinaryClinic>> UpdateVeterinaryClinicRole(int veterinaryClinicId, [FromBody] VeterinaryClinic veterinaryClinic)
        {
            try
            {
                if(veterinaryClinicId != veterinaryClinic.Id)
                {
                    return BadRequest("VeterinaryClinic Id mismatch");
                }

                var veterinaryClinicToUpdate = await _veterinaryClinicRepository.GetVeterinaryClinic(veterinaryClinicId);

                if(veterinaryClinicToUpdate == null)
                {
                    return NotFound($"Veterinary clinic with Id = {veterinaryClinicId} not found");
                }

                return await _veterinaryClinicRepository.UpdateVeterinaryClinic(veterinaryClinic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        [HttpGet]
        [Route("detail")] // api/admins/detail/?adminId=1
        public ActionResult GetAdminById(int adminId)
        {
            var admin = _adminRepository.GetAdminById(adminId);
            var adminToReturn = _mapper.Map<AdminForDetailDto>(admin);

            return Ok(adminToReturn);
        }
    }
}
