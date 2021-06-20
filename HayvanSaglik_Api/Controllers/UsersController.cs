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
    public class UsersController : ControllerBase
    {
        IUserRepository _userRepository;
        IConfiguration _configuration;
        private IMapper _mapper;

        public UsersController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("login")] // api/users/login
        public async Task<ActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var user = await _userRepository.Login(userForLoginDto.Email, userForLoginDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.Now.AddDays(100), // How many days will the token be valid?
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        [HttpPost("register")] // api/users/register
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            if (await _userRepository.UserExists(userForRegisterDto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new User
            {
                RoleId = 3,
                FullName = userForRegisterDto.FullName,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                Email = userForRegisterDto.Email,
            };
            var createdUser = await _userRepository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpGet]
        [Route("animals")] // api/users/animals/?userId=1
        public ActionResult GetAnimalsByUser(int userId)
        {
            var animals = _userRepository.GetAnimalsByUser(userId);

            return Ok(animals);
        }

        [HttpGet]
        [Route("detail")] // api/users/detail/?userId=1
        public ActionResult GetUsersById(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var userToReturn = _mapper.Map<UserForDetailDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("update")] // api/users/update/?userId=1
        public async Task<ActionResult<User>> UpdateUser(int userId, [FromBody] User user)
        {
            try
            {
                if (userId != user.Id)
                {
                    return BadRequest("User Id mismatch");
                }

                var userToUpdate = await _userRepository.GetUser(userId);

                if (userToUpdate == null)
                {
                    return NotFound($"User with Id = {userId} not found");
                }

                return await _userRepository.UpdateUser(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }
    }
}
