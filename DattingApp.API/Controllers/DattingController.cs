using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DattingApp.DataLayer;
using DattingApp.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }
}