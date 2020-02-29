namespace DattingApp.API.Helper
{
    public class PaginationHeader
    {       
        public int CurrentPage { get; set; }
        public int ItemsPerPages { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public PaginationHeader(int currentPage, int itemsPages, int totalItems, int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPerPages = itemsPages;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

    }
}
