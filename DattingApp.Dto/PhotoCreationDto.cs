﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DattingApp.Dto
{
    public class PhotoCreationDto
    {
        public int FkUserId { get; set; }
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public PhotoCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}
