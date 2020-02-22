using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DattingApp.API.Helper;
using DattingApp.DataLayer;
using DattingApp.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DattingApp.API.Controllers
{
    [Authorize]
    [Route("api/photos/{userId}/photo")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryconfig;
        private Cloudinary _cloudinary;
        DattingDAL objDattingDAL = new DattingDAL();
        PhotosDAL objPhotoDAL = new PhotosDAL();
        public PhotosController(IOptions<CloudinarySettings> cloudinaryconfig)
        {
            _cloudinaryconfig = cloudinaryconfig;

            Account acc = new Account(
                _cloudinaryconfig.Value.CloudName,
                _cloudinaryconfig.Value.ApiKey,
                _cloudinaryconfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }


        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<PhotoDto> GetPhoto(int id)
        {
            List<PhotoDto> listPhoto = new List<PhotoDto>();
            PhotoDto objPhoto = new PhotoDto();
            listPhoto = await objPhotoDAL.GetPhoto();
            objPhoto = listPhoto.FirstOrDefault(x => x.Id == id);
            return objPhoto;
        }

        [HttpPost("{id}/SetMainPhoto")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            int ReturnId = await objPhotoDAL.SetMainPhoto(userId, id);

            if (ReturnId == -2)
                return Unauthorized();

            if (ReturnId == -1)
                return BadRequest("This is already the main photo");

            if (ReturnId == 1)
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            List<PhotoDto> objphotoList = new List<PhotoDto>();
            PhotoDto objPhoto = new PhotoDto();
            bool isMainPhoto = false;
            int returnUserId = 0;

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            objphotoList = await objPhotoDAL.GetPhoto();

            objPhoto = objphotoList.FirstOrDefault(x => x.Id == id);

            if (objPhoto != null && objPhoto.Id > 0)
            {
                isMainPhoto = objPhoto.IsMain == true ? true : false;
            }

            if (isMainPhoto)
                return BadRequest("you could not delete main photo");

            if (objPhoto.PublicId != null)
            {
                var deleteParamas = new DeletionParams(objPhoto.PublicId);

                var results = _cloudinary.Destroy(deleteParamas);

                if (results.Result.ToLower() == "ok")
                {
                    returnUserId = await objPhotoDAL.DeletePhoto(id);
                }

                if (returnUserId > 0)
                    return Ok();
            }
            return BadRequest("Failed to delete the photo");
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int UserId, [FromForm]PhotoCreationDto ObjPhotoCreationDto)
        {
            List<UsersDto> listUserDto = new List<UsersDto>();
            UsersDto objUserDto = new UsersDto();
            PhotoDto objPhotoDto = new PhotoDto();
            var file = ObjPhotoCreationDto.File;
            var uploadResult = new ImageUploadResult();


            if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            listUserDto = await objDattingDAL.GetUserList();
            objUserDto = listUserDto.FirstOrDefault(U => U.id == UserId);

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadsParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadsParams);
                }
            }

            ObjPhotoCreationDto.Url = uploadResult.Uri.ToString();
            ObjPhotoCreationDto.PublicId = uploadResult.PublicId;

            objPhotoDto.PublicId = ObjPhotoCreationDto.PublicId;
            objPhotoDto.IsMain = false;
            objPhotoDto.Description = ObjPhotoCreationDto.Description;
            objPhotoDto.AddDate = ObjPhotoCreationDto.DateAdded;
            objPhotoDto.url = ObjPhotoCreationDto.Url;
            objPhotoDto.FkUserId = UserId;

            if (!objUserDto.Photos.Any(x => x.IsMain))
            {
                objPhotoDto.IsMain = true;
            }
            PhotoDto objPhoto = new PhotoDto();
            objPhoto = await objPhotoDAL.InsertPhoto(objPhotoDto);

            if (objPhoto != null && objPhoto.Id > 0)
            {
                return Ok(objPhoto);
            }

            return BadRequest("Could not add the photo");
        }
    }
}