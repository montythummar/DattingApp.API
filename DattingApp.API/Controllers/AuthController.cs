using DattingApp.DataLayer;
using DattingApp.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        AuthDAL objAuthDAL = new AuthDAL();

        private IConfiguration configuration;
        public AuthController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UsersDto objUserDto)
        {
            int userId = 0;
            string userName = Convert.ToString(objUserDto.Username).ToLower();
            if (await isUserExist(userName))
            {
                return BadRequest("Username already exists");
            }

            byte[] PasswordHash, PasswordSalt;
            CreatePassword(objUserDto.Password, out PasswordHash, out PasswordSalt);
            objUserDto.PasswordHash = PasswordHash;
            objUserDto.PasswordSalt = PasswordSalt;

            userId = await objAuthDAL.Register(objUserDto);

            return StatusCode(201);
        }
        
        [HttpPost("Login")]        
        public async Task<IActionResult> Login([FromBody] UserLoginDto objUserLoginDto)
        {
            List<UsersDto> listUser = new List<UsersDto>();
            listUser = await GetUserList();
            var user = listUser.Find(x => x.Username.ToLower() == objUserLoginDto.Username.ToLower());
            if (user == null)

                return BadRequest("Unauthorized");
                

            if (!VerifyPassword(objUserLoginDto.Password, user.PasswordHash, user.PasswordSalt))

                return BadRequest("Unauthorized");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
                new Claim(ClaimTypes.Name,user.Username.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        public async Task<List<UsersDto>> GetUserList()
        {
            List<UsersDto> listUserDto = new List<UsersDto>();
            listUserDto = await objAuthDAL.GetUserList();
            return listUserDto;
        }

        private async Task<bool> isUserExist(string userName)
        {
            bool isUser = false;
            UsersDto objUser = new UsersDto();
            List<UsersDto> listUserDto = new List<UsersDto>();
            listUserDto = await GetUserList();
            isUser = listUserDto.Exists(x => x.Username.ToLower() == userName.ToLower());
            return isUser;
        }

        private bool VerifyPassword(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != PasswordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [AllowAnonymous]
        [HttpPost("CreatePasswordTemp")]
        public IActionResult CreatePasswordTemp([FromBody] UserLoginDto objUserLoginDto)
        {
            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(objUserLoginDto.Password));
            }
            return Ok(passwordSalt + "---" + passwordHash);
        }
    }
}