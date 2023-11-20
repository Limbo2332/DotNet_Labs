namespace NewsSite.DAL.DTO.Page
{
    public class PageList<T> where T : class
    {
        public const int MAX_PAGE_SIZE = 100;
        private int _pageSize = 30;

        public List<T> Items { get; set; } = null!;

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
        }

        public bool HasNextPage => PageSize * PageNumber < TotalCount;

        public bool HasPreviousPage => PageSize > 1;
    }
}
