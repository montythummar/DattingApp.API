using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DattingApp.API.Helper;
using DattingApp.DataLayer;
using DattingApp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        LikesDAL objLikesDAL = new LikesDAL();
        DattingDAL objDattingDAL = new DattingDAL();
        [HttpPost("AddLikes")]
        public async Task<IActionResult> AddLikes([FromBody] LikesDto objLikeDto)
        {
            var userId = objLikeDto.LikerId;
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
        
            var likeList = await objLikesDAL.GetLike();

            var like = likeList.FirstOrDefault(x => x.LikerId == objLikeDto.LikerId && x.LikeeId == objLikeDto.LikeeId);

            if (like != null)            
                return BadRequest("You already like this user");

            var user = await objDattingDAL.GetUserList();

            if (!user.Exists(x => x.id == userId))
            {
                return NotFound();
            }


            var likesId = await objLikesDAL.AddLikes(objLikeDto);

            return Ok();
        }

        [HttpPost("RemoveLikes")]
        public async Task<IActionResult> RemoveLikes([FromBody] LikesDto objLikeDto)
        {
            var userId = objLikeDto.LikerId;
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var likeList = await objLikesDAL.GetLike();

            var like = likeList.FirstOrDefault(x => x.LikerId == objLikeDto.LikerId && x.LikeeId == objLikeDto.LikeeId);

            if (like == null)
            {
                return BadRequest("User is not available for unlike");
            }

            var likesId = await objLikesDAL.RemoveLikes(objLikeDto);

            return Ok();
        }

        [HttpPost("UserLikeeList")]
        public async Task<IActionResult> UserLikeeList([FromBody] LikesDto objLikeDto)
        {
            var userId = objLikeDto.LikerId;
            objLikeDto.PageNumber = objLikeDto.PageNumber <= 0 ? 1 : objLikeDto.PageNumber;
            objLikeDto.PageSize = objLikeDto.PageSize <= 0 ? 5 : objLikeDto.PageSize;
            var likeeUser = await objLikesDAL.GetLikeeUserList(userId);

            var PageListUser = PagedList<UsersDto>.Create(likeeUser.AsQueryable(), objLikeDto.PageNumber, objLikeDto.PageSize);
            Response.AddPagination(PageListUser.CurrentPage, PageListUser.PageSize, PageListUser.TotalCount, PageListUser.TotalPages);

            return Ok(PageListUser);
        }

        [HttpPost("UserLikerList")]
        public async Task<IActionResult> UserLikerList([FromBody] LikesDto objLikeDto)
        {
            var userId = objLikeDto.LikerId;
            objLikeDto.PageNumber = objLikeDto.PageNumber <= 0 ? 1 : objLikeDto.PageNumber;
            objLikeDto.PageSize = objLikeDto.PageSize <= 0 ? 5 : objLikeDto.PageSize;
            var likerUser = await objLikesDAL.GetLikerUserList(userId);

            var PageListUser = PagedList<UsersDto>.Create(likerUser.AsQueryable(), objLikeDto.PageNumber, objLikeDto.PageSize);
            Response.AddPagination(PageListUser.CurrentPage, PageListUser.PageSize, PageListUser.TotalCount, PageListUser.TotalPages);

            return Ok(PageListUser);
        }
    }
}