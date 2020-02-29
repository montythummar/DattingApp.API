using DattingApp.API.Helper;
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
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DattingController : ControllerBase
    {
        DattingDAL objDattingDAL = new DattingDAL();

        [HttpGet("GetUserList")]
        public async Task<ActionResult> GetUserList([FromQuery] UserParams userParams)
        {
            userParams.PageNumber = userParams.PageNumber <= 0 ? 1 : userParams.PageNumber;
            userParams.PageSize = userParams.PageSize <= 0 ? 5 : userParams.PageSize;

            var UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<UsersDto> listUserDto = new List<UsersDto>();
            listUserDto = await objDattingDAL.GetUserList();
            listUserDto = listUserDto.OrderByDescending(x => x.LastActive).ToList();
            var currentUser = listUserDto.FirstOrDefault(x => x.id == UserId);
            userParams.UserId = UserId;

            if (!string.IsNullOrEmpty(userParams.Gender) && userParams.Gender != "ALL")
            {
                listUserDto = listUserDto.Where(x => x.Gender == userParams.Gender).ToList();
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var MinDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var MaxDob = DateTime.Today.AddYears(-userParams.MinAge);

                listUserDto = listUserDto.Where(x => x.DateOfBirth >= MinDob && x.DateOfBirth <= MaxDob).ToList();
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        listUserDto = listUserDto.OrderByDescending(x => x.Created).ToList();
                        break;
                    default:
                        listUserDto = listUserDto.OrderByDescending(x => x.LastActive).ToList();
                        break;
                }
            }

            var PageListUser = PagedList<UsersDto>.Create(listUserDto.AsQueryable(), userParams.PageNumber, userParams.PageSize);

            Response.AddPagination(PageListUser.CurrentPage, PageListUser.PageSize, PageListUser.TotalCount, PageListUser.TotalPages);

            return Ok(PageListUser);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<UsersDto> GetUserById(int id)
        {
            List<UsersDto> listUserDto = new List<UsersDto>();
            UsersDto ObjUser = new UsersDto();
            listUserDto = await objDattingDAL.GetUserList();
            ObjUser = listUserDto.FirstOrDefault(x => x.id == id);
            return ObjUser;
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto objUser)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            int UserId = await objDattingDAL.UpdateUser(id, objUser);

            if (UserId > 0)

                return NoContent();


            throw new Exception($"Updating user {id} faild on save");
        }

    }
}