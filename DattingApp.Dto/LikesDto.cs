using System;
using System.Collections.Generic;
using System.Text;

namespace DattingApp.Dto
{
    public class LikesDto
    {
        public int LikeId { get; set; }
        public int LikerId { get; set; }
        public int LikeeId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }        
    }
}
