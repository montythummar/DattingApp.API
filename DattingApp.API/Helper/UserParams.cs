namespace DattingApp.API.Helper
{
    public class UserParams
    {
        //private const int MaxPageSize = 50;
        //public int PageNumber { get; set; } = 1;
        //private int pageSize = 10;
        //public int PageSize
        //{
        //    get { return pageSize; }
        //    set { PageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        //}


        private const int MaxPageSize = 50;
        public int PageNumber { get; set; }       
        public int PageSize { get; set; }        
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }
    }
}
