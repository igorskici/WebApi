using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRespository _repo;

        private readonly IConfiguration _config;

        public AuthController(IAuthRespository repo,  IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userforregister)
        {
            userforregister.Username = userforregister.Username.ToLower();

            if (await _repo.UserExist(userforregister.Username))
                return BadRequest("Username already exist");

            var usertoCreate = new User
            {
                Username = userforregister.Username,
                Created = DateTime.Now,
                Gender = userforregister.Gender
            };

            var createduser = await _repo.Register(usertoCreate, userforregister.Password);

            return StatusCode(201);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userforlogin)
        {
               
                var userfromRepo = await _repo.Login(userforlogin.Username.ToLower(), userforlogin.Password);


                if (userfromRepo == null)
                    return Unauthorized();

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userfromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userfromRepo.Username.ToString())
                };
  
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescr = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescr);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }  
        }
    }
