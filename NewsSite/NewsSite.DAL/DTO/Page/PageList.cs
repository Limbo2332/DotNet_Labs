namespace NewsSite.DAL.DTO.Page
{
    public class PageList<T> where T : class
    {
        public const int MAX_PAGE_SIZE = 100;
        public const int DEFAULT_PAGE_SIZE = 30;
        private int _pageSize = DEFAULT_PAGE_SIZE;

        public List<T> Items { get; set; } = null!;

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
        }

        public bool HasNextPage => PageSize * PageNumber < TotalCount;

        public bool HasPreviousPage => PageNumber > 1;
    }
}
