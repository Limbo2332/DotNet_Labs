namespace NewsSite.DAL.DTO.Response
{
    public class NewsResponse
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
