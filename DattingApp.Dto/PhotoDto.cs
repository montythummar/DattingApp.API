using System;

namespace DattingApp.Dto
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string Description { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsMain { get; set; }
    }
}
