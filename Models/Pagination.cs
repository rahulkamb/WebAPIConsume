namespace APIConsume.Models
{
    public sealed class Pagination
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public Pagination()
        {
            
        }

        public Pagination(int totalItems, int page, int pageSize)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            int currentPage = page;

            int startPage = currentPage - 1;
            int endPage = currentPage + 1;

            if(startPage <=0)
            {                
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if(endPage > totalPages)
            {
                endPage = totalPages;
                if(endPage > 100){
                    startPage = endPage - 9;
                }                
            }

            TotalItems = totalItems;
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
