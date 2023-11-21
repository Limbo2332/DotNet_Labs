namespace NewsSite.DAL.DTO.Request
{
    public class NewNewsRequest
    {
        public Guid AuthorId { get; set; }

        public Guid? RubricId { get; set; }

        public List<Guid>? TagsIds { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
