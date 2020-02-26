using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DattingApp.Dto
{
    public class UsersDto
    {
        public int id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string Gender { get; set; }       
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime LastActive { get; set; }        
        public string Introduction { get; set; }        
        public string lookingFor { get; set; }        
        public string Interests { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public UsersDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
        public string PhotoUrl { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }   

    public class UserLoginDto
    {        
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {        
        public string Introduction { get; set; }
        public string lookingFor { get; set; }
        public string interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }        
    }

}
