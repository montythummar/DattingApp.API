using DattingApp.DataLayer;
using DattingApp.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DattingController : ControllerBase
    {
        DattingDAL objDattingDAL = new DattingDAL();

        [HttpGet("GetUserList")]
        public async Task<List<UsersDto>> GetUserList()
        {
            List<UsersDto> listUserDto = new List<UsersDto>();
            listUserDto = await objDattingDAL.GetUserList();
            return listUserDto;
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<UsersDto> GetUserById(int id)
        {
            List<UsersDto> listUserDto = new List<UsersDto>();
            UsersDto objUserDto = new UsersDto();
            listUserDto = await objDattingDAL.GetUserList();
            objUserDto = listUserDto.FirstOrDefault(U => U.id == id);
            return objUserDto;
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto objUser)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            int UserId = await objDattingDAL.UpdateUser(id,objUser);

            if(UserId > 0)
         
                return NoContent();
            

           throw new Exception($"Updating user {id} faild on save");
        }

    }
}