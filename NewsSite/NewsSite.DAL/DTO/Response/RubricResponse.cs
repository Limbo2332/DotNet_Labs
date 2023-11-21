namespace NewsSite.DAL.DTO.Response
{
    public class RubricResponse
    {
        public Guid Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
