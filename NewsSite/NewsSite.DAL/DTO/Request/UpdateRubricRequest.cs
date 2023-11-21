namespace NewsSite.DAL.DTO.Request
{
    public class UpdateRubricRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
